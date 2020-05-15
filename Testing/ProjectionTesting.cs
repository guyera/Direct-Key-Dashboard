
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DirectKeyDashboard.Charting.Domain;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Testing {
    public class ProjectionTesting {
        public static void Register() {
            UnitTestRegistry.GetDefault().Register(new UnitTest(TestSimpleProjection, "Simple projection"));
        }

        private static void TestSimpleProjection() {
            IList<JObject> objects = new List<JObject>();

            JObject obj = new JObject();
            obj.Add("test", JToken.FromObject(5f));
            objects.Add(obj);

            obj = new JObject();
            obj.Add("test", JToken.FromObject(7f));
            objects.Add(obj);

            obj = new JObject();
            obj.Add("test", JToken.FromObject(100f));
            objects.Add(obj);

            obj = new JObject();
            obj.Add("test", JToken.FromObject(12f));
            objects.Add(obj);

            obj = new JObject();
            obj.Add("test", JToken.FromObject(15f));
            objects.Add(obj);

            obj = new JObject();
            obj.Add("test", JToken.FromObject(36f));
            objects.Add(obj);

            var projection = new SimpleProjection<float>("test");
            IList<float> intendedProjections = new List<float>() {
                5f, 7f, 100f, 12f, 15f, 36f
            };

            var itr = 0;
            foreach (var curObj in objects) {
                var value = projection.Project(curObj);
                Util.Assert(Math.Abs(value - intendedProjections[itr++]) <= 0.01);
            }
        }
    }
}