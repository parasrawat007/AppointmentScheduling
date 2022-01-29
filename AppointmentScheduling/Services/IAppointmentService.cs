using AppointmentScheduling.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public interface IAppointmentService
    {
        List<DoctorViewModel> GetDoctorList();
        List<PateintViewModel> GetPateintList();
        Task<int> AddUpdate(AppointmentViewModel model);

        List<AppointmentViewModel> DoctorEventsById(string DoctorId);
        List<AppointmentViewModel> PatientEventsById(string PatientId);
    }
}