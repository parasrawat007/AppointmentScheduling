using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Patient = "Patient";
        public static string Doctor = "Doctor";

        public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem{Value=Admin,Text=Admin},
                new SelectListItem{Value=Patient,Text=Patient},
                new SelectListItem{Value=Doctor,Text=Doctor }
            };
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
