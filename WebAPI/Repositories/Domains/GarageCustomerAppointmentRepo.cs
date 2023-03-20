using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageCustomerAppointmentRepo:Repository<GarageCustomerAppointment>, IGarageCustomerAppointmentRepo
    {
        public GarageCustomerAppointmentRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
