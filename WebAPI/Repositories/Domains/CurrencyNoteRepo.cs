﻿using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class CurrencyNoteRepo:Repository<CurrencyNote>, ICurrencyNoteRepo
    {
        public CurrencyNoteRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
