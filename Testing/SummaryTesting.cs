
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DirectKeyDashboard.Charting.Domain;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Testing {
    public class SummaryTesting {
        public static void Register() {
            UnitTestRegistry.GetDefault().Register(new UnitTest(TestAverageSummary, "Average summary"));
            UnitTestRegistry.GetDefault().Register(new UnitTest(TestMedianSummary, "Median summary"));
            UnitTestRegistry.GetDefault().Register(new UnitTest(TestCountSummary, "Count summary"));
        }

        private static void TestAverageSummary() {
            IList<float> nums = new List<float>() {
                1f, 5f, 12f, 100f, 13f
            };
            var summary = new AverageSummary();
            Util.Assert(Math.Abs(summary.Summarize(nums) - 26.2) <= 0.01);
        }

        private static void TestMedianSummary() {
            IList<float> nums = new List<float>() {
                1f, 5f, 12f, 100f, 13f
            };
            var summary = new MedianSummary();
            Util.Assert(Math.Abs(summary.Summarize(nums) - 12) <= 0.01);
        }

        private static void TestCountSummary() {
            IList<JObject> objs = new List<JObject>();
            objs.Add(new JObject());
            objs.Add(new JObject());
            objs.Add(new JObject());
            objs.Add(new JObject());
            objs.Add(new JObject());
            var summary = new CountSummary<JObject>();
            Util.Assert(Math.Abs(summary.Summarize(objs) - 5) <= 0.01);
        }
    }
}