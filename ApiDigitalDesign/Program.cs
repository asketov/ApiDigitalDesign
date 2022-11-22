using System.Reflection;
using ApiDigitalDesign;
using ApiDigitalDesign.AutoMapper;
using ApiDigitalDesign.AutoMapper.MapperProfiles;
using ApiDigitalDesign.Middlewares.AccountValidator;
using ApiDigitalDesign.Middlewares.TokenValidator;
using ApiDigitalDesign.Services;
using DAL;
using Microsoft.EntityFrameworkCore;
using Common.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;


var builder = WebApplication.CreateBuilder(args);
var authSection = builder.Configuration.GetSection(AuthConfig.Position);
var authConfig = authSection.Get<AuthConfig>();
builder.Services.Configure<AuthConfig>(authSection);
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidIssuer = authConfig.Issuer,
        ValidateAudience = false,
        ValidAudience = authConfig.Audience,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = authConfig.SymmetricSecurityKey(),
        ClockSkew = TimeSpan.Zero
    };
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly, 
    typeof(PostProfile).Assembly,typeof(AttachProfile).Assembly,
        typeof(AuthProfile).Assembly, typeof(SubscribeProfile).Assembly, typeof(LikeProfile).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("ValidAccessToken", p =>
    {
        p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        p.RequireAuthenticatedUser();
    });
});

var app = builder.Build();
using (var serviceScope = ((IApplicationBuilder) app)
       .ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
{
    if (serviceScope != null)
    {
        var context = serviceScope.ServiceProvider
            .GetRequiredService<DataContext>();
        context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("Api/swagger.json", "Api");
        c.SwaggerEndpoint("Auth/swagger.json", "Auth");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseTokenValidator();
app.UseAccountValidator();
app.MapControllers();
app.Run();
