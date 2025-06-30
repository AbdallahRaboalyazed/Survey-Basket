using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Health;

namespace SurveyBasket;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddControllers();

        // عشان اشيل التحذير واستخدمها
        #pragma warning disable EXTEXP0018
        services.AddHybridCache();
         #pragma warning restore EXTEXP0018

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowAnyOrigin();
                    //.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!); // عشان تجيب ال AllowedOrigins من ال appsettings.json;
                });
        });


        services.AddAuthSwagger(); // عشان تضيف ال Auth ل swagger

        services.AddDataBase(configuration);

        services.AddOpenApi();

        services.AddAuthConfig(configuration);

        services.AddScoped<IQuestionService, QuestionService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IPollService, PollService>();

        services.AddScoped<IVoteService, VoteService>();

        services.AddScoped<IResultService,ResultService>();

        services.AddScoped<IEmailSender,EmailService>();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<IRoleService, RoleService>();

        services.AddFluentValidationConfig();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();

        services.AddMapsterConfig();

        services.AddBackgroundJobsConfig(configuration);

        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));


        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<AppDBcontext>("Database")
            .AddHangfire(options => { options.MinimumAvailableServers = 1; } )
            .AddCheck<MailProviderHealthCheck>("email service");

        return services;
    }

    public static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {

        services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<VoteRequestValidator>();


        return services;
    }

    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionstring = configuration.GetConnectionString("constr") ??
           throw new InvalidOperationException("Connection String 'constr' not found.");

        services.AddDbContext<AppDBcontext>(options =>
        options.UseSqlServer(connectionstring)
        );


        return services;
    }

    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
    public static IServiceCollection AddAuthConfig(this IServiceCollection services , IConfiguration configuration)
    {

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppDBcontext>()
            .AddDefaultTokenProviders();



        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();


        services.AddSingleton<IJwtProvider, JwtProvider>();



        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();


        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

       

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(o =>
       {
           o.SaveToken = true;
           o.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.Key!)),
               ValidIssuer = jwtOptions?.Issuer!,
               ValidAudience = jwtOptions?.Audience!
           };
       });

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        });

        return services;
    }




    public static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Hangfire services.
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }











    // ده الكود عشان تضيف ال Auth ل swagger
    public static IServiceCollection AddAuthSwagger(this IServiceCollection services)
    {

        // instal this Install-Package Swashbuckle.AspNetCore

        // using Microsoft.OpenApi.Models;

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SurveyBasket API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        return services;
    }
    




}
