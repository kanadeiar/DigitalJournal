namespace DigitalJournalTests.Controllers;

[TestClass]
public class AccountControllerTests
{
    #region Данные аккаунта

    [TestMethod]
    public void Index_SendCorrectRequestNotIsAuthenticated_ShouldCorrectView()
    {
        var expectedName = "TestName";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectRequestNotIsAuthenticated_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerMock.Object, roleManagerMock.Object, signInManagerMock.Object, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedName) }))
                }
            }
        };

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.IndexWebModel));
    }

    [TestMethod]
    public void Index_SendCorrectRequestIsAuthenticated_ShouldCorrectView()
    {
        var profileId = 1;
        var expectedName = "TestName";
        var expectedSurName = "TestSurName";
        var expectedRole = "TestRole";
        var expectedDescription = "TestDescriprionRole";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
            ProfileId = profileId
        };
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        IList<string> roles = new List<string> { expectedRole };
        userManagerMock
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(roles));
        var roleManagerMock = new Mock<RoleManagerMock>();
        IQueryable<Role> rolesQuery = Enumerable.Range(1, 1).Select(r =>
            new Role
            {
                Name = expectedRole,
                Description = expectedDescription
            }).AsQueryable();
        roleManagerMock
            .Setup(_ => _.Roles)
            .Returns(rolesQuery);
        var signInManagerMock = new Mock<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectRequestIsAuthenticated_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        journalContext.Profiles.Add(new Profile
        {
            Id = profileId,
            SurName = expectedSurName,
            UserId = user.Id,
        });
        journalContext.SaveChanges();
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(userManagerMock.Object, roleManagerMock.Object, signInManagerMock.Object, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.IndexWebModel));
        var model = view.Model as AccountController.IndexWebModel;
        Assert
            .AreEqual(user.Id, model.User.Id);
        Assert
            .AreEqual(expectedName, model.User.UserName);
        Assert
            .AreEqual(expectedSurName, model.Profile.SurName);
        Assert
            .AreEqual(1, model.UserRoleNames.Count());
        Assert
            .AreEqual(expectedDescription, model.UserRoleNames.First());
        userManagerMock
            .Verify(_ => _.FindByNameAsync(expectedName), Times.Once);
        userManagerMock
            .Verify(_ => _.GetRolesAsync(It.IsAny<User>()), Times.Once);
    }

    #endregion

    #region Регистрация

    [TestMethod]
    public void Register_SendCorrectRequest_ShouldCorrectView()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);

        var result = controller.Register();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.RegisterWebModel));
    }

    [TestMethod]
    public void Register_SendPostInvalidModel_ShouldCorrectView()
    {
        var expectedName = "TestName";
        var expectedPassword = "123";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendPostInvalidModel_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new AccountController.RegisterWebModel
        {
            UserName = expectedName,
            Password = expectedPassword,
            PasswordConfirm = expectedPassword,
        };
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Register(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(AccountController.RegisterWebModel));
    }

    [TestMethod]
    public void Register_SendPostSuccessRequest_ShouldCorrectRedirect()
    {
        var expectedName = "TestName";
        var expectedSurName = "TestSurName";
        var expectedPassword = "123";
        var userManagerMock = new Mock<UserManagerMock>();
        User callbackUser = default;
        string callbackPassword = default;
        userManagerMock
            .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success))
            .Callback((User u, string p) =>
            {
                callbackUser = u;
                callbackPassword = p;
            });
        userManagerMock
            .Setup(_ => _.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendPostSuccessRequest_ShouldCorrectRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new AccountController.RegisterWebModel
        {
            SurName = expectedSurName,
            FirstName = "test",
            Patronymic = "test",
            Email = "test@example.com",
            Birthday = new DateTime(2021, 1, 1),
            UserName = expectedName,
            Password = expectedPassword,
            PasswordConfirm = expectedPassword,
        };
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerMock.Object, journalContext);

        var result = controller.Register(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirectResult = (RedirectToActionResult)result;
        Assert
            .AreEqual("Home", redirectResult.ControllerName);
        Assert
            .AreEqual(nameof(HomeController.Index), redirectResult.ActionName);
        userManagerMock
            .Verify(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
        userManagerMock
            .Verify();
        signInManagerMock
            .Verify(_ => _.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
        signInManagerMock
            .Verify();
        Assert
            .IsNotNull(callbackUser);
        Assert
            .AreEqual(expectedPassword, callbackPassword);
        var profile = journalContext.Profiles.SingleOrDefault(p => p.SurName == expectedSurName);
        Assert
            .IsInstanceOfType(profile, typeof(Profile));
        Assert
            .AreEqual(callbackUser.ProfileId, profile.Id);
        Assert
            .AreEqual(callbackUser.Id, profile.UserId);
        Assert
            .AreEqual("test@example.com", callbackUser.Email);
        Assert
            .AreEqual(new DateTime(2021, 1, 1), profile.Birthday);
    }

    [TestMethod]
    public void Register_SendPostWithErrorRequest_ShouldCorrectView()
    {
        var expectedName = "TestName";
        var expectedPassword = "123";
        var expectedErrorCode = "Произошла неизвестная ошибка";
        var expectedErrorDescription = "TestDescription";
        var userManagerMock = new Mock<UserManagerMock>();
        var errors = new[]
        {
            new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
        };
        userManagerMock
            .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Failed(errors)));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendPostWithErrorRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new AccountController.RegisterWebModel
        {
            UserName = expectedName,
            Password = expectedPassword,
            PasswordConfirm = expectedPassword,
        };
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext);

        var result = controller.Register(model).Result;

        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var returnErrors = controller.ModelState[string.Empty].Errors;
        Assert
            .AreEqual(expectedErrorCode, returnErrors.FirstOrDefault().ErrorMessage);
        userManagerMock
            .Verify(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
        userManagerMock
            .Verify();
    }

    #endregion

    #region Вход пользователей

    [TestMethod]
    public void Login_SendCorrectRequest_ShouldCorrectView()
    {
        var expectedReturnUrl = "testUrl";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Login_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);

        var result = controller.Login(expectedReturnUrl);

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(AccountController.LoginWebModel));
        var loginModel = viewResult.Model as AccountController.LoginWebModel;
        Assert.AreEqual(expectedReturnUrl, loginModel.ReturnUrl);
    }

    [TestMethod]
    public void Login_SendPostInvalidModel_ShouldCorrectView()
    {
        var expectedErrorCode = "Test";
        var expectedErrorMessage = "Message";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Login_SendPostInvalidModel_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new AccountController.LoginWebModel();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);
        controller.ModelState.AddModelError(expectedErrorCode, expectedErrorMessage);

        var result = controller.Login(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(AccountController.LoginWebModel));
    }

    [TestMethod]
    public void Login_SendPostReturnUrlRequest_ShouldCorrectRedirect()
    {
        var expectedUserName = "TestUser";
        var expectedPassword = "123";
        var expectedReturnUrl = "testUrl";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        signInManagerMock
            .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Login_SendPostReturnUrlRequest_ShouldCorrectRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new AccountController.LoginWebModel
        {
            UserName = expectedUserName,
            Password = expectedPassword,
            ReturnUrl = expectedReturnUrl,
        };
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerMock.Object, journalContext);

        var result = controller.Login(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(LocalRedirectResult));
        var redirectResult = result as LocalRedirectResult;
        Assert
            .AreEqual(expectedReturnUrl, redirectResult.Url);
        signInManagerMock
            .Verify(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()));
        signInManagerMock
            .Verify();
    }

    [TestMethod]
    public void Login_SendPostFailedRequest_ShouldErrorView()
    {
        var expectedErrorCode = "Ошибка в имени пользователя, либо в пароле при входе в систему";
        var expectedUserName = "TestUser";
        var expectedPassword = "123";
        var expectedReturnUrl = "testUrl";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        signInManagerMock
            .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Login_SendPostFailedRequest_ShouldErrorView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new AccountController.LoginWebModel
        {
            UserName = expectedUserName,
            Password = expectedPassword,
            ReturnUrl = expectedReturnUrl,
        };
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerMock.Object, journalContext);

        var result = controller.Login(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(AccountController.LoginWebModel));
        var loginModel = (AccountController.LoginWebModel)viewResult.Model;
        Assert
            .AreEqual(expectedReturnUrl, loginModel.ReturnUrl);
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var returnErrors = controller.ModelState[string.Empty].Errors;
        Assert
            .AreEqual(expectedErrorCode, returnErrors.FirstOrDefault().ErrorMessage);
        signInManagerMock
            .Verify(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        signInManagerMock
            .Verify();
    }

    #endregion

    #region Редактирование своих данных

    [TestMethod]
    public void Edit_SendCorrectRequest_ShouldCorrectView()
    {
        var profileId = 1;
        var expectedName = "TestName";
        var expectedSurName = "TestSurName";
        var expectedPassword = "123";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "test",
            Patronymic = "test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Edit().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.EditWebModel));
        var model = view.Model as AccountController.EditWebModel;
        Assert
            .AreEqual(expectedSurName, model.SurName);
        userManagerMock
            .Verify(_ => _.FindByNameAsync(It.IsAny<string>()));
        userManagerMock
            .Verify();
    }

    [TestMethod]
    public void Edit_SendNotFoundRequest_ShouldNotFound()
    {
        var expectedName = "TestName";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendNotFoundRequest_ShouldNotFound)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Edit().Result;

        Assert
            .IsInstanceOfType(result, typeof(NotFoundResult));
        userManagerMock
            .Verify(_ => _.FindByNameAsync(It.IsAny<string>()));
        userManagerMock
            .Verify();
    }

    [TestMethod]
    public void Edit_SendPostInvalidModel_ShouldCorrectView()
    {
        var expectedSurName = "TestSurName";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostInvalidModel_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var editModel = new AccountController.EditWebModel
        {
            SurName = expectedSurName,
        };
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Edit(editModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.EditWebModel));
        var model = view.Model as AccountController.EditWebModel;
        Assert
            .AreEqual(expectedSurName, model.SurName);
    }

    [TestMethod]
    public void Edit_SendPostCorrectModel_ShouldCorrectRedirect()
    {
        var expectedName = "TestName";
        var expectedSurName = "TestSurName";
        var updatedExpectedSurName = "UpdatedSurName";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        userManagerMock
            .Setup(_ => _.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(IdentityResult.Success));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostCorrectModel_ShouldCorrectRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "test",
            Patronymic = "test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var editModel = new AccountController.EditWebModel
        {
            SurName = updatedExpectedSurName,
            FirstName = "updated",
            Patronymic = "updated",
            Email = "test@email.com",
            Birthday = new DateTime(2021,1,1),
        };
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Edit(editModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirect = result as RedirectToActionResult;
        Assert
            .AreEqual("Account", redirect.ControllerName);
        Assert
            .AreEqual("Index", redirect.ActionName);
        Assert
            .AreEqual(updatedExpectedSurName, profile.SurName);        
        Assert
            .AreEqual("test@email.com", user.Email);
        Assert
            .AreEqual(new DateTime(2021, 1, 1), profile.Birthday);
        userManagerMock
            .Verify(_ => _.FindByNameAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.UpdateAsync(It.IsAny<User>()));
        userManagerMock
            .Verify();
    }

    [TestMethod]
    public void Edit_SendPostErrorUpdate_ShouldCorrectView()
    {
        var expectedName = "TestName";
        var expectedSurName = "TestSurName";
        var updatedExpectedSurName = "UpdatedSurName";
        var expectedErrorCode = "Произошла неизвестная ошибка";
        var expectedErrorDescription = "TestDescription";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        userManagerMock
            .Setup(_ => _.UpdateAsync(It.IsAny<User>()));
        var errors = new[]
        {
            new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
        };
        userManagerMock
            .Setup(_ => _.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(IdentityResult.Failed(errors)));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostErrorUpdate_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "test",
            Patronymic = "test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var editModel = new AccountController.EditWebModel();
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Edit(editModel).Result;

        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.EditWebModel));
        var returnErrors = controller.ModelState[string.Empty].Errors;
        Assert
            .AreEqual(expectedErrorCode, returnErrors.FirstOrDefault().ErrorMessage);
        userManagerMock
            .Verify(_ => _.FindByNameAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.UpdateAsync(It.IsAny<User>()));
        userManagerMock
            .Verify();
    }

    #endregion

    #region Смена пароля

    [TestMethod]
    public void Password_SendCorrectRequest_ShouldCorrectView()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Password_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);

        var result = controller.Password();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.PasswordWebModel));
    }

    [TestMethod]
    public void Password_SendPostInvalidModel_ShouldCorrectView()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Password_SendPostInvalidModel_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var editModel = new AccountController.PasswordWebModel();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Password(editModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.PasswordWebModel));
        var model = view.Model as AccountController.PasswordWebModel;
    }

    [TestMethod]
    public void Password_SendPostCorrectModel_ShouldCorrectView()
    {
        var oldPassword = "old";
        var newPassword = "new";
        var expectedName = "TestName";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        signInManagerMock
            .Setup(_ => _.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
        userManagerMock
            .Setup(_ => _.RemovePasswordAsync(It.IsAny<User>()));
        userManagerMock
            .Setup(_ => _.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Password_SendPostCorrectModel_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var editModel = new AccountController.PasswordWebModel
        {
            OldPassword = oldPassword,
            Password = newPassword,
            PasswordConfirm = newPassword,
        };
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerMock.Object, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Password(editModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirect = result as RedirectToActionResult;
        Assert
            .AreEqual("Account", redirect.ControllerName);
        Assert
            .AreEqual("Index", redirect.ActionName);
        userManagerMock
            .Verify(_ => _.FindByNameAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.RemovePasswordAsync(It.IsAny<User>()));
        userManagerMock
            .Verify(_ => _.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>()));
        userManagerMock
            .Verify();
        signInManagerMock
            .Verify(_ => _.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()));
        signInManagerMock
            .Verify();
    }

    #endregion

    #region Выход из системы

    [TestMethod]
    public void Logout_SendRequestWithReturnUrl_ShouldCorrectRedirect()
    {
        var expectedReturnUrl = "testUrl";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        signInManagerMock
            .Setup(_ => _.SignOutAsync());
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Logout_SendRequestWithReturnUrl_ShouldCorrectRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerMock.Object, journalContext);

        var result = controller.Logout(expectedReturnUrl).Result;

        Assert
            .IsInstanceOfType(result, typeof(LocalRedirectResult));
        var redirectResult = (LocalRedirectResult)result;
        Assert
            .AreEqual(expectedReturnUrl, redirectResult.Url);
        signInManagerMock
            .Verify(_ => _.SignOutAsync());
        signInManagerMock
            .Verify();
    }

    #endregion

    #region Отказ в доступе

    [TestMethod]
    public void AccessDenied_SendCorrectRequest_ShouldCorrectView()
    {
        var returnUrl = "return";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);

        var result = controller.AccessDenied();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
    }

    #endregion

    #region WebAPI

    [TestMethod]
    public void IsNameFree_SendRequest_ShouldCorrectJson()
    {
        var expectedName = "TestName";
        var loggerStub = Mock
            .Of<ILogger<AccountController>>();
        var userManagerMock = new Mock<UserManagerMock>();
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext);

        var result = controller.IsNameFree(expectedName).Result;

        Assert
            .IsInstanceOfType(result, typeof(JsonResult));
        var json = (JsonResult)result;
        Assert
            .AreEqual("true", json.Value);
        userManagerMock
            .Verify(_ => _.FindByNameAsync(expectedName), Times.Once);
        userManagerMock
            .Verify();
    }

    #endregion
}

#region Вспомогательное

public class UserManagerMock : UserManager<User>
{
    /// <summary> Мок подменяющий сервис управления пользователями </summary>
    public UserManagerMock()
        : base(new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object)
    {
    }
}

public class RoleManagerMock : RoleManager<Role>
{
    /// <summary> Мок подменяющий сервис управления ролями </summary>
    public RoleManagerMock()
        : base(new Mock<IRoleStore<Role>>().Object,
            new IRoleValidator<Role>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<Role>>>().Object)
    {
    }
}

public class SignInManagerMock : SignInManager<User>
{
    /// <summary> Мок подменяющий вход/выход пользователей </summary>
    public SignInManagerMock()
        : base(new Mock<UserManagerMock>().Object,
            new HttpContextAccessor(),
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object)
    {
    }
}

#endregion

