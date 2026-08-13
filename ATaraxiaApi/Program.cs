var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.Configure<JWTconfig>(builder.Configuration.GetSection("JWT"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IAuthService, AuthService>();

StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeSettings:SecretKey");

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ChargeService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IStripeAppService, StripeAppService>();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                  builder.Configuration.GetConnectionString("DefaultConnection"),
                       b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


var jwtKey = builder.Configuration["JWT:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT:Key configuration is missing.");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ClockSkew = TimeSpan.Zero

                    };
                });

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var adminEmail = builder.Configuration["AdminSeed:Email"];
    var adminPassword = builder.Configuration["AdminSeed:Password"];

    if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();