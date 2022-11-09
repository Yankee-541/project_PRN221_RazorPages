using WebRazor.Models;
using WebRazor.Hubs;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// add services
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSession(otp => otp.IdleTimeout = TimeSpan.FromMinutes(5));
builder.Services.AddDbContext<PRN221DBContext>();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//builder.Services.AddSession(otp => otp.IdleTimeout = TimeSpan.FromMilliseconds(30));
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidAudience = builder.Configuration["JWT:ValidAudience"],
//            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
//        };
//    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseRouting();
app.UseStaticFiles();
app.UseSession();
app.MapRazorPages();
app.MapHub<HubServer>("/hub");
//app.Use(async (context, next) =>
//{
//    var token = context.Session.GetString("Token");
//    if (!string.IsNullOrEmpty(token))
//    {
//        context.Request.Headers.Add("Authorization", "Bearer " + token);
//    }
//    await next();
//});
app.Run();