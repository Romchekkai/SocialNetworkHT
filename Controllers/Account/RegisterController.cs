using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetworkHT.Models.Users;
using SocialNetworkHT.ViewModels.Account;
using System.Threading.Tasks;

namespace SocialNetworkHT.Controllers.Account
{
    public class RegisterController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<RegisterController> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View("Home/Register");
        }

        [Route("RegisterPart2")]
        [HttpGet]
        public IActionResult RegisterPart2(RegisterViewModel model)
        {
            return View("RegisterPart2", model);
        }



        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            _logger.LogInformation("Попытка регистрации {model}", @model);
            
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    _logger.LogInformation("Ошибка {model}", @model);

                    try
                    {
                        throw new System.Exception("Тут ошибка!");

                    }
                    catch (System.Exception ex)
                    {

                        _logger.LogError(ex,"Ошибка {model}", @model);                        
                    }
                    

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("RegisterPart2", model);
        }
    }
}
