using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageAppointmentManagementRepo:Repository<GarageAppointmentManagement>, IGarageAppointmentManagementRepo
    {
        public GarageAppointmentManagementRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
