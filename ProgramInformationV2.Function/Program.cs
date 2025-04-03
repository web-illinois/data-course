using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Search;
using ProgramInformationV2.Search.Getters;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureOpenApi()
    .ConfigureAppConfiguration((hostContext, config) => {
        if (hostContext.HostingEnvironment.IsDevelopment()) {
            _ = config.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((hostContext, services) => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        _ = services.AddApplicationInsightsTelemetryWorkerService();
        _ = services.ConfigureFunctionsApplicationInsights();
        _ = services.AddDbContextFactory<ProgramContext>(options => options.UseSqlServer(hostContext.Configuration["Values:AppConnection"]).EnableSensitiveDataLogging(true));
        _ = services.AddScoped<ProgramRepository>();
        _ = services.AddSingleton(c => OpenSearchFactory.CreateLowLevelClient(hostContext.Configuration["Values:SearchUrl"], hostContext.Configuration["Values:AccessKey"], hostContext.Configuration["Values:SecretKey"], hostContext.Configuration["Values:Debug"] == "true"));
        _ = services.AddSingleton(c => OpenSearchFactory.CreateClient(hostContext.Configuration["Values:SearchUrl"], hostContext.Configuration["Values:AccessKey"], hostContext.Configuration["Values:SecretKey"], true));
        _ = services.AddScoped<ProgramGetter>();
        _ = services.AddScoped<CredentialGetter>();
        _ = services.AddScoped<CourseGetter>();
        _ = services.AddScoped<RequirementSetGetter>();
        _ = services.AddScoped(b => new FilterHelper(b.GetService<ProgramRepository>(), null));
    })
    .Build();

host.Run();