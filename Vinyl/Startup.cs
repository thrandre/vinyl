using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Vinyl.Actions;
using Vinyl.Data;
using Vinyl.Data.Contexts;
using Vinyl.Models;
using Vinyl.Models.Wrappers;
using Vinyl.Utils;

namespace Vinyl
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            TypeConverterInstance.Current
                .AddConverter<RecordId, int>(val => new RecordId(val), obj => obj.Value)
                .AddConverter<RecordTitle, string>(val => new RecordTitle(val), obj => obj.Value)
                .AddConverter<Genre, string>(Genre.FromKey, obj => obj.Value)
                .AddConverter<Year, int>(val => new Year(val), obj => obj.Value)
                .AddConverter<ArtistName, string>(val => new ArtistName(val), obj => obj.Value)
                .AddConverter<ArtistId, int>(val => new ArtistId(val), obj => obj.Value);

            services.AddDbContext<VinylContext>(options => options.UseInMemoryDatabase(databaseName: "Vinyl"));

            services.AddTransient<GetRecordsAction>();
            services.AddTransient<CreateRecordAction>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new BoxedTypeJsonConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<MyOperationFilter>();
                c.SchemaFilter<MySchemaFilter>();
                c.SwaggerDoc("v1", new Info { Title = "Vinyl API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, VinylContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => 
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());

            app.UseMvc();

            DbInitializer.Initialize(context);

            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }

    public class MySchemaFilter : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            if (!TypeConverterInstance.Current.CanConvert(context.SystemType))
            {
                return;
            }

            var schema = context.SchemaRegistry.GetOrRegister(TypeConverterInstance.Current.ConvertsTo(context.SystemType));

            model.Type = schema.Type;
            model.Format = schema.Format;
            model.Properties = new Dictionary<string, Schema>();

            if (!context.SystemType.IsSubclassOfGenericType(typeof(TypesafeEnum<>)))
            {
                return;
            }

            var foo = context.SystemType.GetMethod(nameof(Genre.GetAll)).Invoke(null, new object[] { });
            var bar = ((IEnumerable) foo).Cast<object>().Select(e => TypeConverterInstance.Current.ConvertFrom(context.SystemType, e));

            model.Enum = bar.ToList();
        }
    }

    public class MyOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor == null)
            {
                throw new InvalidOperationException("Usjda.");
            }

            operation.OperationId = $"{descriptor.ControllerName}_{descriptor.ActionName}";

            if (operation.Parameters == null || !operation.Parameters.Any())
            {
                return;
            }

            if (context.ApiDescription.HttpMethod == HttpMethod.Get.Method)
            {
                operation.Parameters
                    .OfType<NonBodyParameter>()
                    .Where(p => p.Type == null)
                    .ToList()
                    .ForEach(p =>
                    {
                        var apiParam = context.ApiDescription.ParameterDescriptions.First(pd => pd.Name == p.Name);

                        if (!TypeConverterInstance.Current.CanConvert(apiParam.Type))
                        {
                            return;
                        }

                        var schema = context.SchemaRegistry.GetOrRegister(apiParam.Type);
                        var fallbackSchema = context.SchemaRegistry.GetOrRegister(TypeConverterInstance.Current.ConvertsTo(apiParam.Type));

                        p.Type = fallbackSchema.Type;
                        p.Format = fallbackSchema.Format;
                        p.Extensions.Add("x-schema", schema);
                    });
            }
        }
    }
}
