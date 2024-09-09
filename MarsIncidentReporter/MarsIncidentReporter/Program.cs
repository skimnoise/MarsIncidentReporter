using MarsIncidentReporter.Data;
using MarsIncidentReporter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add OAuth2 support in Swagger
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://{builder.Configuration["Auth0:Domain"]}/authorize"),
                TokenUrl = new Uri($"https://{builder.Configuration["Auth0:Domain"]}/oauth/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID Connect scope" },
                    { "profile", "Access to your profile information" },
                    { "email", "Access to your email address" }
                }
            }
        }
    });

    // Add security requirement to use OAuth2 for all API calls
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new List<string> { "openid", "profile", "email" }  // Specify the scopes required
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("https://localhost:7181", "https://localhost:7181/swagger", "https://localhost:7181/Account/Login")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Configure Authentication with Auth0
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; ;
  options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect("Auth0", options =>
{
  options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
  options.ClientId = builder.Configuration["Auth0:ClientId"];
  options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
  options.ResponseType = "code";

  // Configure the callback path for Auth0
  options.CallbackPath = new PathString("/callback");
  options.ClaimsIssuer = "Auth0";

  // Save tokens for access control
  options.SaveTokens = true;

  // Set scopes to request
  options.Scope.Clear();
  options.Scope.Add("openid");
  options.Scope.Add("profile");
  options.Scope.Add("email");

  // Set events for redirect to Auth0's login page
  options.Events = new OpenIdConnectEvents
  {
    OnRedirectToIdentityProvider = context =>
    {
      context.ProtocolMessage.SetParameter("audience", builder.Configuration["Auth0:Audience"]);
      return Task.CompletedTask;
    }
  };
})
.AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.RequireHttpsMetadata = true; // Ensure HTTPS is used
    //options.SaveToken = true; // Save token for authentication
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}",
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Auth0:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("https://localhost:7181/roles", "Admin"));
    options.AddPolicy("Reader", policy => policy.RequireClaim("https://localhost:7181/roles", "Reader"));
    options.AddPolicy("AdminOrReader", policy => policy.RequireClaim("https://localhost:7181/roles", "Admin", "Reader"));
});

builder.Services.AddHttpClient<SpaceXApiService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MarsIncidentReportsDB"));
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed exception page in development
    app.UseSwagger();
}
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.OAuthClientId(builder.Configuration["Auth0:ClientId"]);
    c.OAuthClientSecret(builder.Configuration["Auth0:ClientSecret"]);
    c.OAuthUsePkce();
    c.OAuthScopes("openid profile email");
    c.OAuthAppName("My API");
    c.OAuth2RedirectUrl("https://localhost:7181/swagger/oauth2-redirect.html");
        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
    {
        { "audience", builder.Configuration["Auth0:Audience"] }
    });
});


// Enable static files and routing
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); // Enable authorization middleware

// Configure endpoint routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
