﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;

namespace InConsoleApp
{
    class Program
    {
        private static IServiceProvider _servicesProvider;

        static void Main(string[] args)
        {
            SetUp();
            Log.Information($"Started: {DateTime.Now.ToShortTimeString()}");

            try
            {
                var ci = _servicesProvider.GetService<ConfigInterpreter>();
                ci.DescribeSettings();

                Log.Debug("Disposing ...");
                DisposeServices();
                Log.Information("Finished Program.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred.");
            }
            finally
            {
                //Log.CloseAndFlush();

                Log.Debug("Disposing ...");
                DisposeServices();
                Log.Information("Finishing Program.");
            }

        }

        static void SetUp()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets(typeof(Program).Assembly)
                .Build();

            var services = ConfigureServicesProvider(config);
            services.AddSingleton(config);
            _servicesProvider = services.BuildServiceProvider();

            var lc = new LoggerConfiguration().MinimumLevel.Information();
            Log.Logger = lc
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("InConsoleApp", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    "log.txt",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1)
                 )
                .CreateLogger();
        }
        private static IServiceCollection ConfigureServicesProvider(IConfiguration config)
        {
            var services = new ServiceCollection();

            services.AddTransient<ConfigInterpreter>();

            services.AddLogging(configure =>
                configure.AddSerilog()
            )
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug)
            ;

            return services;
        }

        private static void DisposeServices()
        {
            if (_servicesProvider != null)
            {
                if (_servicesProvider is IDisposable)
                {
                    ((IDisposable)_servicesProvider).Dispose();
                }
            }
        }
    }
}
