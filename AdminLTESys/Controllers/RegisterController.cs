using System;
using System.Text;
using AdminLTESys.Models;
using Ambiel.AppService.UserApp;
using Ambiel.AppService.UserApp.Dtos;
using Ambiel.Utility.StringExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTESys.Controllers
{
    public class RegisterController : Controller
    {
        private IUserAppService _userAppService;

        public RegisterController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(RegisterModel model)
        {           
            bool isUserExists = _userAppService.IsUserNameExists(model.UserName);
            if (isUserExists)
            {
                ModelState.AddModelError("UserName", "用户名已被占用");
            }
            if (ModelState.IsValid)
            {
                model.Password = model.Password.ToMD5String("ambiel1");
                model.RePassword =  model.RePassword.ToMD5String("ambiel1");
                model.DepartmentId = "1";
                UserDto userDto = new UserDto();
                userDto.CreateTime = DateTime.Now;
                userDto.EMail = model.Email;
                userDto.UserName = model.UserName;
                userDto.Name = "一般用户";
                userDto.Password = model.Password;
                var user =  _userAppService.InsertOrUpdate(userDto);
                if (user != null)
                {
                    Response.WriteAsync("<script>alert('注册成功');window.location.href='./Login/Index';</script>");  
                    //return RedirectToAction("Index","Login");
                }
            }

            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }
    }
}