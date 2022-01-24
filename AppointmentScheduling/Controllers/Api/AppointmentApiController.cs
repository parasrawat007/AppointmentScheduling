using AppointmentScheduling.Models.ViewModel;
using AppointmentScheduling.Services;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers.Api
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;
        public AppointmentApiController(IAppointmentService appointmentService,IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalenderData")]
        public IActionResult SaveCalenderData(AppointmentViewModel model)
        {
            CommonResponse<int> response = new();
            try
            {
                response.status = _appointmentService.AddUpdate(model).Result;
                if (response.status == 1)
                {
                    response.message = Helper.appointmentUpdated;
                }
                else if (response.status==2)
                {
                    response.message = Helper.appointmentAdded;
                }
            }
            catch (Exception e)
            {

                response.message=e.Message;
                response.status = Helper.failure_code;
            }
           
            return Ok(response  );
        }
    }
}
