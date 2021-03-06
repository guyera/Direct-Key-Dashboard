using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For polymorphic model binding of summaries, mostly for
// drilldown charts
namespace DirectKeyDashboard.Charting.Domain
{
    public class SummaryModelBinderProvider : IModelBinderProvider {
        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            // This is for polymorphic binding of summaries. It is only necessary when attempting
            // to bind to an abstract model type. It is done by figuring out which dynamic
            // type it should represent through its SubtypeName member variable.
            // If the object type is not abstract, this provider should return null.
            
            var curType = context.Metadata.ModelType;
            while (curType != null && curType != typeof(object) && curType.IsAbstract) {
                // Iterate upwards through the class hierarchy until we hit the root (object)
                // or find the summary class
                if (curType.IsGenericType && curType.GetGenericTypeDefinition() == typeof(Summary<,>)) {
                    // We found the Projection<> class, thus this type must be a descendent of it
                    // So we can certainly bind it using the ProjectionModelBinder
                    Console.WriteLine($"Found match for summary<>: {context.Metadata.ModelType.FullName}");
                    return new SummaryModelBinder(context);
                }
                curType = curType.BaseType; // Keep iterating up the class hierarchy
            }
            return null; // We failed to find the Projection class; return null
        }
    }
}