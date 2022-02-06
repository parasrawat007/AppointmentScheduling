﻿using AppointmentScheduling.Services;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers
{
    //[Authorize(Roles = Helper.Admin)]
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }
       //[Authorize(Roles = Helper.Admin)]
        public IActionResult Index()
        {
            ViewBag.DoctorList=_service.GetDoctorList();
            ViewBag.PateintList=_service.GetPateintList();
            ViewBag.Duration = Helper.GetTimeDropDown();
            return View();
        }


    }
}
