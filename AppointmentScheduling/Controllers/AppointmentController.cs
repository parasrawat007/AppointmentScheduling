using AppointmentScheduling.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            ViewBag.DoctorList=_service.GetDoctorList();
            ViewBag.PateintList=_service.GetPateintList();
            return View();
        }
    }
}
