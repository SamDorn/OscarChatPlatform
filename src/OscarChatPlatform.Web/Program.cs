using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using OscarChatPlatform.Application;
using OscarChatPlatform.Application.Services;
using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;
using OscarChatPlatform.Infrastructure;
using OscarChatPlatform.Infrastructure.Repositories;
using OscarChatPlatform.Infrastructure.WebSocket;

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


// Add database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
    );

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<INotificationService, SignalRNotificationService>();

builder.Services.AddSession();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapHub<ChatHub>("/chatHub");

app.UseRequestLocalization();

app.Run();
