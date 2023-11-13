using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PINChat.Api.Data;
using PINChat.Api.Library;
using PINChat.Api.Library.DataAccess;
using PINChat.Api.Library.DataAccess.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional:true, reloadOnChange:true);
builder.Configuration.AddJsonFile("appsettings.Development.json", optional:true, reloadOnChange:true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCors(policy=>
{
    policy.AddPolicy("OpenCorsPolicy",opt=>opt
        .WithOrigins(
            "https://localhost:7154",
            "https://chat.anmal.dev",
            "https://pinchat.anmal.dev")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
}); 

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("PINChatAuth") ?? 
                       throw new InvalidOperationException("Connection string 'PINChatAuth' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//  Personal services

builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<IUserData, UserData>();
builder.Services.AddTransient<IGroupData, GroupData>();
builder.Services.AddTransient<IMessageData, MessageData>();
builder.Services.AddTransient<ISettingData, SettingData>();
builder.Services.AddTransient<IImageData, ImageData>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "JwtBearer";
        options.DefaultChallengeScheme = "JwtBearer";
    })
    .AddJwtBearer("JwtBearer", jwtBearerOptions =>
    {
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Secrets:SecurityKey")!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });

var app = builder.Build();

app.UseCors("OpenCorsPolicy");

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();