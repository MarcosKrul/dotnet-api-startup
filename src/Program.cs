using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TucaAPI.src.Attributes;
using TucaAPI.src.Common;
using TucaAPI.src.Data;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Middlewares;
using TucaAPI.src.Models;
using TucaAPI.src.Provider;
using TucaAPI.src.Providers;
using TucaAPI.src.Repositories;
using TucaAPI.src.Services.Account;
using TucaAPI.src.Services.Comment;
using TucaAPI.src.Services.Portfolio;
using TucaAPI.src.Services.Stock;
using TucaAPI.src.Services.UserManagement;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new ValidationFailedResult(context.ModelState);
            result.ContentTypes.Add(MediaTypeNames.Application.Json);
            return result;
        };
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = MessageKey.PLEASE_ENTER_VALID_TOKEN,
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        }
    );
    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
                new string[] { }
            }
        }
    );
});

builder
    .Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
            .Json
            .ReferenceLoopHandling
            .Ignore;
    });

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(EnvVariables.DB_CONNECTION_STRING)
    );
});

builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection(EnvVariables.MAIL_SETTINGS)
);

builder
    .Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = Constants.MIN_PASSWORD_LENGTH;
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = Constants.MAX_LOGIN_ATTEMPTS;
        options.Lockout.DefaultLockoutTimeSpan = Constants.RESET_LOGIN_ATTEMPTS;
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = Constants.TOKEN_EXPIRES_IN;
});

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
            options.DefaultScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme =
                JwtBearerDefaults.AuthenticationScheme;
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
                System.Text.Encoding.UTF8.GetBytes(
                    builder.Configuration[EnvVariables.JWT_SIGNIN_KEY]
                        ?? Constants.DEFAULT_JWT_SECRET
                )
            )
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        AuthorizationPolicies.ADMIN_ONLY,
        policy => policy.RequireClaim(ClaimTypes.Role, PermissionRoles.ADM)
    );
    options.AddPolicy(
        AuthorizationPolicies.USER_ONLY,
        policy => policy.RequireClaim(ClaimTypes.Role, PermissionRoles.USER)
    );
});

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IMailSenderProvider, MailSenderProvider>();
builder.Services.AddScoped<ITemplateRenderingProvider, TemplateRenderingProvider>();
builder.Services.AddScoped<RegisterAccountService>();
builder.Services.AddScoped<ConfirmAccountService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ForgotPasswordService>();
builder.Services.AddScoped<ResetPasswordService>();
builder.Services.AddScoped<DeleteUserService>();
builder.Services.AddScoped<GetStocksFromUserPortfolioService>();
builder.Services.AddScoped<DeleteUserPortfolioService>();
builder.Services.AddScoped<AddStockAtUserPortfolioService>();
builder.Services.AddScoped<CreateStockService>();
builder.Services.AddScoped<DeleteStockService>();
builder.Services.AddScoped<UpdateStockService>();
builder.Services.AddScoped<GetStockByIdService>();
builder.Services.AddScoped<GetAllStockService>();
builder.Services.AddScoped<CreateCommentService>();
builder.Services.AddScoped<DeleteCommentService>();
builder.Services.AddScoped<GetAllCommentService>();
builder.Services.AddScoped<GetCommentByIdService>();
builder.Services.AddScoped<UpdateCommentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
