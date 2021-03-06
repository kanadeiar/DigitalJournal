var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services =>
{
    var databaseName = builder.Configuration["Database"];
    switch (databaseName)
    {
        case "SQLite":
            services.AddDbContext<IdentityContext>(options => options
                .UseSqlite(builder.Configuration.GetConnectionString("SQLiteIdentityConnection"), o => o.MigrationsAssembly("DigitalJournal.Dal")));
            services.AddDbContext<DigitalJournalContext>(options => options
                .UseSqlite(builder.Configuration.GetConnectionString("SQLiteDigitalJournalConnection"), o => o.MigrationsAssembly("DigitalJournal.Dal")));
            break;
        case "MSSQL":
            services.AddDbContext<IdentityContext>(options => options
                .UseSqlServer(builder.Configuration.GetConnectionString("MSSQLIdentityConnection"), o => o.MigrationsAssembly("DigitalJournal.Dal.Mssql")));
            services.AddDbContext<DigitalJournalContext>(options => options
                .UseSqlServer(builder.Configuration.GetConnectionString("MSSQLDigitalJournalConnection"), o => o.MigrationsAssembly("DigitalJournal.Dal.Mssql")));
            break;
    }

    services.AddIdentity<User, Role>()
        .AddEntityFrameworkStores<IdentityContext>();
    services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequiredLength = 3;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    });
    services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie(options => {
        options.Events.DisableRedirectForPath(e => e.OnRedirectToLogin,
             "/api", StatusCodes.Status401Unauthorized);
        options.Events.DisableRedirectForPath(e => e.OnRedirectToAccessDenied,
            "/api", StatusCodes.Status403Forbidden);
    }).AddJwtBearer(options => {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["jwtSecret"])),
            ValidateAudience = false,
            ValidateIssuer = false
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async ctx => {
                var usrmgr = ctx.HttpContext.RequestServices
                    .GetRequiredService<UserManager<User>>();
                var signinmgr = ctx.HttpContext.RequestServices
                    .GetRequiredService<SignInManager<User>>();
                string? username =
                    ctx.Principal?.FindFirst(ClaimTypes.Name)?.Value;
                var idUser = await usrmgr.FindByNameAsync(username);
                ctx.Principal =
                    await signinmgr.CreateUserPrincipalAsync(idUser);
            }
        };
    });

    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddRazorPages().AddRazorRuntimeCompilation();

    services.AddBlazoredToast();

    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IHomeInfoService, HomeInfoService>();

    services.AddTransient<IIdentitySeedTestData, IdentitySeedTestData>();
    services.AddTransient<IDigitalJournalSeedTestData, DigitalJournalSeedTestData>();
});

builder.Services.AddServerSideBlazor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider
        .GetRequiredService<IIdentitySeedTestData>()
        .SeedTestData(app.Services, builder.Configuration);
    var seederJournal = scope.ServiceProvider
        .GetRequiredService<IDigitalJournalSeedTestData>()
        .SeedTestData(app.Services, builder.Configuration);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithRedirects("~/home/error/{0}");

app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("online/{param?}", "/Shared/_Host");

app.Run();

public partial class Program { }