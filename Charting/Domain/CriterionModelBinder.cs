using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DirectKeyDashboard.Charting.Domain {
    public class CriterionModelBinder : IModelBinder
    {
        private readonly ModelBinderProviderContext _providerContext;
        public CriterionModelBinder(ModelBinderProviderContext providerContext) {
            _providerContext = providerContext;
        }

        public CriterionModelBinder() {}

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var typeName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(Criterion.SubtypeName));
            var typeValue = bindingContext.ValueProvider.GetValue(typeName).FirstValue;
            var type = string.IsNullOrEmpty(typeValue) ? null : Type.GetType(typeValue.ToString(), true);
            // If the type is null, then the object is null (every instance must have a
            // SubtypeName). Fail to bind, in this case
            if (typeValue == null) {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            // Otherwise, create a new metadata and a new binding context for the subtype,
            // and recursively bind the subtype
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
            Console.WriteLine("Criterion model binder 2");
        }
    }
}