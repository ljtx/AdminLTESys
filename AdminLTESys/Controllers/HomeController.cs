﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminLTESys.Common;
using Microsoft.AspNetCore.Mvc;
using AdminLTESys.Models;
using Ambiel.AdoNet;
using log4net;

namespace AdminLTESys.Controllers
{
    public class HomeController : AmbielBaseController
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(HomeController));
        public IActionResult Index()
        {
            //DBUtils.ExecuteScalar(CommandType.Text, "select * from users");
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