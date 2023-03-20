using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Supplier
{
    [Route("api/Supplier")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;
        private readonly ISupplierItemService _supplierItemService;
        private readonly ISupplierItemCategoryService _supplierItemCategoryService;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly INumberRangeService _numberRangeService;

        public SupplierController(ISupplierService supplierService, IMapper mapper, ISupplierItemService supplierItemService, ISupplierItemCategoryService supplierItemCategoryService,
            UserManager<AppUser> userManager, INotificationService notificationService, INumberRangeService numberRangeService
            , IUserService userService)
        {
            _supplierService = supplierService;
            _supplierItemService = supplierItemService;
            _mapper = mapper;
            _supplierItemCategoryService = supplierItemCategoryService;
            _userManager = userManager;
            _notificationService = notificationService;
            _numberRangeService = numberRangeService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierCardDTO>>(await _supplierService.GetAllAsync()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllSuppliersForList()
        {           
            return Ok(_mapper.Map<IEnumerable<SupplierDTO>>(await _supplierService.GetAllForListAsync()));
        }
        [HttpPost("GetAssignedSupplierCodeSupplier")]
        public async Task<IActionResult> GetSupplierForDropDwonAssignAsync()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierDTO>>(await _supplierService.GetSupplierForDropDownAssignAsync()));
        }
        [HttpPost("GetUnAssignedSupplierCodeSupplier")]
        public async Task<IActionResult> GetAllDropDownAsync()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierDTO>>(await _supplierService.GetSupplierForDropDownAsync()));
        }
        [HttpGet("GetAllForapprovalStatus")]
        public async Task<IActionResult> GetAllForapprovalStatus()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierDTO>>(await _supplierService.GetAllForApproval()));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(SupplierDTO Model)
        {
            var list = await _supplierService.GetByIdAsync(Model.Id);

            var merge = _mapper.Map(Model, list.FirstOrDefault());
            merge.UserId = list.FirstOrDefault().UserId;

            return Ok(_mapper.Map<SupplierDTO>(await _supplierService.UpdateSupplierAsync(merge)));
        }
        [HttpPut("Reject")]
        public async Task<IActionResult> Reject(SupplierDTO Model)
        {
            var list = await _supplierService.GetByIdAsync(Model.Id);
            list.FirstOrDefault().Status = Enum.GetName(typeof(Status), Status.Rejected);
            return Ok(_mapper.Map<SupplierDTO>(await _supplierService.UpdateSupplierAsync(_mapper.Map<WebAPI.Models.Supplier>(list.FirstOrDefault()))));
        }
        [HttpPut("Approve")]
        public async Task<IActionResult> Approve(SupplierDTO Model)
        {
            var list = await _supplierService.GetByIdAsync(Model.Id);
            list.FirstOrDefault().Status = Enum.GetName(typeof(Status), Status.Active);
            return Ok(_mapper.Map<SupplierDTO>(await _supplierService.UpdateSupplierAsync(_mapper.Map<WebAPI.Models.Supplier>(list.FirstOrDefault()))));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> getBySupplierId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierDTO>>(await _supplierService.GetByIdAsync(Id)));
        }

        [HttpPut("{Id}/ToggleApprovalStatus")]
        public async Task<IActionResult> ToggleApprovalStatus(long Id)
        {
            List<NotificationReceiver> notificationReceivers = new();
            IEnumerable<Models.Supplier> list = await _supplierService.GetByIdAsync(Id);
            Models.Supplier supplier = list.FirstOrDefault();

            if (supplier.Status == Enum.GetName(typeof(Status), Status.Pending) || supplier.Status == Enum.GetName(typeof(Status), Status.Rejected))
            {
                supplier.Status = Enum.GetName(typeof(Status), Status.Processing);
                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var Admin = await _userService.GetUserByEmailAndCheck("admin@fougito.com" , "Admin");

                notificationReceivers.Add(new NotificationReceiver
                {
                    ReceiverId = Admin.FirstOrDefault().Id,
                    IsSeen = false,
                    IsDelivered = false,
                    IsRead = false,
                    ReceiverType = Enum.GetName(typeof(Logins), Logins.Supplier)
                });

                Notification notification = new()
                {
                    OriginatorId = UserId,
                    OriginatorName = supplier.NameAsPerTradeLicense,
                    Description = "New supplier listed for approval",
                    RecordId = supplier.Id,
                    OriginatorType = Enum.GetName(typeof(Logins), Logins.Supplier),
                    Url = $"Admin/Supplier/Approvals/{supplier.Id}",
                    NotificationReceivers = notificationReceivers
                };
                await _notificationService.AddNotification(notification);
            }

            else if (supplier.Status == Enum.GetName(typeof(Status), Status.Processing))
                supplier.Status = Enum.GetName(typeof(Status), Status.Pending);

            return Ok(_mapper.Map<SupplierDTO>(await _supplierService.UpdateSupplierAsync(supplier)));
        }

        [HttpPut("{Id}/ToggleActiveStatus")]
        public async Task<IActionResult> ToggleActiveStatus(long Id)
        {
            List<NotificationReceiver> notificationReceivers = new();
            IEnumerable<Models.Supplier> list = await _supplierService.GetByIdAsync(Id);
            Models.Supplier supplier = list.FirstOrDefault();
            AppUser users = await _userManager.FindByIdAsync(supplier.UserId);
            if (supplier.Status == Enum.GetName(typeof(Status), Status.Active) )
            {
                supplier.Status = Enum.GetName(typeof(Status), Status.Inactive);
                users.IsActive = false;
               await  _userManager.UpdateAsync(users);
                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var Admin = await _userManager.FindByEmailAsync("admin@fougito.com");

                notificationReceivers.Add(new NotificationReceiver
                {
                    ReceiverId = Admin.Id,
                    IsSeen = false,
                    IsDelivered = false,
                    IsRead = false,
                    ReceiverType = Enum.GetName(typeof(Logins), Logins.Supplier)
                });

                Notification notification = new()
                {
                    OriginatorId = UserId,
                    OriginatorName = supplier.NameAsPerTradeLicense,
                    Description = "Supplier Deactive Successfully...!",
                    RecordId = supplier.Id,
                    OriginatorType = Enum.GetName(typeof(Logins), Logins.Supplier),
                    Url = $"Admin/Supplier/Approvals/{supplier.Id}",
                    NotificationReceivers = notificationReceivers
                };
                await _notificationService.AddNotification(notification);
            }

            else if (supplier.Status == Enum.GetName(typeof(Status), Status.Inactive))
            {
                supplier.Status = Enum.GetName(typeof(Status), Status.Active);
                users.IsActive = true;
                await _userManager.UpdateAsync(users);
                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var Admin = await _userManager.FindByEmailAsync("admin@fougito.com");

                notificationReceivers.Add(new NotificationReceiver
                {
                    ReceiverId = Admin.Id,
                    IsSeen = false,
                    IsDelivered = false,
                    IsRead = false,
                    ReceiverType = Enum.GetName(typeof(Logins), Logins.Supplier)
                });

                Notification notification = new()
                {
                    OriginatorId = UserId,
                    OriginatorName = supplier.NameAsPerTradeLicense,
                    Description = "Supplier Active Successfully...!",
                    RecordId = supplier.Id,
                    OriginatorType = Enum.GetName(typeof(Logins), Logins.Supplier),
                    Url = $"Admin/Supplier/Approvals/{supplier.Id}",
                    NotificationReceivers = notificationReceivers
                };
                await _notificationService.AddNotification(notification);
            }

            return Ok(_mapper.Map<SupplierDTO>(await _supplierService.UpdateSupplierAsync(supplier)));
        }
        [HttpPost]
        public async Task<IActionResult> AddSupplier(SupplierDTO Model)
        {
            Model.SupplierCode = await _numberRangeService.GetNumberRangeByName("SUPPLIERORDER");
            var supplier = _mapper.Map<SupplierDTO>(await _supplierService.AddSupplierAsync(_mapper.Map<Models.Supplier>(Model)));
            return Ok(supplier);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<SupplierDTO>(await _supplierService.ArchiveSupplierAsync(Id)));
        }
    }
}
