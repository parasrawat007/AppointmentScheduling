using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModel;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _EmailSender;

        public AppointmentService(ApplicationDbContext db,IEmailSender EmailSender)
        {
            _db = db;
            _EmailSender = EmailSender;
        }

        public async Task<int> AddUpdate(AppointmentViewModel model)
        {
            var StartDate = DateTime.Parse(model.StartDate);
            var EndDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            var patient = _db.Users.FirstOrDefault(u => u.Id==model.PatientId);
            var doctor = _db.Users.FirstOrDefault(u => u.Id==model.DoctorId);
            if (model != null && model.Id>0)
            {
                var appointmet = _db.Appointments.FirstOrDefault(a => a.Id == model.Id);

                appointmet.Title = model.Title;
                appointmet.StartDate = StartDate;
                appointmet.EndDate = EndDate;
                appointmet.Duration = model.Duration;
                appointmet.Description = model.Description;
                appointmet.DoctorId = model.DoctorId;
                appointmet.PatientId = model.PatientId;
                appointmet.AdminId = model.AdminId;
                appointmet.IsDoctorApproved = false;

                _db.Appointments.Update(appointmet);
                await _db.SaveChangesAsync();
              
                return 1;
            }
            else
            {
                Appointment appointment = new()
                { 
                    Title=model.Title,
                    StartDate=StartDate,
                    EndDate=EndDate,
                    Duration=model.Duration,
                    Description=model.Description,
                    DoctorId = model.DoctorId,
                    PatientId=model.PatientId,
                    AdminId=model.AdminId,
                    IsDoctorApproved=false  
                };
                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();

                await _EmailSender.SendEmailAsync(doctor.Email, "Appointment Created", $"Your Appointment with {patient.Name} is crated is in pending status"); 
                await _EmailSender.SendEmailAsync(patient.Email, "Appointment Created", $"Your Appointment with {doctor.Name} is crated is in pending status"); 
                return 2;
            }
           
        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return await _db.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> Delete(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
                return await _db.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public List<AppointmentViewModel> DoctorEventsById(string DoctorId)
        {
            return _db.Appointments.Where(a => a.DoctorId == DoctorId).Select(a=>new AppointmentViewModel {
                Id = a.Id,
                DoctorId =DoctorId,
                AdminId=a.AdminId,
                Description=a.Description,
                Duration=a.Duration,
                PatientId=a.PatientId,
                Title=a.Title,
                StartDate=a.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate=a.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDoctorApproved=a.IsDoctorApproved
            }).ToList();
        }

        public AppointmentViewModel GetById(int id)
        {
            return _db.Appointments.Where(a => a.Id == id).Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                AdminId = a.AdminId,
                Description = a.Description,
                Duration = a.Duration,
                PatientId = a.PatientId,
                Title = a.Title,
                StartDate = a.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = a.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDoctorApproved = a.IsDoctorApproved,
                PatientName=_db.Users.SingleOrDefault(x=>x.Id== a.PatientId).Name,
                DoctorName=_db.Users.SingleOrDefault(x=>x.Id== a.DoctorId).Name,
            }).SingleOrDefault();
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

        public List<AppointmentViewModel> PatientEventsById(string PatientId)
        {
            return _db.Appointments.Where(a => a.PatientId == PatientId).Select(a => new AppointmentViewModel
            {
                Id=a.Id,
                DoctorId = a.DoctorId,
                AdminId = a.AdminId,
                Description = a.Description,
                Duration = a.Duration,
                PatientId = PatientId,
                Title = a.Title,
                StartDate = a.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = a.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDoctorApproved = a.IsDoctorApproved
            }).ToList();
        }
    }
}
