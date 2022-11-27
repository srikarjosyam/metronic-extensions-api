using metronic_extensions_api;
using metronic_extensions_api.Auth;
using metronic_extensions_api.ErrorHandling;
using metronic_extensions_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// added swagger with the auth header
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Metronic Extensions API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Auth0"));

builder.Services.AddSingleton<TokenmgmtService>();

builder.Services.AddScoped<ManagementAPIService>();

builder.Services.AddHttpClient("MgmtAPI", c => c.BaseAddress = new Uri(builder.Configuration["Auth0:Audience"]));

//Adding Logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

var domain = builder.Configuration["Auth0:Domain"];
var keyResolver = new OpenIdConnectSigningKeyResolver(domain);

//builder.Services.AddAuthorization(o =>
//{
//    o.AddPolicy("todo:read-write", p => p.
//        RequireAuthenticatedUser().
//        RequireClaim("scope", "todo:read-write"));
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
     {
         c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
         c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidAudience = builder.Configuration["Auth0:Audience"],
             ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}/",
             IssuerSigningKeyResolver = (token, securityToken, kid, parameters) => keyResolver.GetSigningKey(kid),
             ValidateLifetime = true
         };
     });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Error Handler
app.UseErrorHandling();


app.MapControllers();

app.Run();
