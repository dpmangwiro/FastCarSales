using FastCarSales.Client;
using FastCarSales.Client.Events;
using FastCarSales.Client.LocalService;
using FastCarSales.Client.Pages;
using FastCarSales.Components;
using FastCarSales.Components.Account;
using FastCarSales.Data;
using FastCarSales.Data.Models;
using FastCarSales.Data.Seeding;
using FastCarSales.MapperConfigurations.Profiles;
using FastCarSales.Services;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Images;
using FastCarSales.Services.Posts;
using FastCarSales.Services.Statistics;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

//builder.Services.AddLogging(config => { config.AddConsole(); config.AddDebug(); });

//// Configure Serilog
//Log.Logger = new LoggerConfiguration()
//	.WriteTo.Console()
//	.WriteTo.File("Logs/myapp-.log", rollingInterval: RollingInterval.Day).CreateLogger();

//builder.Host.UseSerilog();

builder.Services.AddBlazorBootstrap();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Permission", "Admin"));

	//options.AddPolicy("AdminPolicy", policy => 
 //       policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == ClaimTypes.Email && (c.Value == "dpmangwiro@gmail.com" || c.Value == "fastcarsalesadmin@gmail.com"))));
});


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<FastCarSalesDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<FastCarSalesDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddAutoMapper(typeof(CarsProfile).GetTypeInfo().Assembly);

builder.Services.AddTransient<ICarsService, CarsService>();
builder.Services.AddTransient<IPostsService, PostsService>();
builder.Services.AddTransient<IImagesService, ImagesService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<TestService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddSingleton<EventAggregator>();

var baseAddress = builder.Configuration["BaseAddress"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => { builder.WithOrigins(baseAddress!).AllowAnyHeader().AllowAnyMethod(); });
});

// Access configuration settings
var environment = builder.Environment;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress!) });

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();

    app.Use((context, next) => { var circuitOptions = context.RequestServices.GetService<IOptions<CircuitOptions>>(); circuitOptions.Value.DetailedErrors = true; return next(); });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();

//user antiforegry must go between userouting and mapendpoints
app.UseAntiforgery();
//If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseCors() must go between them.
app.UseCors();

//If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseAuthorization() must go between them.
app.UseAuthorization();

//Configure your application startup by adding app.UseAuthorization() in the application startup code. If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseAuthorization() must go between them.
//Microsoft.AspNetCore.Routing.EndpointMiddleware.ThrowMissingAuthMiddlewareException(Endpoint endpoint)
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FastCarSales.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// Seed the database
await SeedDatabaseAsync(app.Services);
await RunClaimsInitializer(app.Services);
app.Run();

//----------------------------------------------------------------------------------------------

static async Task SeedDatabaseAsync(IServiceProvider services)
{
	using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<FastCarSalesDbContext>();

    await dbContext.Database.MigrateAsync();

    await new FastCarSalesDbContextSeeder().SeedAsync(dbContext, scope.ServiceProvider);	
}

static async Task RunClaimsInitializer(IServiceProvider services)
{
	await ClaimsInitializer.InitializeAsync(services);
}