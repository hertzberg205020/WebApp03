using System.Data.Entity;

namespace WebApp03.Repository;

public interface IDbContextHolder
{
    DbContext getCurrentCtx();  
}