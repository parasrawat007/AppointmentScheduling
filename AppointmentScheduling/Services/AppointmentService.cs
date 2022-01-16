﻿using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModel;
using AppointmentScheduling.Utility;
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
            /*
              var doctors = (from user in _db.Users
                     join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                     join roles in _db.Roles.Where(x=>x.Name==Helper.Doctor) on userRoles.RoleId equals roles.Id
                     select new DoctorViewModel { 
                        Id=user.Id,
                        Name=user.Name
                     }).ToList();
             */

            //var query = _db.Users.Join(_db.UserRoles, u => u.Id, r => r.UserId, (user, role) => new { Id = user.Id, Name = user.Name, RoleId = role.RoleId });
            //var doctors = _db.Roles.Where(r=>r.Name==Helper.Doctor).Join(query, r => r.Id, q=>q.RoleId, (role, user) => new DoctorViewModel{ Id = user.Id, Name = user.Name }).ToList();

            var role = _db.Roles.First(r => r.Name == Helper.Doctor);
            var map = _db.UserRoles.Where(u => u.RoleId == role.Id).Select(u=>u.UserId);
            var doctors = _db.Users.Where(u => map.Contains(u.Id)).Select(u=>new DoctorViewModel { Id=u.Id,Name=u.Name}).ToList();
            
            return doctors;
        }

        public List<PateintViewModel> GetPateintList()
        {
            var role = _db.Roles.First(r => r.Name == Helper.Patient);
            var map = _db.UserRoles.Where(u => u.RoleId == role.Id).Select(u => u.UserId);
            var doctors = _db.Users.Where(u => map.Contains(u.Id)).Select(u => new PateintViewModel { Id = u.Id, Name = u.Name }).ToList();

            return doctors;
        }
    }
}
