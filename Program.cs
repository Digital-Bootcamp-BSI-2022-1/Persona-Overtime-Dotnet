using Microsoft.EntityFrameworkCore;

using System.Text;
using dotnet_2.Infrastructure.Data.Models;
using dotnet_2.Infrastructure.Data.Services;
using dotnet_2.Infrastructure.Dto;
using dotnet_2.Infrastructure.Shared;
using dotnet_2.Infrastructure.Data;
using dotnet_2.Infrastructure;

using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
var builder = WebApplication.CreateBuilder(args);
var securityScheme = new OpenApiSecurityScheme() 

{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security",
};

var securityReq = new OpenApiSecurityRequirement()
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
        new string[] {}
    }
};


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(securityReq);
});
 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = false,
        ValidateAudience = false,
        RequireExpirationTime = true
    };

});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// database config
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("Mail"));
builder.Services.Configure<WaSettings>(builder.Configuration.GetSection("Wa"));
builder.Services.AddSingleton<IMailService, MailService>();
builder.Services.AddSingleton<IWaService, WaService>();
builder.Services.AddSingleton<IMailService, MailService>();
builder.Services.AddSingleton<IWaService, WaService>();
builder.Services.AddSingleton<IMailServicePassword, MailServicePassword>();
builder.Services.AddSingleton<IWaServicePassword, WaServicePasswords>();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/register", async (User request, AppDbContext db) =>
{
    var result = await db.Users.Where(item => item.nik == request.nik).FirstOrDefaultAsync();

    if (result != null)
    {
        return Results.BadRequest(new { message = "User already exist" });
    }

    var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password).ToString();
    request.password = passwordHash;

    db.Users.Add(request);
    await db.SaveChangesAsync();

    return Results.Ok(new { message = "Registration succes!" });

});


// Login
app.MapPost("/auth/login", async (LoginRequest request, AppDbContext db) =>
{
    // verify username user
    var result = await db.Users.Where(item => item.nik == request.nik).FirstOrDefaultAsync();

    if (result == null)
    {
        return Results.BadRequest(new { message = "Username incorrect" });
    }

    bool verifyPassword = BCrypt.Net.BCrypt.Verify(request.Password, result.password);

    // verify password
    if (!verifyPassword)
    {
        return Results.BadRequest(new { message = "Password Incorrect" });
    }

    var token = new Jwt().GenerateJwtToken(result);
    var expiredAt = (Int32)DateTime.UtcNow.AddDays(3).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

    // insert token to auth token
    var authToken = new AuthTokenn
    {
        user_id = result.id,
        role = result.role,
        expired_at = expiredAt,
        token = token,
    };

    db.AuthTokenns.Add(authToken);
    await db.SaveChangesAsync();


    return Results.Ok(new LoginResponse
    {
        Name = result.name,
        Email = result.email,
        ExpiredAt = expiredAt,
        Token = token,
    });
});

// Create Organization Data
app.MapPost("/post/organization", async (Organization request, AppDbContext db, HttpContext context) =>
{
    var result = await db.Organizations.Where(item => item.member == request.member).FirstOrDefaultAsync();

    if (result != null)
    {
        return Results.BadRequest(new RegisterResponse{
        succes = false,
        message= $"employee already exist",
        origin = null
    });
    }
    db.Organizations.Add(request);
    await db.SaveChangesAsync();

    return Results.Ok(new RegisterResponse
    {
        succes = true,
        message= $"Employee Successfully Added",
        origin = null
    });
});

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);

}

public interface IWaService
{
    Task SendWaAsync(MailRequest mailRequest);
}

public interface IMailServicePassword
{
    Task SendEmailPasswordAsync(MailRequestPassword mailRequestPassword);

}

public interface IWaServicePassword
{
    Task SendWaPasswordsAsync(MailRequestPassword mailRequestPassword);
}

interface IAppDbContext 
{

}
