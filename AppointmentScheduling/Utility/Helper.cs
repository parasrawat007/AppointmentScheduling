using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Utility
{
    public static class Helper
    {
        public const string Admin = "Admin";
        public const string Patient = "Patient";
        public const string Doctor = "Doctor";
        public static string appointmentAdded = "Appointment added successfully.";
        public static string appointmentUpdated = "Appointment updated successfully.";
        public static string appointmentDeleted = "Appointment deleted successfully.";
        public static string appointmentExists = "Appointment for selected date and time already exists.";
        public static string appointmentNotExists = "Appointment not exists.";
        public static string meetingConfirm = "Meeting confirm successfully.";
        public static string meetingConfirmError = "Error while confirming meeting.";
        public static string appointmentAddError = "Something went wront, Please try again.";
        public static string appointmentUpdatError = "Something went wront, Please try again.";
        public static string somethingWentWrong = "Something went wront, Please try again.";
        public static int success_code = 1;
        public static int failure_code = 0;

        public static List<SelectListItem> GetRolesForDropDown(bool IsAdmin)
        {
            if (IsAdmin)
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem{Value=Admin,Text=Admin}
                };
            }
            else
            {
                return new List<SelectListItem>()
                {                   
                    new SelectListItem{Value=Patient,Text=Patient},
                    new SelectListItem{Value=Doctor,Text=Doctor }
                };
            }
        }

        public static List<SelectListItem> GetTimeDropDown()
        {
            int min = 60;
            List<SelectListItem> Duration = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                Duration.Add(new SelectListItem { Value = min.ToString(), Text = i + " Hr" });
                min = min + 30;
                Duration.Add(new SelectListItem { Value = min.ToString(), Text = i + " Hr 30 Min" });
                min = min + 30;
            }
            return Duration;
        }
    }
}
