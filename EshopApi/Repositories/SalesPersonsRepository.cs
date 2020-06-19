using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EshopApi.Contracts;
using EshopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EshopApi.Repositories
{
    public class SalesPersonsRepository:ISalesPersonsRepository
    {
        private EshopApi_DBContext _context;

        public SalesPersonsRepository(EshopApi_DBContext context)
        {
            _context = context;
        }

        public IEnumerable<SalesPersons> GetAll()
        {
            return _context.SalesPersons.ToList();
        }

        public async Task<SalesPersons> Add(SalesPersons sales)
        {
            await _context.SalesPersons.AddAsync(sales);
            await _context.SaveChangesAsync();
            return sales;
        }

        public async Task<SalesPersons> Find(int id)
        {
            return await _context.SalesPersons.SingleOrDefaultAsync(s => s.SalesPersonId == id);

        }

        public async Task<SalesPersons> Update(SalesPersons sales)
        {
            _context.Update(sales);
            await _context.SaveChangesAsync();
            return sales;
        }

        public async Task<SalesPersons> Remove(int id)
        {
            var sales = await _context.SalesPersons.SingleAsync(s => s.SalesPersonId == id);
            _context.SalesPersons.Remove(sales);
            await _context.SaveChangesAsync();
            return sales;
        }

        public async Task<bool> IsExists(int id)
        {
            return await _context.SalesPersons.AnyAsync(s => s.SalesPersonId == id);
        }
    }
}
