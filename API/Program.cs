using API.Db;
using API.Identity;
using API.Middleware;
using API.Models;
using API.Service.GoogleApi;
using API.Service.Mailing;
using API.Service.User;
using API.Utils.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Information("Server Booting Up...");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
{
    config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
});
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddExceptionMiddleware();
builder.Services.AddRequestLogging(builder.Configuration);
builder.Services.AddScoped<IGoogleSearchService, GoogleSearchService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<ISerializerService, SerializerService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleClaimsService, RoleClaimsService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddLocalization(builder.Configuration);
builder.Services.AddHttpContextAccessor();



builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());





//Cors
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Angular
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddControllers();


//Swagger Services + Authentication Services
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseLocalization(builder.Configuration);
app.UseStaticFiles();
//app.UseSecurityHeaders(builder.Configuration);
app.UseExceptionMiddleware();
//app.UseCorsPolicy();
app.UseRouting();
app.UseAuthentication();
app.UseCurrentUser();
app.UseAuthorization();
app.UseRequestLogging(builder.Configuration);
//app.UseEndpoints();
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("App is on developpement mode");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = String.Empty;
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "app v1");
    });
}

app.MapControllers();

app.Run();