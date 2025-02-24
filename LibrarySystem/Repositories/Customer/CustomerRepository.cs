
using System.Security.Cryptography.X509Certificates;
using LibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories.Customer
{
    public class CustomerRepository(AppDbContext context) : ICustomerRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Models.Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Models.Customer>> GetCustomersAsync(int page, int pageSize)
        {
            int skipCount = (page - 1) * pageSize;

            return await _context.Customers
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<Models.Customer> CreateAsync(Models.Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Models.Customer> UpdateAsync(Models.Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
