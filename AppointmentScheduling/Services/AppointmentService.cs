using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<DoctorViewModel> GetDoctorList()
        {
            var doctors = _db.Users.Select(e => new DoctorViewModel
            {
                Id=e.Id, 
                Name=e.Name 
            }).ToList();
            return doctors;
        }

        public List<PateintViewModel> GetPateintList()
        {
            throw new NotImplementedException();
        }
    }
}
