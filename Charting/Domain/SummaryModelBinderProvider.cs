using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class SummaryModelBinderProvider : IModelBinderProvider {
        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            if (!context.Metadata.ModelType.IsGenericType || context.Metadata.ModelType.GetGenericTypeDefinition() != typeof(Summary<,>)) {
                return null;
            }
            return new SummaryModelBinder(context);
        }
    }
}