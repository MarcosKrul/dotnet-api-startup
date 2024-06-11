using Microsoft.EntityFrameworkCore;
using TucaAPI.Data;
using TucaAPI.Interfaces;
using TucaAPI.Attributes;
using TucaAPI.Repositories;
using TucaAPI.Models;
using Microsoft.AspNetCore.Identity;
using TucaAPI.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TucaAPI.Service;
using Microsoft.OpenApi.Models;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Interfaces;
using TucaAPI.src.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateAttribute>();
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = Messages.PLEASE_ENTER_VALID_TOKEN,
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

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(EnvVariables.DB_CONNECTION_STRING));
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(EnvVariables.MAIL_SETTINGS));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = Constants.MIN_PASSWORD_LENGTH;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = Constants.MAX_LOGIN_ATTEMPTS;
    options.Lockout.DefaultLockoutTimeSpan = Constants.RESET_LOGIN_ATTEMPTS;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = Constants.FORGOT_PASSWORD_TOKEN_EXPIRES_IN;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration[EnvVariables.JWT_ISSUER],
        ValidateAudience = true,
        ValidAudience = builder.Configuration[EnvVariables.JWT_AUDIENCE],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration[EnvVariables.JWT_SIGNIN_KEY] ?? Constants.DEFAULT_JWT_SECRET)
        )
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.ADMIN_ONLY, policy => policy.RequireClaim(PermissionClaims.ADM));
    options.AddPolicy(AuthorizationPolicies.USER_ONLY, policy => policy.RequireClaim(PermissionClaims.USER));
});

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IMailSenderService, MailSenderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
