using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DutchTreat.Data;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected ProductContext RepositoryContext { get; set; }
 
    public RepositoryBase(ProductContext repositoryContext)
    {
        this.RepositoryContext = repositoryContext;
    }
 
    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await this.RepositoryContext.Set<T>().ToListAsync();
    }
 
    public async Task<IEnumerable<T>> FindByConditionAync(Expression<Func<T, bool>> expression)
    {
        return await this.RepositoryContext.Set<T>().Where(expression).ToListAsync();
    }
 
    public void Create(T entity)
    {
        this.RepositoryContext.Set<T>().Add(entity);
    }
 
    public void Update(T entity)
    {
        this.RepositoryContext.Set<T>().Update(entity);
    }
 
    public void Delete(T entity)
    {
        this.RepositoryContext.Set<T>().Remove(entity);
    }
 
    public async Task SaveAsync()
    {
        await this.RepositoryContext.SaveChangesAsync();
    }
}