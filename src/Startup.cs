using System;
using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Vinyl.Controllers;
using Vinyl.Data.Contexts;
using Vinyl.Data;
using Vinyl.Models;
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
            var modelMapper = new GenericModelFactory().Then(fac =>
                {
                    fac.AddMapping(typeof(ValueWrapper<>), (type, gtype) =>
                    {
                        var constructorInfo = type.GetConstructor(new[] { gtype });
                        return val => constructorInfo.Invoke(new[] {Convert.ChangeType(val, gtype)});
                    });

                    fac.AddMapping(typeof(TypesafeEnum<>), (type, gtype) =>
                    {
                        var methodInfo = type.GetMethod(nameof(Genre.FromKey), new[] { gtype });
                        return val => methodInfo.Invoke(null, new[] {Convert.ChangeType(val, gtype)});
                    });
                });

            services.AddDbContext<VinylContext>(options => options.UseInMemoryDatabase(databaseName: "Vinyl"));
            services.AddTransient<GetRecordsAction>();

            services
                .AddMvc(config =>
                {
                    config.ModelBinderProviders.Insert(0, new GenericModelBinderProvider(modelMapper));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new ValueWrapperJsonConverter(modelMapper));
                    options.SerializerSettings.Converters.Add(new TypesafeEnumJsonConverter(modelMapper));
                });

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<MyOperationFilter>();
                c.SwaggerDoc("v1", new Info { Title = "My Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, VinylContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            DbInitializer.Initialize(context);

            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }

    public class TypesafeEnumJsonConverter : CachingJsonConverter
    {
        private GenericModelFactory GenericModelFactory { get; }

        public TypesafeEnumJsonConverter(GenericModelFactory genericModelFactory)
        {
            GenericModelFactory = genericModelFactory;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return GenericModelFactory.Map(objectType, existingValue);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var typesafeEnum = value as ITypesafeEnum;
            if (typesafeEnum != null)
            {
                writer.WriteValue(typesafeEnum.Key);
            }
        }

        public override bool CanConvertCached(Type type)
        {
            return GenericModelFactory.CanMap(type, typeof(TypesafeEnum<>)) &&
                   type.IsSubclassOfGenericType(typeof(TypesafeEnum<>));
        }
    }

    public abstract class CachingJsonConverter : JsonConverter
    {
        private ConcurrentDictionary<Type, bool> Cache { get; } = new ConcurrentDictionary<Type, bool>();
        public abstract bool CanConvertCached(Type type);
        public override bool CanConvert(Type type)
        {
            return Cache.GetOrAdd(type, CanConvertCached);
        }
    }

    public class ValueWrapperJsonConverter : CachingJsonConverter
    {
        private GenericModelFactory GenericModelFactory { get; }

        public ValueWrapperJsonConverter(GenericModelFactory genericModelFactory)
        {
            GenericModelFactory = genericModelFactory;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueWrapper = value as IValueWrapper;
            if (valueWrapper != null)
            {
                writer.WriteValue(valueWrapper.Value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return GenericModelFactory.Map(objectType, existingValue);
        }

        public override bool CanConvertCached(Type type)
        {
            return GenericModelFactory.CanMap(type, typeof(ValueWrapper<>)) &&
                   type.IsSubclassOfGenericType(typeof(ValueWrapper<>));
        }
    }

    public class MyOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            return;
        }
    }
}
