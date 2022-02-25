namespace DigitalJournalTests.Controllers;

[TestClass]
public class ApiProfileControllerTests
{
    readonly Random random = new Random();

    [TestMethod]
    public void GetAll_CorrectRequest_ShouldCorrectOk()
    {
        var expProfile = new Profile
        {
            SurName = "testSurName",
            UserId = "test",
        };
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        journalContext.Profiles.Add(expProfile);
        journalContext.SaveChanges();
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.GetAll().Result as OkObjectResult;

        var results = ((IEnumerable<Profile>)result.Value).ToArray();
        Assert
            .AreEqual(expProfile.SurName, results[0].SurName);
    }

    [TestMethod]
    public void Get_CorrectRequest_ShouldCorrectOk()
    {
        var expProfile = new Profile
        {
            SurName = "testSurName",
            UserId = "test",
        };
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        journalContext.Profiles.Add(expProfile);
        journalContext.SaveChanges();
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Get(expProfile.Id).Result as OkObjectResult;

        var actual = (Profile)result.Value;
        Assert
            .AreEqual(expProfile.SurName, actual.SurName);
    }

    [TestMethod]
    public void Get_NotFoundRequest_ShouldNotFound()
    {
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Get(999).Result;

        Assert
            .IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public void Add_NullRequest_ShouldException()
    {
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        Assert.ThrowsExceptionAsync<ArgumentNullException>(() => controller.Add(null));
    }

    [TestMethod]
    public void Add_CorrectRequest_ShouldCorrectResult()
    {
        var expProfile = new Profile
        {
            SurName = "testSurName",
            UserId = "test",
        };
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Add(expProfile).Result as OkObjectResult;

        Assert
            .AreEqual(expProfile.Id, result.Value);
    }

    [TestMethod]
    public void Edit_NullRequest_ShouldException()
    {
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        Assert.ThrowsExceptionAsync<ArgumentNullException>(() => controller.Edit(null));
    }

    [TestMethod]
    public void Edit_NotFoundRequest_ShouldNotFound()
    {
        var expProfile = new Profile
        {
            Id = 999,
            SurName = "testSurName",
            UserId = "test",
        };
        var baseProfile = new Profile
        {
            UserId = "old",
        };
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        journalContext.Profiles.Add(baseProfile);
        journalContext.SaveChanges();
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Edit(expProfile).Result;

        Assert
            .IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public void Edit_CorrectNoEqualRequest_ShouldCorrect()
    {
        var expProfile = new Profile
        {
            SurName = "testSurName",
            UserId = "test",
        };
        var baseProfile = new Profile
        {
            UserId = "old",
        };
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        journalContext.Profiles.Add(baseProfile);
        journalContext.SaveChanges();
        expProfile.Id = baseProfile.Id;
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Edit(expProfile).Result;

        Assert
            .IsInstanceOfType(result, typeof(OkObjectResult));
        Assert
            .AreEqual(expProfile.Id, ((OkObjectResult)result).Value);
        Assert
            .AreEqual(expProfile.SurName, baseProfile.SurName);
    }

    [TestMethod]
    public void Edit_CorrectEqualRequest_ShouldCorrect()
    {
        var expProfile = new Profile
        {
            SurName = "testSurName",
            UserId = "test",
        };
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        journalContext.Profiles.Add(expProfile);
        journalContext.SaveChanges();
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Edit(expProfile).Result;

        Assert
            .IsInstanceOfType(result, typeof(OkObjectResult));
        Assert
            .AreEqual(expProfile.Id, ((OkObjectResult)result).Value);
    }

    [TestMethod]
    public void Delete_NotFoundRequest_ShouldNotFound()
    {
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Delete(999).Result;

        Assert
            .IsInstanceOfType(result, typeof(NotFoundObjectResult));
        Assert
            .IsFalse((bool)((NotFoundObjectResult)result).Value);
    }

    [TestMethod]
    public void Delete_CorrectRequest_ShouldCorrectDeleted()
    {
        var expProfile = new Profile
        {
            SurName = "testSurName",
            UserId = "test",
        };
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        journalContext.Profiles.Add(expProfile);
        journalContext.SaveChanges();
        var controller = new ApiProfileController(journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Delete(expProfile.Id).Result;

        Assert
            .IsInstanceOfType(result, typeof(OkObjectResult));
        Assert
            .IsTrue((bool)((OkObjectResult)result).Value);
        Assert
            .AreEqual(0, journalContext.Profiles.Count());
    }
}