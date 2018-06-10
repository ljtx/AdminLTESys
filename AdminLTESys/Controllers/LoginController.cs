using AdminLTESys.Models;
using Ambiel.AppService.UserApp;
using Ambiel.Utility.Convert;
using Ambiel.Utility.StringExtension;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTESys.Controllers
{
    public class LoginController : Controller
    {
        private IUserAppService _userAppService;
        
        public LoginController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //检查用户信息
                var user = _userAppService.CheckUser(model.UserName, model.Password.ToMD5String("ambiel1"));
                if (user != null)
                {
                    //记录Session
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                    //跳转到系统首页
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorInfo = "用户名或密码错误。";
                return View();
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