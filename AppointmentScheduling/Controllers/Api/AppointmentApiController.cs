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
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentViewModel model)
        {
            CommonResponse<int> response = new();
            try
            {
                response.status = _appointmentService.AddUpdate(model).Result;
                if (response.status == 1)
                {
                    response.message = Helper.appointmentUpdated;
                }
                else if (response.status == 2)
                {
                    response.message = Helper.appointmentAdded;
                }
            }
            catch (Exception e)
            {

                response.message=e.Message;
                response.status = Helper.failure_code;
            }
           
            return Ok(response);
        }
        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string DoctorId)
        {
            CommonResponse<List<AppointmentViewModel>> response = new();
            try
            {
                if(role==Helper.Patient)
                {
                    response.dataenum = _appointmentService.PatientEventsById(loginUserId);
                    response.status = Helper.success_code;
                }
                else if (role == Helper.Doctor)
                {
                    response.dataenum = _appointmentService.DoctorEventsById(loginUserId);
                    response.status = Helper.success_code;
                }
                else
                {
                    response.dataenum = _appointmentService.DoctorEventsById(DoctorId);
                    response.status = Helper.success_code;
                }
            }
            catch (Exception e)
            {

                response.message = e.Message;
                response.status = Helper.failure_code;
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            CommonResponse<AppointmentViewModel> response = new();
            try
            {

                response.dataenum = _appointmentService.GetById(id);
                response.status = Helper.success_code;
            }
            catch (Exception e)
            {

                response.message = e.Message;
                response.status = Helper.failure_code;
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public IActionResult ConfirmEvent(int id)
        {
            CommonResponse<int> response = new();
            try
            {

                response.status =  _appointmentService.ConfirmEvent(id).Result;
                if (response.status > 0)
                {
                    response.status = Helper.success_code;
                    response.message = Helper.meetingConfirm;
                }
                else {

                    response.status = Helper.failure_code;
                    response.message = Helper.meetingConfirmError;
                }
               
            }
            catch (Exception e)
            {

                response.message = e.Message;
                response.status = Helper.failure_code;
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("DeleteAppointment/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            CommonResponse<int> response = new();
            try
            {

                response.status = await _appointmentService.Delete(id);
                response.message = response.status==1?Helper.appointmentDeleted:Helper.somethingWentWrong;
            }
            catch (Exception e)
            {

                response.message = e.Message;
                response.status = Helper.failure_code;
            }
            return Ok(response);
        }
    }
}
