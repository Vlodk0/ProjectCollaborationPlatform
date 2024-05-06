using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.BL.Services;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.WebAPI.Helpers.ErrorFilter;
using Serilog;
using System.Security.Claims;
using System.Text;

namespace ProjectCollaborationPlatform.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });

            builder.Services.AddControllers();
            builder.Services.AddTransient<DBSeeder>();
            builder.Services.AddScoped<IDeveloperService, DeveloperService>();
            builder.Services.AddScoped<IProjectOwnerService, ProjectOwnerService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IBoardService, BoardService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IFunctionalityBlockService, FunctionalityBlockService>();
            builder.Services.AddScoped<ITechnologyService, TechnologyService>();
            builder.Services.AddTransient<IPhotoManageService, PhotoManageService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ProjectPlatformContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("db"));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigins", policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],//better to use bound models for configs instead of direct IConfiguration calls

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:SecretKey"])),
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ProjectOwnerRole",
                    policy => policy.RequireClaim(ClaimTypes.Role, "ProjectOwner"));
                options.AddPolicy("DeveloperRole",
                    policy => policy.RequireClaim(ClaimTypes.Role, "Dev"));
                options.AddPolicy("AdminRole",
                    policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("AdminProjectOwnerRole",
                    policy => policy.RequireClaim(ClaimTypes.Role, "ProjectOwner", "Admin"));
                options.AddPolicy("DevProjectOwnerRole",
                    policy => policy.RequireClaim(ClaimTypes.Role, "ProjectOwner", "Dev"));
            });

            builder.Host.UseSerilog((context, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration));

            var app = builder.Build();

            if (args.Length == 1 && args[0].ToLower() == "seeddata")
                SeedData(app);

            void SeedData(IHost app)
            {
                var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                using (var scope = scopedFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<DBSeeder>();
                    service.SeedTechnologies();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors("AllowMyOrigins");

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
