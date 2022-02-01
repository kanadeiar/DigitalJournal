namespace DigitalJournalTests.Controllers;

[TestClass]
public class AccountControllerTests
{
    readonly Random random = new Random();

    #region Данные аккаунта

    [TestMethod]
    public void Index_SendCorrectRequestNotIsAuthenticated_ShouldCorrectView()
    {
        #region Удалить
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        #endregion

        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerMock.Object, roleManagerMock.Object, signInManagerMock.Object, journalContext, serviceFake)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                }
            }
        };

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(IndexWebModel));
    }

    [TestMethod]
    public void Index_SendCorrectRequestIsAuthenticated_ShouldCorrectView()
    {
        #region удалить
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var userManagerStub = Mock.Of<UserManagerMock>();
        var rolwManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        #endregion

        var expectedModel = new IndexWebModel
        {
            User = new User
            {
                UserName = "test",
            }
        };
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.GetIndexWebModel(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedModel));
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedModel.User.UserName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(userManagerStub, rolwManagerStub, signInManagerMock.Object, journalContext, serviceFake.Object)
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
            .IsInstanceOfType(view.Model, typeof(IndexWebModel));
        var model = view.Model as IndexWebModel;
        Assert
            .AreEqual(expectedModel.User.UserName, model.User.UserName);
        serviceFake
            .Verify(_ => _.GetIndexWebModel(expectedModel.User.UserName), Times.Once);
        serviceFake
            .Verify();
    }

    #endregion

    #region Регистрация

    [TestMethod]
    public void Register_SendCorrectRequest_ShouldCorrectView()
    {
        #region Удалить
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        #endregion

        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext, serviceFake);

        var result = controller.Register();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserRegisterWebModel));
    }

    [TestMethod]
    public void Register_SendPostInvalidModel_ShouldCorrectView()
    {
        #region del
        var expectedName = "TestName";
        var expectedPassword = "123";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendPostInvalidModel_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        #endregion

        var expectedModel = new UserRegisterWebModel
        {
            UserName = "TestName",
            Password = "123",
            PasswordConfirm = "123",
        };
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext, serviceFake);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Register(expectedModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(UserRegisterWebModel));
    }

    [TestMethod]
    public void Register_SendPostSuccessRequest_ShouldCorrectRedirect()
    {
        #region del
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
        #endregion del

        var expectedModel = new UserRegisterWebModel
        {
            SurName = "TestSurName",
            FirstName = "test",
            Patronymic = "test",
            Email = "test@example.com",
            Birthday = new DateTime(2021, 1, 1),
            UserName = "TestName",
            Password = "123",
            PasswordConfirm = "123",
        };
        var serviceFake = new Mock<IAccountService>();
        UserRegisterWebModel callbackModel = null;
        serviceFake
            .Setup(_ => _.RequestRegisterUser(It.IsAny<UserRegisterWebModel>()))
            .Returns(Task.FromResult((true, Array.Empty<string>())))
            .Callback((UserRegisterWebModel m) => { callbackModel = m; });
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerMock.Object, journalContext, serviceFake.Object);

        var result = controller.Register(expectedModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirectResult = (RedirectToActionResult)result;
        Assert
            .AreEqual("Home", redirectResult.ControllerName);
        Assert
            .AreEqual("Index", redirectResult.ActionName);
        serviceFake
            .Verify(_ => _.RequestRegisterUser(It.IsAny<UserRegisterWebModel>()), Times.Once);
        Assert
            .AreEqual(expectedModel.SurName, callbackModel.SurName);
    }

    [TestMethod]
    public void Register_SendPostWithErrorRequest_ShouldCorrectView()
    {
        #region del
        var expectedName = "TestName";
        var expectedPassword = "123";

        var expectedErrorDescription = "TestDescription";
        var userManagerMock = new Mock<UserManagerMock>();
        //var errors = new[]
        //{
        //    new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
        //};
        //userManagerMock
        //    .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
        //    .Returns(Task.FromResult(IdentityResult.Failed(errors)));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendPostWithErrorRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        #endregion del

        var expectedErrorCode = "Произошла неизвестная ошибка";
        var expectedModel = new UserRegisterWebModel
        {
            UserName = "TestName",
            Password = "123",
            PasswordConfirm = "123",
        };
        var expectedErrors = new[]
        {
            expectedErrorCode
        };
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.RequestRegisterUser(It.IsAny<UserRegisterWebModel>()))
            .Returns(Task.FromResult((false, expectedErrors)));
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext, serviceFake.Object);

        var result = controller.Register(expectedModel).Result;

        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var returnErrors = controller.ModelState[string.Empty].Errors;
        Assert
            .AreEqual(expectedErrorCode, returnErrors.FirstOrDefault().ErrorMessage);
        serviceFake
            .Verify(_ => _.RequestRegisterUser(It.IsAny<UserRegisterWebModel>()), Times.Once);
    }

    #endregion

    #region Вход пользователей

    [TestMethod]
    public void Login_SendCorrectRequest_ShouldCorrectView()
    {
        #region del

        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Login_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        #endregion

        var expectedReturnUrl = "testUrl";
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext, serviceFake);

        var result = controller.Login(expectedReturnUrl);

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(UserLoginWebModel));
        var loginModel = viewResult.Model as UserLoginWebModel;
        Assert.AreEqual(expectedReturnUrl, loginModel.ReturnUrl);
    }

    [TestMethod]
    public void Login_SendPostInvalidModel_ShouldCorrectView()
    {
        #region del
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Login_SendPostInvalidModel_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new UserLoginWebModel();
        #endregion

        var expectedErrorCode = "Test";
        var expectedErrorMessage = "Message";
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext, serviceFake);
        controller.ModelState.AddModelError(expectedErrorCode, expectedErrorMessage);

        var result = controller.Login(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(UserLoginWebModel));
    }

    [TestMethod]
    public void Login_SendPostReturnUrlRequest_ShouldCorrectRedirect()
    {
        #region del
        var expectedUserName = "TestUser";
        var expectedPassword = "123";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        signInManagerMock
            .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Login_SendPostReturnUrlRequest_ShouldCorrectRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        #endregion

        var expectedReturnUrl = "testUrl";
        var expectedModel = new UserLoginWebModel
        {
            UserName = "username",
            Password = "123",
            ReturnUrl = expectedReturnUrl,
        };
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.LoginPasswordSignIn(It.IsAny<UserLoginWebModel>()))
            .Returns(Task.FromResult(true));
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerMock.Object, journalContext, serviceFake.Object);

        var result = controller.Login(expectedModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(LocalRedirectResult));
        var redirectResult = result as LocalRedirectResult;
        Assert
            .AreEqual(expectedReturnUrl, redirectResult.Url);
    }

    [TestMethod]
    public void Login_SendPostFailedRequest_ShouldErrorView()
    {
        #region del

        var expectedUserName = "TestUser";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        signInManagerMock
            .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Login_SendPostFailedRequest_ShouldErrorView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        #endregion

        var expectedErrorCode = "Ошибка в имени пользователя, либо в пароле при входе в систему";
        var expectedReturnUrl = "testUrl";
        var expectedModel = new UserLoginWebModel
        {
            UserName = "username",
            Password = "123",
            ReturnUrl = expectedReturnUrl,
        };
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.LoginPasswordSignIn(It.IsAny<UserLoginWebModel>()))
            .Returns(Task.FromResult(false));
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerMock.Object, journalContext, serviceFake.Object);

        var result = controller.Login(expectedModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(UserLoginWebModel));
        var loginModel = (UserLoginWebModel)viewResult.Model;
        Assert
            .AreEqual(expectedReturnUrl, loginModel.ReturnUrl);
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var returnErrors = controller.ModelState[string.Empty].Errors;
        Assert
            .AreEqual(expectedErrorCode, returnErrors.FirstOrDefault().ErrorMessage);
    }

    #endregion

    #region Редактирование своих данных

    [TestMethod]
    public void Edit_SendCorrectRequest_ShouldCorrectView()
    {
        var expectedName = "TestName";
        #region del

        var profileId = 1;

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
        #endregion del

        var expectedModel = new UserEditWebModel
        {
            SurName = "testSurName",
        };
        var serviceFake = new Mock<IAccountService>();
        (bool, UserEditWebModel ?) returnValues = (true, expectedModel);
        serviceFake
            .Setup(_ => _.GetEditModelByName(It.IsAny<string>()))
            .Returns(Task.FromResult(returnValues));
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext, serviceFake.Object)
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
            .IsInstanceOfType(view.Model, typeof(UserEditWebModel));
        var model = view.Model as UserEditWebModel;
        Assert
            .AreEqual(expectedModel.SurName, model.SurName);
    }

    [TestMethod]
    public void Edit_SendNotFoundRequest_ShouldNotFound()
    {
        #region del
        var expectedName = "TestName";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendNotFoundRequest_ShouldNotFound)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        #endregion

        var serviceFake = new Mock<IAccountService>();
        (bool, UserEditWebModel?) returnValues = (false, null);
        serviceFake
            .Setup(_ => _.GetEditModelByName(It.IsAny<string>()))
            .Returns(Task.FromResult(returnValues));
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext, serviceFake.Object)
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
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext, serviceFake);
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


        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext, serviceFake)
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
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext, serviceFake)
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
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext, serviceFake);

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
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext, serviceFake);
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
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerMock.Object, journalContext, serviceFake)
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
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerMock.Object, journalContext, serviceFake);

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
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext, serviceFake);

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
        var userManagerMock = new Mock<UserManagerMock>();
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerStub, journalContext, serviceFake);

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
}



