using EduPlatform.Core.Abstractions;
using EduPlatform.Persistence;
using EduPlatform.Application;
using Microsoft.EntityFrameworkCore;
using EduPlatform.Application.Services;
using EduPlatform.Persistence.Repositories;
using EduPlatform.Infrastructure;
using EduPlatform.API.Extensions;
using EduPlatform.API.Endpoints;
using Microsoft.Extensions.Options;
using EduPlatform.Core.Enums;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "corsPolicy";

var corsOptions = builder.Configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();

builder.Services.AddCors(options => {
    options.AddPolicy(name: corsPolicy,
        policy => {
            if (corsOptions!.AllowAnyOrigin) {
                policy.AllowAnyOrigin();
            }
            else {
                policy.WithOrigins(corsOptions.AllowedOrigins);
            }
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));

builder.Services.RequirePermissions("Create", Permission.Delete);

builder.Services.RequirePermissions("Read", Permission.Read);

builder.Services.RequirePermissions("Update", Permission.Delete);

builder.Services.RequirePermissions("Delete", Permission.Delete);

builder.Services.AddApiAuthentication(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddDbContext<EduPlatformDbContext>(
    options => {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(EduPlatformDbContext)));
    });

builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<UsersService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicy);

app.UseCookiePolicy(new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

/*app.UseCors(x => {
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000/");
    x.WithMethods().AllowAnyMethod();
});*/

app.UseAuthentication();

app.UseAuthorization();

//добавление эндпоинтов
app.AddMappedEndpoints();

app.MapControllers();

app.Run();
