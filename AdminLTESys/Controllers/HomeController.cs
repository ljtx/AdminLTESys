using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminLTESys.Common;
using Microsoft.AspNetCore.Mvc;
using AdminLTESys.Models;
using Ambiel.AdoNet;
using Ambiel.RedisClient;
using log4net;
using Microsoft.Extensions.Options;

namespace AdminLTESys.Controllers
{
    public class HomeController : AmbielBaseController
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(HomeController));
        private IOptions<RedisConfigOptions> _config;
        private RedisClient redisClient = null;
        public HomeController(IOptions<RedisConfigOptions> config)
        {
            _config = config;
            redisClient = RedisConnectionSingleton.GetInstance(config);
        }

        public IActionResult Index()
        {
            //DBUtils.ExecuteScalar(CommandType.Text, "select * from users");
            //SqlParam[] sqlParams = new SqlParam[]{new SqlParam("@Id","cf9f7f8f-4569-4d33-b860-bb7b54e41083") };
          // int val =  DBUtils.ExecuteNonQuery(CommandType.Text,@"update users set EMail='864305382@qq.com'
                     // where Id=@Id",sqlParams);
            log.Info("登陆主页了");
            //DBUtils.ExecuteScalar(CommandType.Text, "select * from users");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}