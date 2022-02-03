namespace DigitalJournalTests.Controllers;

[TestClass]
public class AccountControllerTests
{
    readonly Random random = new Random();

    #region Данные аккаунта

    [TestMethod]
    public void Index_SendCorrectRequestNotIsAuthenticated_ShouldCorrectView()
    {
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake)
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
            .IsInstanceOfType(view.Model, typeof(UserIndexWebModel));
    }

    [TestMethod]
    public void Index_SendCorrectRequestIsAuthenticated_ShouldCorrectView()
    {
        var expectedModel = new UserIndexWebModel
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
        var controller = new AccountController(serviceFake.Object)
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
            .IsInstanceOfType(view.Model, typeof(UserIndexWebModel));
        var model = view.Model as UserIndexWebModel;
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
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake);

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
        var expectedModel = new UserRegisterWebModel
        {
            UserName = "TestName",
            Password = "123",
            PasswordConfirm = "123",
        };
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake);
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
        var controller = new AccountController(serviceFake.Object);

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
        var controller = new AccountController(serviceFake.Object);

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
        var expectedReturnUrl = "testUrl";
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake);

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
        var expectedErrorCode = "Test";
        var expectedErrorMessage = "Message";
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake);
        controller.ModelState.AddModelError(expectedErrorCode, expectedErrorMessage);

        var result = controller.Login(new UserLoginWebModel()).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(UserLoginWebModel));
    }

    [TestMethod]
    public void Login_SendPostReturnUrlRequest_ShouldCorrectRedirect()
    {
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
        var controller = new AccountController(serviceFake.Object);

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
        var controller = new AccountController(serviceFake.Object);

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
        var controller = new AccountController(serviceFake.Object)
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
        var expectedName = "TestName";
        var serviceFake = new Mock<IAccountService>();
        (bool, UserEditWebModel?) returnValues = (false, null);
        serviceFake
            .Setup(_ => _.GetEditModelByName(It.IsAny<string>()))
            .Returns(Task.FromResult(returnValues));
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(serviceFake.Object)
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
        var editModel = new UserEditWebModel
        {
            SurName = expectedSurName,
        };
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Edit(editModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserEditWebModel));
        var model = view.Model as UserEditWebModel;
        Assert
            .AreEqual(expectedSurName, model.SurName);
    }

    [TestMethod]
    public void Edit_SendPostCorrectModel_ShouldCorrectRedirect()
    {
        var expectedName = "TestName";
        var expectedModel = new UserEditWebModel
        {
            SurName = "updatedTestSurname",
            FirstName = "updated",
            Patronymic = "updated",
            Email = "test@email.com",
            Birthday = new DateTime(2021, 1, 1),
        };
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.RequestUpdateUserProfile(It.IsAny<string>(), It.IsAny<UserEditWebModel>()))
            .Returns(Task.FromResult((true, Array.Empty<string>())));
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(serviceFake.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Edit(expectedModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirect = result as RedirectToActionResult;
        Assert
            .AreEqual("Account", redirect.ControllerName);
        Assert
            .AreEqual("Index", redirect.ActionName);
        serviceFake
            .Verify(_ => _.RequestUpdateUserProfile(expectedName, It.IsAny<UserEditWebModel>()), Times.Once);
        serviceFake
            .Verify();
    }

    [TestMethod]
    public void Edit_SendPostErrorUpdate_ShouldCorrectView()
    {
        var expectedName = "TestName";
        var expectedErrorCode = "Произошла неизвестная ошибка";
        var expectedErrors = new[]
        {
            expectedErrorCode,
        };
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.RequestUpdateUserProfile(It.IsAny<string>(), It.IsAny<UserEditWebModel>()))
            .Returns(Task.FromResult((false, expectedErrors)));
        var controller = new AccountController(serviceFake.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Edit(new UserEditWebModel()).Result;

        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserEditWebModel));
        var returnErrors = controller.ModelState[string.Empty].Errors;
        Assert
            .AreEqual(expectedErrorCode, returnErrors.FirstOrDefault().ErrorMessage);
    }

    #endregion

    #region Смена пароля

    [TestMethod]
    public void Password_SendCorrectRequest_ShouldCorrectView()
    {
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake);

        var result = controller.Password();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserPasswordWebModel));
    }

    [TestMethod]
    public void Password_SendPostInvalidModel_ShouldCorrectView()
    {
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Password(new UserPasswordWebModel()).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserPasswordWebModel));
    }

    [TestMethod]
    public void Password_SendPostErrorUpdatePassword_ShouldCorrectErrorView()
    {
        var expectedUserName = "Testname";
        var expectedErrors = new string[] { "TestError" };
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.CheckAndChangePassword(It.IsAny<string>(), It.IsAny<UserPasswordWebModel>()))
            .Returns(Task.FromResult((false, expectedErrors )));
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedUserName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(serviceFake.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Password(new UserPasswordWebModel()).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserPasswordWebModel));
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var returnErrors = controller.ModelState[string.Empty].Errors;
        Assert
            .AreEqual(expectedErrors.First(), returnErrors.FirstOrDefault().ErrorMessage);
        serviceFake
            .Verify(_ => _.CheckAndChangePassword(expectedUserName, It.IsAny<UserPasswordWebModel>()));
    }

    [TestMethod]
    public void Password_SendPostCorrectModel_ShouldCorrectView()
    {
        var expectedUserName = "Testname";
        var expectedEditModel = new UserPasswordWebModel
        {
            OldPassword = "old",
            Password = "new",
            PasswordConfirm = "new",
        };
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.CheckAndChangePassword(It.IsAny<string>(), It.IsAny<UserPasswordWebModel>()))
            .Returns(Task.FromResult((true, Array.Empty<string>())));
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedUserName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(serviceFake.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Password(expectedEditModel).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirect = result as RedirectToActionResult;
        Assert
            .AreEqual("Account", redirect.ControllerName);
        Assert
            .AreEqual("Index", redirect.ActionName);
        serviceFake
            .Verify(_ => _.CheckAndChangePassword(expectedUserName, It.IsAny<UserPasswordWebModel>()));
    }

    #endregion

    #region Выход из системы

    [TestMethod]
    public void Logout_SendRequestWithReturnUrl_ShouldCorrectRedirect()
    {
        var expectedReturnUrl = "testUrl";
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.SignOut());
        var controller = new AccountController(serviceFake.Object);

        var result = controller.Logout(expectedReturnUrl).Result;

        Assert
            .IsInstanceOfType(result, typeof(LocalRedirectResult));
        var redirectResult = (LocalRedirectResult)result;
        Assert
            .AreEqual(expectedReturnUrl, redirectResult.Url);
        serviceFake
            .Verify(_ => _.SignOut());
    }

    #endregion

    #region Отказ в доступе

    [TestMethod]
    public void AccessDenied_SendCorrectRequest_ShouldCorrectView()
    {
        var serviceFake = Mock.Of<IAccountService>();
        var controller = new AccountController(serviceFake);

        var result = controller.AccessDenied();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
    }

    #endregion

    #region WebAPI

    [TestMethod]
    public void IsNameFree_SendCorrectRequest_ShouldCorrectJson()
    {
        var expectedName = "TestName";
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.UserNameIsFree(It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        var controller = new AccountController(serviceFake.Object);

        var result = controller.IsNameFree(expectedName).Result;

        Assert
            .IsInstanceOfType(result, typeof(JsonResult));
        var json = (JsonResult)result;
        Assert
            .AreEqual("true", json.Value);
        serviceFake
            .Verify(_ => _.UserNameIsFree(expectedName));
    }

    [TestMethod]
    public void IsNameFree_SendFountRequest_ShouldFalseJson()
    {
        var expectedName = "TestName";
        var serviceFake = new Mock<IAccountService>();
        serviceFake
            .Setup(_ => _.UserNameIsFree(It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        var controller = new AccountController(serviceFake.Object);

        var result = controller.IsNameFree(expectedName).Result;

        Assert
            .IsInstanceOfType(result, typeof(JsonResult));
        var json = (JsonResult)result;
        Assert
            .AreEqual("Такой логин уже занят другим пользователем", json.Value);
        serviceFake
            .Verify(_ => _.UserNameIsFree(expectedName));
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



