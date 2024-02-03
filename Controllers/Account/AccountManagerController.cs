﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkHT.Models.Users;
using SocialNetworkHT.ViewModels.Account;
using System.Security.Policy;
using System.Threading.Tasks;
using SocialNetworkHT.Extentions;

namespace SocialNetworkHT.Controllers.Account
{
    public class AccountManagerController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }


        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Home/Login");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = _mapper.Map<User>(model);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage", "AccountManager");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return RedirectToAction("Index", "Home"); 
        }


        [Route("MyPage")]
        [HttpGet]
        [Authorize]
        public IActionResult MyPage()
        {
            var user = User;

            var result = _userManager.GetUserAsync(user);

            return View("MyPage", new UserViewModel(result.Result));
        }


        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [Route("UserEdit")]
        [HttpGet]
        public IActionResult UserEdit()
        {
            var user = User;

            var result = _userManager.GetUserAsync(user);

            var editmodel = _mapper.Map<UserEditViewModel>(result.Result);

            return View("UserEdit", editmodel);
        }

        [Authorize]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                user.Convert(model);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage", "AccountManager");
                }
                else
                {
                    return RedirectToAction("UserEdit", "AccountManager");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("UserEdit", model);
            }
        }

    }
}
