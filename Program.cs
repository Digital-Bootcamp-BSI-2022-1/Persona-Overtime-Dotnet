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
app.MapPost("/post/organization", async (Organization request, AppDbContext db, HttpContext context) =>
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


// // post Overtime data
// app.MapPost("/overtime", [Authorize] async (int id, HttpRequest request, AppDbContext db, HttpContext context) =>
// {
//     var tokenData = new Jwt().GetTokenClaim(context);
//     var user = await db.Users.FindAsync(int.Parse(tokenData.id!));

//     int.TryParse(request.Form["id"], out id);
//     string? start_date = request.Form["start_date"];
//     string? end_date = request.Form["end_date"];
//     string? start_time = request.Form["start_time"];
//     string? end_time = request.Form["end_time"];
//     int.TryParse(request.Form["status"], out int status);
//     int.TryParse(request.Form["is_completed"], out int is_completed);
//     string? remarks = request.Form["remarks"];
//     var attachment = request.Form.Files["attachment"];

//     var result = await db.Overtime.Where(item => item.id == id).FirstOrDefaultAsync();

//     if (result != null)
//     {
//         result.start_date = start_date;
//         result.end_date = end_date;
//         result.start_time = start_time;
//         result.end_time = end_time;
//         result.status = status;


//     Account account = new Account(
//         "personacloud",
//         "928111718482376",
//         "jSta9msnS2hrHI-2SYyl7D1wPXA");

//     Cloudinary cloudinary = new Cloudinary(account);
//     cloudinary.Api.Secure = true;


//     var uploadParams = new ImageUploadParams(){
//     File = new FileDescription(@"https://upload.wikimedia.org/wikipedia/commons/a/ae/Olympic_flag.jpg"),
//     PublicId = "olympic_flag"};
//     var uploadResult = cloudinary.Upload(uploadParams);

//         // var path_extention = new FileInfo(request.Form.Files["attachment"].FileName);
//         // var filePath = Path.Combine("image", $"{DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")}_attachment_{path_extention.Extension}");
//         // using (var stream = System.IO.File.Create(filePath))
//         //     {
//         //         await request.Form.Files["attachment"].CopyToAsync(stream);
//         //     }
//         switch (status)
//             {
//             case 1:
//                 result.status_text = "Request Approval";
//                 result.request_date = DateTime.Now.ToString("MM/dd/yyyy");
//                 result.request_time = DateTime.Now.ToString("HH:mm:ss");
//                 break;
//             case 2:
//                 result.status_text = "Approved";
//                 result.approved_date = DateTime.Now.ToString("MM/dd/yyyy");
//                 result.approved_time = DateTime.Now.ToString("HH:mm:ss");
//                 break;
//             case 3:
//                 result.status_text = "Rejected";
//                 result.is_completed = 1;
//                 result.completed_date = DateTime.Now.ToString("MM/dd/yyyy");
//                 result.completed_time = DateTime.Now.ToString("HH:mm:ss");
//                 break;
//             case 4:
//                 result.status_text = "Settlement Approval";
//                 result.start_date = start_date;
//                 result.start_time = start_time;
//                 result.end_date = end_date;
//                 result.end_time = end_time;
//                 result.remarks = remarks;
//                 result.attachment = filePath;
//                 break;
//             case 5:
//                 result.status_text = "Revise";
//                 break;
//             case 6:
//                 result.status_text = "Completed";
//                 result.is_completed = 1;
//                 result.completed_date = DateTime.Now.ToString("MM/dd/yyyy");
//                 result.completed_time = DateTime.Now.ToString("HH:mm:ss");
//                 break;
//             case 9:
//                 result.status_text = "Cancelled";
//                 result.is_completed = 1;
//                 result.completed_date = DateTime.Now.ToString("MM/dd/yyyy");
//                 result.completed_time = DateTime.Now.ToString("HH:mm:ss");
//                 break;
//             }
//         result.is_completed = is_completed;
//         result.remarks = remarks;
//         result.attachment = filePath;
        
//         return Results.Ok(new RegisterResponse
//         {
//             succes = true,
//             message= "Overtime Successfully Updated",
//             origin = null
//         });
//     }
//     Overtime overtime = new Overtime();
//     switch (status)
//     {
//         case 1:
//             overtime.status_text = "Request Approval";
//             break;
//         case 2:
//             overtime.status_text = "Approved";
//             break;
//         case 3:
//             overtime.status_text = "Rejected";
//             break;
//         case 4:
//             overtime.status_text = "Settlement Approval";
//             break;
//         case 5:
//             overtime.status_text = "Revise";
//             break;
//         case 6:
//             overtime.status_text = "Completed";
//             break;

//         case 9:
//             overtime.status_text = "Cancelled";
//             break;
//     }
//         result!.start_date = start_date;
//         result.user.id = user.id;
//         result.end_date = end_date;
//         result.start_time = start_time;
//         result.end_time = end_time;
//         result.status = status;
//         result.is_completed = is_completed;
//         result.remarks = remarks;
//         var path_extention2 = new FileInfo(request.Form.Files["attachment"].FileName);
//         var filePath2 = Path.Combine("image", $"{DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")}_attachment_{path_extention2.Extension}");
//         using (var stream = System.IO.File.Create(filePath2))
//             {
//                 await request.Form.Files["attachment"].CopyToAsync(stream);
//             }
//         result.attachment = filePath2;

//         var dateNow = DateTime.Now;
//         result.request_date = DateTime.Now.ToString("MM/dd/yyyy");
//         result.request_time = DateTime.Now.ToString("HH:mm:ss");

//     db.Overtime.Add(overtime);

//     await db.SaveChangesAsync();

//     return Results.Ok(new RegisterResponse
//     {
//         succes = true,
//         message= "Overtime Data Successfully Added",
//         origin = null
//     });
// });

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
