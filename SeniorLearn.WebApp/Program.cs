using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Configuration;
using SeniorLearn.WebApp.Data.Configuration.Database;
using SeniorLearn.WebApp.Data.Configuration.Migrations;
using SeniorLearn.WebApp.Data.Identity;
using SeniorLearn.WebApp.Services.Member;
using System.Data;
using System.Globalization;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");



builder.Services.AddDbContext<ApplicationDbContext>(options => 
{
    options.UseSqlServer(connectionString, s =>
    {
        s.UseAzureSqlDefaults(true);
        s.EnableRetryOnFailure(5);
    }).ReplaceService<IMigrationsSqlGenerator, MyMigrationsSqlGenerator>();

});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ActiveRolePolicy", policy =>
//    {
//        policy.RequireRole("STANDARD", "HONORARY", "HONORARY");
//        policy.RequireClaim(UserRoleType.RoleTypes, )
//    });
//});

//Configure Cookie/JWT Authentication/Authorization policy
builder.Services
    .AddAuthentication(options => 
    {
        options.DefaultScheme = "SeniorLearnScheme";
        options.DefaultChallengeScheme = "SeniorLearnScheme";
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddPolicyScheme("SeniorLearnScheme", "SeniorLearnScheme", options =>
    {
        options.ForwardDefaultSelector = c =>
        {
            string auth = c.Request.Headers[HeaderNames.Authorization]!;
            if (!string.IsNullOrWhiteSpace(auth) && auth.StartsWith("Bearer "))
            {
                return JwtBearerDefaults.AuthenticationScheme;
            }
            return IdentityConstants.ApplicationScheme;
        };
    });


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options => 
{
    var supportedCultures = new[] { new CultureInfo("en-AU") };
    options.DefaultRequestCulture = new RequestCulture("en-AU");
    options.SupportedCultures = supportedCultures;  
    options.SupportedUICultures = supportedCultures;    
});


builder.Services.AddControllersWithViews();
    
//AutoMapper
builder.Services.AddAutoMapper(typeof(SeniorLearn.WebApp.Mapper.AuotMapperProfile));

builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserManager<User>>();

var app = builder.Build();

app.UseRequestLocalization();



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

app.UseCors(policy => 
{
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();    
});


app.UseAuthorization();

app.MapControllerRoute(
name: "areas",
pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//Seed Db
await RegisterInitialRolesAndAministrator(app);


app.Run();




async Task RegisterInitialRolesAndAministrator(WebApplication app)
{

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManger = services.GetRequiredService<RoleManager<IdentityRole>>();

    if (context.Organisations.Any())
    {
        //var organisation = new Organisation { Name = "SeniorLearn", TimetableId = 1 };
        //organisation.Timetable = new Timetable { OrganisationId = 1 };
        //context.Organisations.Add(organisation);
        //await context.SaveChangesAsync();

        var roles = new[] {
            new IdentityRole
            {
                Id = "STANDARD",
                Name = "STANDARD",
                NormalizedName = "STANDARD"
            },
            new IdentityRole
            {
                Id = "PROFESSIONAL",
                Name = "PROFESSIONAL",
                NormalizedName = "PROFESSIONAL"
            },
            new IdentityRole
            {
                Id = "HONORARY",
                Name = "HONORARY",
                NormalizedName = "HONORARY"
            },
            new IdentityRole
            {
                Id = "ADMINISTRATION",
                Name = "ADMINISTRATION",
                NormalizedName = "ADMINISTRATION"
            }
        };
        foreach (var role in roles)
        {
            if (!await roleManger.RoleExistsAsync(role.Name!))
            {
                await roleManger.CreateAsync(role);
            }
        }

        string username = "administration@seniorlearn.org.au";
        string password = "a123!@#";
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username);
        //make sure the username doesn't exist
        if (user == null)
        {
            user = new User { UserName = username, Email = username, EmailConfirmed = true };
            //Create account   
            await userManager.CreateAsync(user, password);

            //assign role to account  
            await userManager.AddToRoleAsync(user, "ADMINISTRATION");
        }
    }
}

