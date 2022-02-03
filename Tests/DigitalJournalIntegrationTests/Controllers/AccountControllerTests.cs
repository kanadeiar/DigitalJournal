namespace DigitalJournalIntegrationTests.Controllers;

[TestClass]
public class AccountControllerTests
{
    readonly Random random = new Random();

    [TestMethod]
    public void Index_CorrectRequestReplaceService_ShouldView()
    {
        var testHost = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptorIdentity = services
                        .SingleOrDefault(_ => _.ServiceType == typeof(DbContextOptions<IdentityContext>));
                    services.Remove(descriptorIdentity);
                    services.AddDbContext<IdentityContext>(options =>
                    {
                        options.UseInMemoryDatabase(random.Next(int.MaxValue).ToString());
                    });
                    var descriptorJournal = services
                        .SingleOrDefault(_ => _.ServiceType == typeof(DbContextOptions<DigitalJournalContext>));
                    services.Remove(descriptorJournal);
                    services.AddDbContext<DigitalJournalContext>(options =>
                    {
                        options.UseInMemoryDatabase(random.Next(int.MaxValue).ToString());
                    });
                    var descriptorInitializerIdentity =
                        services.SingleOrDefault(_ => _.ServiceType == typeof(IIdentitySeedTestData));
                    services.Remove(descriptorInitializerIdentity);
                    services.AddTransient(_ => Mock.Of<IIdentitySeedTestData>());
                    var descriptorInitializerJournal =
                        services.SingleOrDefault(_ => _.ServiceType == typeof(IDigitalJournalSeedTestData));
                    services.Remove(descriptorInitializerJournal);
                    services.AddTransient(_ => Mock.Of<IDigitalJournalSeedTestData>());
                });
            });
        var testClient = testHost.CreateClient();

        var response = testClient.GetAsync("Account/Index").Result;

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}

