using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DirectKeyDashboard.Charting.Domain
{
    public class ProjectionModelBinderProvider : IModelBinderProvider {
        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            // This is for polymorphic binding of projections. It is only necessary when attempting
            // to bind to an abstract model type, Projection. It is done by figuring out which dynamic
            // type it should represent through its SubtypeName member variable.
            // If the object type is not abstract, this provider should return null.

            var curType = context.Metadata.ModelType;
            while (curType != null && curType != typeof(object) && curType.IsAbstract) {
                // Iterate upwards through the class hierarchy until we hit the root (object)
                if (curType.IsGenericType && curType.GetGenericTypeDefinition() == typeof(Projection<>)) {
                    // We found the Projection<> class, thus this type must be a descendent of it
                    // So we can certainly bind it using the ProjectionModelBinder
                    return new ProjectionModelBinder(context);
                }
                curType = curType.BaseType; // Keep iterating up the class hierarchy
            }
            return null; // We failed to find the Projection class; return null
        }
    }
}