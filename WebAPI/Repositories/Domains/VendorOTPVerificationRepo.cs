﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class VendorOTPVerificationRepo : Repository<VendorOTPVerification>, IVendorOTPVerificationRepo
    {
        public VendorOTPVerificationRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
