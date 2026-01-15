using mas.Data;
using mas.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace mas.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public IRepository<Product> Products { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Review> Reviews { get; }
    public IRepository<Order> Orders { get; }
    public IRepository<OrderItem> OrderItems { get; }
    public IRepository<Cart> Carts { get; }
    public IRepository<CartItem> CartItems { get; }
    public IRepository<Coupon> Coupons { get; }
    public IRepository<Notification> Notifications { get; }
    public IRepository<ContactMessage> ContactMessages { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Products = new Repository<Product>(context);
        Categories = new Repository<Category>(context);
        Reviews = new Repository<Review>(context);
        Orders = new Repository<Order>(context);
        OrderItems = new Repository<OrderItem>(context);
        Carts = new Repository<Cart>(context);
        CartItems = new Repository<CartItem>(context);
        Coupons = new Repository<Coupon>(context);
        Notifications = new Repository<Notification>(context);
        ContactMessages = new Repository<ContactMessage>(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
