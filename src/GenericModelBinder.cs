using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Vinyl
{
    public class GenericModelBinderProvider : IModelBinderProvider
    {
        private GenericModelFactory GenericModelFactory { get; }

        public GenericModelBinderProvider(GenericModelFactory genericModelFactory)
        {
            GenericModelFactory = genericModelFactory;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return GenericModelFactory.CanMap(context.Metadata.ModelType)
                ? new GenericModelBinder(GenericModelFactory)
                : null;
        }
    }

    public class GenericModelBinder : IModelBinder
    {
        private GenericModelFactory GenericModelFactory { get; }

        public GenericModelBinder(GenericModelFactory genericModelFactory)
        {
            GenericModelFactory = genericModelFactory;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            bindingContext.Result = ModelBindingResult.Success(GenericModelFactory.Map(bindingContext.ModelType, valueProviderResult.FirstValue));

            return Task.CompletedTask;
        }
    }
}
