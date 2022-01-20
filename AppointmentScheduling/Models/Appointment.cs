﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndeDate { get; set; }
        public int Duration { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public bool IsDoctorApproved { get; set; }
        public string AdminId { get; set; }

    }
}
