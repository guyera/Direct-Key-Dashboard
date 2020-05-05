using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DirectKeyDashboard.Charting.Domain {
    public class SummaryModelBinder : IModelBinder
    {
        private readonly ModelBinderProviderContext _providerContext;
        public SummaryModelBinder(ModelBinderProviderContext providerContext) {
            _providerContext = providerContext;
        }

        public SummaryModelBinder() {}

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var typeName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(Summary.SubtypeName));
            var typeValue = bindingContext.ValueProvider.GetValue(typeName).FirstValue;
            //var typeValue = bindingContext.ValueProvider.GetValue($"{bindingContext.OriginalModelName}.ModelType");
            var type = Type.GetType(typeValue.ToString(), true);
            var modelMetadata = _providerContext.MetadataProvider.GetMetadataForType(type);
            var binder = _providerContext.CreateBinder(modelMetadata);
            var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                bindingContext.ActionContext,
                bindingContext.ValueProvider,
                modelMetadata,
                bindingInfo: null,
                bindingContext.ModelName);

            await binder.BindModelAsync(newBindingContext);
            bindingContext.Result = newBindingContext.Result;

            if (newBindingContext.Result.IsModelSet)
            {
                // Setting the ValidationState ensures properties on derived types are correctly 
                bindingContext.ValidationState[newBindingContext.Result] = new ValidationStateEntry
                {
                    Metadata = modelMetadata,
                };
            }
        }
    }
}