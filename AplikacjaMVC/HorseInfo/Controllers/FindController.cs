﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HorseInfo.Controllers
{
    public class FindController : Controller
    {
        // GET: Find
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FindTraining()
        {
            return View();
        }
    }
}