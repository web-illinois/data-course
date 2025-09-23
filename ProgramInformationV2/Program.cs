using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using OpenSearch.Client;
using ProgramInformationV2.Components;
using ProgramInformationV2.Data.Cache;
using ProgramInformationV2.Data.CourseImport;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.Uploads;
using ProgramInformationV2.Search;
using ProgramInformationV2.Search.AuditHelpers;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Helpers;
using ProgramInformationV2.Search.Setters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options => {
    options.FallbackPolicy = options.DefaultPolicy;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddWebOptimizer(pipeline => {
    pipeline.AddJavaScriptBundle("/js/site.js", "/wwwroot/js/*.js").UseContentRoot();
    pipeline.AddCssBundle("/css/site.css", "/wwwroot/css/*.css").UseContentRoot();
});

builder.Services.AddScoped(b => new UploadStorage(builder.Configuration["AzureStorage"], builder.Configuration["AzureAccountName"], builder.Configuration["AzureAccountKey"], builder.Configuration["AzureImageContainerName"]));

builder.Services.AddDbContextFactory<ProgramContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")).EnableSensitiveDataLogging(true));
builder.Services.AddScoped<ProgramRepository>();
builder.Services.AddSingleton<CacheHolder>();
builder.Services.AddScoped<SourceHelper>();
builder.Services.AddScoped<FilterHelper>();
builder.Services.AddScoped<CourseImportHelper>();
builder.Services.AddScoped<LogHelper>();
builder.Services.AddScoped<SecurityHelper>();
builder.Services.AddScoped<FieldManager>();
builder.Services.AddScoped<FacultyNameCourseHelper>();
builder.Services.AddScoped<CourseImportManager>();
builder.Services.AddScoped<ProgramFieldItemMultipleDelete>();
builder.Services.AddSingleton(b => OpenSearchFactory.CreateClient(builder.Configuration["SearchUrl"], builder.Configuration["SearchAccessKey"], builder.Configuration["SearchSecretAccessKey"], bool.Parse(builder.Configuration["SearchDebug"] ?? "false")));
builder.Services.AddSingleton(b => OpenSearchFactory.CreateLowLevelClient(builder.Configuration["SearchUrl"], builder.Configuration["SearchAccessKey"], builder.Configuration["SearchSecretAccessKey"], bool.Parse(builder.Configuration["SearchDebug"] ?? "false")));
builder.Services.AddScoped<ProgramGetter>();
builder.Services.AddScoped<CredentialGetter>();
builder.Services.AddScoped<ProgramSetter>();
builder.Services.AddScoped<CourseGetter>();
builder.Services.AddScoped<CourseSetter>();
builder.Services.AddScoped<RequirementSetGetter>();
builder.Services.AddScoped<RequirementSetSetter>();
builder.Services.AddScoped<RequirementSetAudits>();
builder.Services.AddScoped<JsonHelper>();
builder.Services.AddScoped<BulkEditor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseWebOptimizer();

app.Lifetime.ApplicationStarted.Register(() => {
    var factory = app.Services.GetService<IServiceScopeFactory>() ?? throw new NullReferenceException("service scope factory is null");
    using var serviceScope = factory.CreateScope();
    // Ensure the database is created
    var context = serviceScope.ServiceProvider.GetRequiredService<ProgramContext>();
    _ = context.Database.EnsureCreated();
    // Ensure the search index is created
    var openSearchClient = serviceScope.ServiceProvider.GetRequiredService<OpenSearchClient>();
    Console.WriteLine(OpenSearchFactory.MapIndex(openSearchClient));
});

app.Run();