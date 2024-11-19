using FastCarSales.Client;
using FastCarSales.Client.Events;
using FastCarSales.Client.LocalService;
using FastCarSales.Data;
using FastCarSales.MapperConfigurations.Profiles;
using FastCarSales.Services;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Images;
using FastCarSales.Services.Posts;
using FastCarSales.Services.Statistics;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAutoMapper(typeof(CarsProfile).GetTypeInfo().Assembly);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<FastCarSalesDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddBlazorBootstrap();

//Make sure to register any service twice i.e also with server program.cs
builder.Services.AddTransient<ICarsService, CarsService>();
builder.Services.AddTransient<IPostsService, PostsService>();
builder.Services.AddTransient<IImagesService, ImagesService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<TestService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddSingleton<EventAggregator>();


builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

//builder.Services.AddLogging(config => { config.AddConsole(); config.AddDebug(); });

// optional: remove default logging providers
//builder.Logging.ClearProviders();


//// Configure Serilog
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//    .Enrich.FromLogContext().WriteTo.Console()
//    .WriteTo.File("Logs/myapp-.log", rollingInterval: RollingInterval.Day)
//    .CreateLogger(); 

//builder.Logging.AddSerilog();

await builder.Build().RunAsync();
