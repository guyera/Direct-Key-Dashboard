using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DirectKeyDashboard.Charting.Domain
{
    public class CriterionModelBinderProvider : IModelBinderProvider {
        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            // This is for polymorphic binding of criteria. It is only necessary when attempting
            // to bind to an abstract criterion model type. It is done by figuring out which dynamic
            // type it should represent through its SubtypeName member variable.
            // If the object type is not abstract, this provider should return null.

            if (context.Metadata.ModelType == null || !context.Metadata.ModelType.IsAbstract ||
                    (!context.Metadata.ModelType.IsSubclassOf(typeof(Criterion)) && context.Metadata.ModelType != typeof(Criterion))) {
                return null;
            }
            return new CriterionModelBinder(context);
        }
    }
}