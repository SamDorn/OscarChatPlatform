using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OscarChatPlatform.Application;
using OscarChatPlatform.Application.Services;
using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;
using OscarChatPlatform.Infrastructure;
using OscarChatPlatform.Infrastructure.Authentication;
using OscarChatPlatform.Infrastructure.Repositories;
using OscarChatPlatform.Infrastructure.WebSocket;
using OscarChatPlatform.Web.Extensions;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

// Add repositories

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatRoomQueueRepository, ChatRoomQueueRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();


// Add jwtToken
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();


// Add database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!))
    };

    options.Events = new CookieJwtBearerEvents("token");
});

builder.Services.AddAuthorization();

// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<ChatRoomService>();
builder.Services.AddScoped<ConnectionService>();
builder.Services.AddScoped<INotificationService, SignalRNotificationService>();

var app = builder.Build();


using var context = app.Services.CreateScope();
var dbContext = context.ServiceProvider.GetRequiredService<ApplicationDbContext>();
// Check and apply pending migrations
await dbContext.Database.EnsureCreatedAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 401)
    {
        // Solo per richieste non-API
        if (!context.HttpContext.Request.Path.StartsWithSegments("/api"))
        {
            context.HttpContext.Response.Redirect("/");
        }
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");

app.UseRequestLocalization();



app.Run();


