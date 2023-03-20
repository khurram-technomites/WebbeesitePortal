using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartAppointmentManagementRepo : Repository<SparePartAppointmentManagement>, ISparePartAppointmentManagementRepo
    {
        public SparePartAppointmentManagementRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
