using MarsIncidentReporter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add security definition for Bearer tokens
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and the JWT token."
    });

    // Add security requirement to Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure Authentication with Auth0
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect("Auth0", options =>
{
  options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
  options.ClientId = builder.Configuration["Auth0:ClientId"];
  options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
  options.ResponseType = "code";

  // Configure the callback path for Auth0
  options.CallbackPath = new PathString("/Account/Callback");
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
      context.ProtocolMessage.SetParameter("audience", "https://dev-mfay1fcfb1wvdp3y.us.auth0.com/api/v2/");
      return Task.CompletedTask;
    }
  };
});

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
  options.AddPolicy("Reader", policy => policy.RequireRole("Reader"));
});

builder.Services.AddHttpClient<SpaceXApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Redirect to the login page by default
app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/Account/Login");
    return Task.CompletedTask;
});

app.MapControllers();

app.Run();
