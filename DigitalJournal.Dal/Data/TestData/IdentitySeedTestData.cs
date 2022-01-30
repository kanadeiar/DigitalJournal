namespace DigitalJournal.Dal.Data;

public static class IdentitySeedTestData
{
    /// <summary> Заполнение идентификационной базы данных тестовыми данными </summary>
    /// <param name="provider">Провайдер</param>
    /// <param name="configuration">Корфигурация</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task SeedTestData(IServiceProvider provider, IConfiguration configuration)
    {
        provider = provider.CreateScope().ServiceProvider;
        var logger = provider.GetRequiredService<ILogger<IdentityContext>>();
        using var context = new IdentityContext(provider.GetRequiredService<DbContextOptions<IdentityContext>>());

        if (context == null || context.Users == null)
        {
            logger.LogError("Null IdentityContext");
            throw new ArgumentNullException("Null IdentityContext");
        }
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation($"Applying migrations: {string.Join(",", pendingMigrations)}");
            await context.Database.MigrateAsync();
        }
        if (context.Users.Any())
        {
            logger.LogInformation("Identity database contains data - database init with test data is not required");
            return;
        }
        logger.LogInformation("Begin writing test data to database IdentityContext ...");

        #region Identity

        UserManager<User> userManager = provider.GetRequiredService<UserManager<User>>();
        RoleManager<Role> roleManager = provider.GetRequiredService<RoleManager<Role>>();

        if (await roleManager.FindByNameAsync(TestData.AdminRole.Name) is null)
        {
            await roleManager.CreateAsync(new Role { Name = TestData.AdminRole.Name, Description = TestData.AdminRole.Description });
        }
        if (await roleManager.FindByNameAsync(TestData.UserRole.Name) is null)
        {
            await roleManager.CreateAsync(new Role { Name = TestData.UserRole.Name, Description = TestData.UserRole.Description });
        }
        if (await roleManager.FindByNameAsync(TestData.MasterRole.Name) is null)
        {
            await roleManager.CreateAsync(new Role { Name = TestData.MasterRole.Name, Description = TestData.MasterRole.Description });
        }
        if (await roleManager.FindByNameAsync(TestData.OperatorRole.Name) is null)
        {
            await roleManager.CreateAsync(new Role { Name = TestData.OperatorRole.Name, Description = TestData.OperatorRole.Description });
        }
        if (await userManager.FindByNameAsync(TestData.Admin.Username) is null)
        {
            var adminUser = new User
            {
                UserName = TestData.Admin.Username,
                Email = TestData.Admin.Email,
            };
            var result = await userManager.CreateAsync(adminUser, TestData.Admin.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, TestData.Admin.Rolename);
                await userManager.AddToRoleAsync(adminUser, TestData.User.Rolename);
                await userManager.AddToRoleAsync(adminUser, TestData.Master.Rolename);
                await userManager.AddToRoleAsync(adminUser, TestData.Operator.Rolename);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                logger.LogError("Учётная запись пользователя {0} не создана по причине: {1}", adminUser.UserName, string.Join(",", errors));
                throw new InvalidOperationException($"Ошибка при создании пользователя {adminUser.UserName}, список ошибок: {string.Join(",", errors)}");
            }
        }
        if (await userManager.FindByNameAsync(TestData.User.Username) is null)
        {
            var user = new User
            {
                UserName = TestData.User.Username,
                Email = TestData.User.Email,
            };
            var result = await userManager.CreateAsync(user, TestData.User.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, TestData.User.Rolename);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                logger.LogError("Учётная запись пользователя {0} не создана по причине: {1}", user.UserName, string.Join(",", errors));
                throw new InvalidOperationException($"Ошибка при создании пользователя {user.UserName}, список ошибок: {string.Join(",", errors)}");
            }
        }
        if (await userManager.FindByNameAsync(TestData.Master.Username) is null)
        {
            var user = new User
            {
                UserName = TestData.Master.Username,
                Email = TestData.Master.Email,
            };
            var result = await userManager.CreateAsync(user, TestData.Master.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, TestData.Master.Rolename);
                await userManager.AddToRoleAsync(user, TestData.Operator.Rolename);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                logger.LogError("Учётная запись пользователя {0} не создана по причине: {1}", user.UserName, string.Join(",", errors));
                throw new InvalidOperationException($"Ошибка при создании пользователя {user.UserName}, список ошибок: {string.Join(",", errors)}");
            }
        }
        if (await userManager.FindByNameAsync(TestData.Operator.Username) is null)
        {
            var user = new User
            {
                UserName = TestData.Operator.Username,
                Email = TestData.Operator.Email,
            };
            var result = await userManager.CreateAsync(user, TestData.Operator.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, TestData.Operator.Rolename);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                logger.LogError("Учётная запись пользователя {0} не создана по причине: {1}", user.UserName, string.Join(",", errors));
                throw new InvalidOperationException($"Ошибка при создании пользователя {user.UserName}, список ошибок: {string.Join(",", errors)}");
            }
        }

        #endregion

        logger.LogInformation("Complete writing test data to database IdentityContext ...");
    }
}

