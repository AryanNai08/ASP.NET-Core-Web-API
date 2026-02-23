using CollegeApi.Configurations;
using CollegeApi.Data;
using CollegeApi.Data.Repository;
using CollegeApi.MyLogging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Runtime.CompilerServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Minute)
    .CreateLogger();

//use serilog along with built-in logger
//builder.Logging.AddSerilog();


//use this line to override the built-in logger
//builder.Host.UseSerilog();



//db configuration
builder.Services.AddDbContext<CollegeDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"));
});

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// 🔵 Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//auto mapper configuration
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddScoped<IStudentRepository,StudentRepository>();

builder.Services.AddScoped(typeof(ICollegeRepository<>),typeof(CollegeRepository<>));


//added cors
builder.Services.AddCors(options =>
{
    // Named Policy: AllowAll - permits any origin
    options.AddPolicy("AllowAll", policy =>
    {
        // For all origins
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

    // Named Policy: AllowOnlyLocalhost - permits only localhost
    options.AddPolicy("AllowOnlyLocalhost", policy =>
    {
        // For specific origin
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });

    // Named Policy: AllowOnlyGoogle - permits Google domains
    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        // For specific origins
        policy.WithOrigins("http://google.com", "http://gmail.com", "http://drive.google.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    // Named Policy: AllowOnlyMicrosoft - permits Microsoft domains
    options.AddPolicy("AllowOnlyMicrosoft", policy =>
    {
        // For specific origins
        policy.WithOrigins("http://outlook.com", "http://microsoft.com", "http://onedrive.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    // Default Policy (uncomment to use)
    // options.AddDefaultPolicy(policy =>
    // {
    //     policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    // });
});


var JWTSecretForLocal = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForLocal"));

var JWTSecretForGoogle = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForGoogle"));

var JWTSecretForMicrosoft = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForMicrosoft"));



//Add Authentication Configuration (Default)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LoginForGoogleUsers",options =>
{
    //but make it false in production environment
    //options.RequireHttpsMetadata = false;

    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // validate the signing key
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(JWTSecretForGoogle),
        ValidateIssuer = false,
        ValidateAudience =false
    };
}).AddJwtBearer("LoginForLocalUsers", options =>
{
    //but make it false in production environment
    //options.RequireHttpsMetadata = false;

    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // validate the signing key
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(JWTSecretForLocal),
        ValidateIssuer = false,
        ValidateAudience = false
    };
}).AddJwtBearer("LoginForMicrosoftUsers", options =>
{
    //but make it false in production environment
    //options.RequireHttpsMetadata = false;

    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // validate the signing key
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(JWTSecretForMicrosoft),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


// Singleton: Same instance throughout the app
//builder.Services.AddSingleton<IMyLogger, LogToFile>();

// Scoped: New instance per HTTP request
builder.Services.AddScoped<IMyLogger, LogToDB>();

// Transient: New instance every time DI container is asked
//builder.Services.AddTransient<IMyLogger, LogToServerMemory>();


var app = builder.Build();

// 🔵 Enable Swagger only in development (best practice)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//this line need to be added after routes and before authorization
app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testingendpoint",
        context => context.Response.WriteAsync("Test Response"))
    .RequireCors("AllowOnlyLocalhost");

    endpoints.MapControllers().RequireCors("AllowAll");

    endpoints.MapGet("api/testendpoint2",
        //context => context.Response.WriteAsync("Test response 1"));
    context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecretForLocal")));
});

app.MapControllers();

app.Run();














