using System.Data.Entity;

namespace WebApp03.Repository.Impl;

public class DbContextHolder: IDbContextHolder
{
    private DbContext _context;

    public DbContextHolder(DbContext ctx)
    {
        _context = ctx;
    }

    public DbContext getCurrentCtx() => _context ??= new ExamDbContext();
}