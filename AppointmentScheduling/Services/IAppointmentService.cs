using AppointmentScheduling.Models.ViewModel;
using System.Collections.Generic;

namespace AppointmentScheduling.Services
{
    public interface IAppointmentService
    {
        List<DoctorViewModel> GetDoctorList();
        List<PateintViewModel> GetPateintList();
    }
}