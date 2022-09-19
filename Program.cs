using Microsoft.EntityFrameworkCore;

using System.Text;
using dotnet_2.Infrastructure.Data.Models;
using dotnet_2.Infrastructure.Data.Services;
using dotnet_2.Infrastructure.Dto;
using dotnet_2.Infrastructure.Shared;
using dotnet_2.Infrastructure.Data;
using dotnet_2.Infrastructure;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

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
builder.Services.AddAuthentication();

builder.Services.AddHttpContextAccessor();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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

//Edit User Data
app.MapPut("/edit/users", async (User editUser, AppDbContext db, HttpContext context) =>
{
    var tokenData = new Jwt().GetTokenClaim(context);
    var user = await db.Users.FindAsync(int.Parse(tokenData.id!));

    if (user is null) return Results.NotFound();
    user.nik = editUser.nik;
    user.password = editUser.password;
    user.name = editUser.name;
    user.role = editUser.role;
    user.grade = editUser.grade;
    user.employment_status = editUser.employment_status;
    user.phone = editUser.phone;
    user.email = editUser.email;
    user.ktp = editUser.ktp;
    user.npwp = editUser.npwp;
    user.join_date = editUser.join_date;
    await db.SaveChangesAsync();
    return Results.Ok("Edit Data Successfully");
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

// Get User Data
app.MapGet("/profile", [Authorize] async (AppDbContext db, HttpContext context) => 
{
    var tokenData = new Jwt().GetTokenClaim(context);
    var user = await db.Users.FindAsync(int.Parse(tokenData.id!));

    if (user is null) return Results.NotFound("Data Was Not Found");
    var userdto = new UserDTO(user);
    return Results.Ok(userdto);
});

// Create Organization Data
app.MapPost("/post/organization", [Authorize] async (Organization request, AppDbContext db, HttpContext context) =>
{
    var result = await db.Organizations.Where(item => item.id == request.id).FirstOrDefaultAsync();

    if (result != null)
    {
        return Results.BadRequest(new RegisterResponse{
        succes = false,
        message= "organization ID already exist",
        origin = null
    });
    }
    db.Organizations.Add(request);
    await db.SaveChangesAsync();

    return Results.Ok(new RegisterResponse
    {
        succes = true,
        message= $"Organization data Successfully Added",
        origin = null
    });
});

// Post work shedule data
app.MapPost("/post/work_shedule", [Authorize] async (HttpRequest request, AppDbContext db, HttpContext context) =>
{
    int.TryParse(request.Form["id"], out int id);
    string? work_shedule = request.Form["work_shedule"];
    TimeOnly.TryParse(request.Form["start_time"], out TimeOnly start_time);
    TimeOnly.TryParse(request.Form["end_time"], out TimeOnly end_time);
    TimeOnly.TryParse(request.Form["start_break_time1"], out TimeOnly start_break_time1);
    TimeOnly.TryParse(request.Form["end_break_time1"], out TimeOnly end_break_time1);
    TimeOnly.TryParse(request.Form["start_break_time2"], out TimeOnly start_break_time2);
    TimeOnly.TryParse(request.Form["end_break_time2"], out TimeOnly end_break_time2);

    var result = await db.WorkSchedules.Where(item => item.id == id).FirstOrDefaultAsync();

    if (result != null)
    {
        return Results.BadRequest(new RegisterResponse{
        succes = false,
        message= "WorkShedule ID already exist",
        origin = null
    });
    }
    WorkSchedule workSchedule = new WorkSchedule();
    workSchedule.work_shedule = work_shedule;
    workSchedule.start_time = start_time;
    workSchedule.end_time = end_time;
    workSchedule.start_break_time1 = start_break_time1;
    workSchedule.end_break_time1 = end_break_time1;
    workSchedule.start_break_time2 = start_break_time2;
    workSchedule.end_break_time2 = end_break_time2;

    db.WorkSchedules.Add(workSchedule);
    await db.SaveChangesAsync();

    return Results.Ok(new RegisterResponse
    {
        succes = true,
        message= $"WorkShedule data Successfully Added",
        origin = null
    });
});

// post Overtime data
app.MapPost("/overtime", [Authorize] async (HttpRequest request, AppDbContext db, HttpContext context) =>
{
    DateOnly.TryParse(request.Form["start_date"], out DateOnly start_date);
    DateOnly.TryParse(request.Form["end_date"], out DateOnly end_date);
    TimeOnly.TryParse(request.Form["start_time"], out TimeOnly start_time);
    TimeOnly.TryParse(request.Form["end_time"], out TimeOnly end_time);
    int.TryParse(request.Form["status"], out int status);
    int.TryParse(request.Form["is_completed"], out int is_completed);
    string? remarks = request.Form["remarks"];
    var attachment = request.Form.Files["attachment"];

    var result = await db.Users.Where(item => item.grade == "VIA").FirstOrDefaultAsync();

    if (result == null)
    {
        return Results.NotFound(new RegisterResponse
        {
            succes = false,
            message= "Employee with your grade is not allowed request Overtime",
            origin = null
        });
    }
   
    var overtime_data = await db.Overtime.Where(item => item.start_date == start_date).FirstOrDefaultAsync();
    
    if (overtime_data != null)
        {
            DateOnly overtime_date = overtime_data!.start_date;
            return Results.BadRequest(new RegisterResponse
            {
                succes = false,
                message= $"You already have overtime request on {overtime_date}",
                origin = null
            });
        }
    
    Account account = new Account(
        "personacloud",
        "928111718482376",
        "jSta9msnS2hrHI-2SYyl7D1wPXA");

    Cloudinary cloudinary = new Cloudinary(account);
    cloudinary.Api.Secure = true;

    var uploadFile = context.Request.Form.Files["attachment"];
    var file = new FileInfo(context.Request.Form.Files["attachment"].FileName);
    string filename = file.ToString().Substring(0, file.ToString().Length - file.Extension.ToString().Length);

    using (var filestream = uploadFile!.OpenReadStream())
    {
        var uploadUpload = new ImageUploadParams
        {
            File = new FileDescription(uploadFile.FileName, filestream),
            PublicId = $"{DateTime.Now.ToString("yyyy-MM-ddThh-mm-ss")}_{filename}"
        };

        var uploadResult = await cloudinary.UploadAsync(uploadUpload);

    Overtime overtime = new Overtime();
    switch (status)
            {
            case 1:
                overtime.status_text = "Request Approval";
                overtime.request_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.request_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            case 2:
                overtime.status_text = "Approved";
                overtime.approved_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.approved_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            case 3:
                overtime.status_text = "Rejected";
                overtime.is_completed = 1;
                overtime.completed_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.completed_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            case 4:
                overtime.status_text = "Settlement Approval";
                overtime.start_date = start_date;
                overtime.start_time = start_time;
                overtime.end_date = end_date;
                overtime.end_time = end_time;
                overtime.remarks = remarks;
                overtime.attachment = uploadResult.Url.ToString();
                break;
            case 5:
                overtime.status_text = "Revise";
                break;
            case 6:
                overtime.status_text = "Completed";
                overtime.is_completed = 1;
                overtime.completed_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.completed_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            case 9:
                overtime.status_text = "Cancelled";
                overtime.is_completed = 1;
                overtime.completed_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.completed_time = TimeOnly.FromDateTime(DateTime.Now);
                break;  
    }

    var tokenData = new Jwt().GetTokenClaim(context);
    var user = await db.Users.FindAsync(int.Parse(tokenData.id!));

        overtime.start_date = start_date;
        overtime.user = user;
        overtime.end_date = end_date;
        overtime.start_time = start_time;
        overtime.end_time = end_time;

        var ot_range = Enumerable.Range(0, int.MaxValue)
          .Select(multiplier => start_time.Add(TimeSpan.FromMinutes(30 * multiplier)))
          .TakeWhile(span => span <= end_time);

        var breaktime = await db.WorkSchedules.FirstOrDefaultAsync();

        int thisDuration1 = (int) (end_time - start_time).Hours;

        foreach(TimeOnly i in ot_range)
        {
            if ( i >= breaktime!.start_break_time1 && i <= breaktime.end_break_time1) thisDuration1--;
            else if ( i >= breaktime!.start_break_time2 && i <= breaktime.end_break_time2) thisDuration1--;
        };
        
        overtime.duration = thisDuration1;

        overtime.status = status;
        overtime.is_completed = is_completed;
        overtime.remarks = remarks;
        overtime.attachment = uploadResult.Url.ToString();
        overtime.request_date = DateOnly.FromDateTime(DateTime.Now);
        overtime.request_time = TimeOnly.FromDateTime(DateTime.Now);

    db.Overtime.Add(overtime);
    await db.SaveChangesAsync();

    return Results.Ok(new RegisterResponse
    {
        succes = true,
        message= "Overtime Data Successfully Added",
        origin = null
    });
}});

// update Overtime data
app.MapPost("/update/overtime/{id}", [Authorize] async (int id, HttpRequest request, AppDbContext db, HttpContext context) =>
{
    DateOnly.TryParse(request.Form["start_date"], out DateOnly start_date);
    DateOnly.TryParse(request.Form["end_date"], out DateOnly end_date);
    TimeOnly.TryParse(request.Form["start_time"], out TimeOnly start_time);
    TimeOnly.TryParse(request.Form["end_time"], out TimeOnly end_time);
    int.TryParse(request.Form["status"], out int status);
    string? remarks = request.Form["remarks"];
    var attachment = request.Form.Files["attachment"];

    var overtime = await db.Overtime.FindAsync(id);

    if (overtime is null)
    {
        return Results.NotFound(new RegisterResponse
        {
            succes = false,
            message= "The Overtime ID is not exist",
            origin = null
        });
    }

        overtime.start_date = start_date;
        overtime.end_date = end_date;
        overtime.start_time = start_time;
        overtime.end_time = end_time;
        int thisDuration = (int) (end_time - start_time).Hours;
        overtime.duration = thisDuration;
        overtime.status = status;

        switch (status)
            {
            case 1:
                overtime.status_text = "Request Approval";
                overtime.request_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.request_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            case 2:
                overtime.status_text = "Approved";
                overtime.approved_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.approved_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            case 3:
                overtime.status_text = "Rejected";
                overtime.is_completed = 1;
                overtime.completed_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.completed_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            case 4:
                overtime.status_text = "Settlement Approval";
                overtime.start_date = start_date;
                overtime.start_time = start_time;
                overtime.end_date = end_date;
                overtime.end_time = end_time;
                overtime.remarks = remarks;
                // result.attachment = uploadResult.Url.ToString();
                break;
            case 5:
                overtime.status_text = "Revise";
                break;
            case 6:
                overtime.status_text = "Completed";
                overtime.is_completed = 1;
                overtime.completed_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.completed_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            case 9:
                overtime.status_text = "Cancelled";
                overtime.is_completed = 1;
                overtime.completed_date = DateOnly.FromDateTime(DateTime.Now);
                overtime.completed_time = TimeOnly.FromDateTime(DateTime.Now);
                break;
            }
        overtime.remarks = remarks;
        // // result.attachment = uploadResult.Url.ToString();

        await db.SaveChangesAsync();

        return Results.Ok(new RegisterResponse
        {
            succes = true,
            message= "Your overtime data has successfully updated",
            origin = null
        });
    }
);

app.UseAuthentication();
app.UseAuthorization();
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