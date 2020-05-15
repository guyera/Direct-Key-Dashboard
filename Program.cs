using System;
using DirectKeyDashboard.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DirectKeyDashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #if UNIT_TESTS
            // Register desired tests and run them
            ProjectionTesting.Register();
            SummaryTesting.Register();
            UnitTestRegistry.GetDefault().RunTests();
            #endif

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
