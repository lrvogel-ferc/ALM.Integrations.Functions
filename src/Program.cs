
using ALM.Integrations.Common.Constants;
using ALM.Integrations.Services;
using ALM.Integrations.Services.Apis;
using ALM.Integrations.Services.Handlers;

namespace ALM.Integrations.Functions;
internal class Program
{
    static void Main(string[] args)
    {
        
        var host = new HostBuilder()
            //.ConfigureFunctionsWorkerDefaults()
            .ConfigureFunctionsWebApplication()
            .ConfigureServices((appBuilder, services) =>
            {
                
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();
                
                // internal/support services
                services.AddSingleton<XrmService>();
                services.AddScoped<AtmsWebApi>();

                // Function services
                services.AddTransient<WorkItemService>();

                // http clients
                services.AddHttpClient(ClientFactories.AtmsWebApi, client =>
                {
                   client.BaseAddress = 
                    new System.Uri(appBuilder.Configuration[ConfigKeys.AtmsWebApiBaseUrl]);
                });//.AddHttpMessageHandler<AtmsWebApiHandler>();
            })
            .Build();

        host.Run();
    }
}
