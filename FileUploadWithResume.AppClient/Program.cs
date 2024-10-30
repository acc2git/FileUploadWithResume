using FileUploadWithResume.AppClient.Api;
using FileUploadWithResume.AppClient.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileUploadWithResume.AppClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            var serviceProvider = InitServices();
            ApplicationConfiguration.Initialize();
            Application.Run(serviceProvider.GetRequiredService<Form1>());
        }

        static ServiceProvider InitServices()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddHttpClient("FileApi").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
            });
            services.AddSingleton<Form1>();
            services.AddSingleton<IFileApiClient, FileApiClient>();
            services.AddOptions();

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            services.Configure<FileApiClientOption>(configuration.GetSection("FileApiClientOption"));
            return services.BuildServiceProvider();
        }
    }
}