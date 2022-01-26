using System.Data;
using System.Linq.Expressions;
using API.Db;
using API.Db.Entity.Entity.Interface;
using API.Utils;
using API.Wrapper;
using Dapper;
using DN.WebApi.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace API.Repository;

public class RepositoryAsync : IRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<RepositoryAsync> _localizer;

    public RepositoryAsync(ApplicationDbContext dbContext, IStringLocalizer<RepositoryAsync> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;
    }

    // Get List

    public Task<List<T>> GetListAsync<T>(
        Expression<Func<T, bool>>? condition = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T, object>>[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        Filter(condition, orderBy, includes, asNoTracking: asNoTracking)
            .ToListAsync(cancellationToken);

    public Task<List<T>> GetListAsync<T>(
        BaseSpecification<T>? specification = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
        where T : class, IBaseEntity, ISoftDelete =>
        Filter(specification: specification, asNoTracking: asNoTracking)
            .ToListAsync(cancellationToken);

    public Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(
        Expression<Func<T, bool>>? condition = null,
        Expression<Func<T, TProjectedType>>? selectExpression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T, object>>[]? includes = null,
        CancellationToken cancellationToken = default)
        where T : class, IBaseEntity, ISoftDelete =>
        selectExpression == null
            ? throw new ArgumentNullException(nameof(selectExpression))
            : Filter(condition, orderBy, includes, asNoTracking: true)
                .Select(selectExpression)
                .ToListAsync(cancellationToken);

    public Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(
        Expression<Func<T, TProjectedType>> selectExpression,
        BaseSpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
        where T : class, IBaseEntity, ISoftDelete =>
        selectExpression == null
            ? throw new ArgumentNullException(nameof(selectExpression))
            : Filter(specification: specification, asNoTracking: true)
                .Select(selectExpression)
                .ToListAsync(cancellationToken);

    public Task<PaginatedResult<TDto>> GetListAsync<T, TDto>(
        PaginationSpecification<T> specification,
        CancellationToken cancellationToken = default)
        where T : class, IBaseEntity, ISoftDelete
    {
        if (specification == null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        if (specification.PageIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(specification.PageIndex),
                $"The value of {nameof(specification.PageIndex)} must be greater than or equal to 0.");
        }

        if (specification.PageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(specification.PageSize),
                $"The value of {nameof(specification.PageSize)} must be greater than 0.");
        }

        IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();

        if (specification.Filters is not null)
        {
            query = query.ApplyFilter(specification.Filters);
        }

        if (specification.AdvancedSearch?.Fields.Count > 0 &&
            !string.IsNullOrWhiteSpace(specification.AdvancedSearch.Keyword))
        {
            query = query.AdvancedSearch(specification.AdvancedSearch);
        }
        else if (!string.IsNullOrWhiteSpace(specification.Keyword))
        {
            query = query.SearchByKeyword(specification.Keyword);
        }

        query = query.Specify(specification);

        return query.ToMappedPaginatedResultAsync<T, TDto>(specification.PageIndex, specification.PageSize,
            cancellationToken);
    }

    // Get One By Id

    public Task<T?> GetByIdAsync<T>(
        int entityId,
        Expression<Func<T, object>>[]? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        Filter(e => e.Id == entityId, includes: includes, asNoTracking: asNoTracking)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<TProjectedType?> GetByIdAsync<T, TProjectedType>(
        int entityId,
        Expression<Func<T, TProjectedType>> selectExpression,
        Expression<Func<T, object>>[]? includes = null,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        selectExpression == null
            ? throw new ArgumentNullException(nameof(selectExpression))
            : Filter(e => e.Id == entityId, includes: includes, asNoTracking: true)
                .Select(selectExpression)
                .FirstOrDefaultAsync(cancellationToken);

    public async Task<TDto> GetByIdAsync<T, TDto>(
        int entityId,
        Expression<Func<T, object>>[]? includes = null,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete
    {
        async Task<TDto?> GetDto() =>
            await GetByIdAsync(entityId, includes, true, cancellationToken) is object entity
                ? AutoMapperUtils.BasicAutoMapper<T, TDto>((T)entity)
                : default;

        // Only get from cache when no includes defined
        var dto = await GetDto();

        return dto is null
            ? throw new EntityNotFoundException(string.Format(_localizer["entity.notfound"], typeof(T).Name, entityId))
            : dto;
    }

    // Get One By Condition

    public Task<T?> GetAsync<T>(
        Expression<Func<T, bool>>? condition = null,
        Expression<Func<T, object>>[]? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        Filter(condition, includes: includes, asNoTracking: asNoTracking)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<T?> GetAsync<T>(
        BaseSpecification<T>? specification = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        Filter(specification: specification, asNoTracking: asNoTracking)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<TProjectedType?> GetAsync<T, TProjectedType>(
        Expression<Func<T, TProjectedType>> selectExpression,
        Expression<Func<T, bool>>? condition = null,
        Expression<Func<T, object>>[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        selectExpression == null
            ? throw new ArgumentNullException(nameof(selectExpression))
            : Filter(condition, includes: includes, asNoTracking: asNoTracking)
                .Select(selectExpression)
                .FirstOrDefaultAsync(cancellationToken);

    public Task<TProjectedType?> GetAsync<T, TProjectedType>(
        Expression<Func<T, TProjectedType>> selectExpression,
        BaseSpecification<T>? specification = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        selectExpression == null
            ? throw new ArgumentNullException(nameof(selectExpression))
            : Filter(specification: specification, asNoTracking: asNoTracking)
                .Select(selectExpression)
                .FirstOrDefaultAsync(cancellationToken);

    // Get Count

    public Task<int> GetCountAsync<T>(
        Expression<Func<T, bool>>? condition = null,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        Filter(condition)
            .CountAsync(cancellationToken);

    // Check if Exists

    public Task<bool> ExistsAsync<T>(
        Expression<Func<T, bool>> condition,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        Filter(condition)
            .AnyAsync(cancellationToken);

    // Filter

    private IQueryable<T> Filter<T>(
        Expression<Func<T, bool>>? condition = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T, object>>[]? includes = null,
        BaseSpecification<T>? specification = null,
        bool asNoTracking = false)
        where T : class, IBaseEntity, ISoftDelete
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (condition is not null)
        {
            query = query.Where(condition);
        }

        if (specification is not null)
        {
            query = query.Specify(specification);
        }

        if (includes is not null)
        {
            query = query.IncludeMultiple(includes);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return query;
    }

    // Create (these won't work with database generated id's if we were to suport that...)

    public async Task<int> CreateAsync<T>(T entity, CancellationToken cancellationToken = default)
        where T : class, IBaseEntity, ISoftDelete
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async Task<IList<int>> CreateRangeAsync<T>(IEnumerable<T> entities,
        CancellationToken cancellationToken = default)
        where T : class, IBaseEntity, ISoftDelete
    {
        var baseEntities = entities.ToList();
        await _dbContext.Set<T>().AddRangeAsync(baseEntities, cancellationToken);
        return baseEntities.Select(x => x.Id).ToList();
    }

    // Update

    public Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged)
        {
            throw new NothingToUpdateException();
        }

        var existing = _dbContext.Set<T>().Find(entity.Id);

        _ = existing ??
            throw new EntityNotFoundException(string.Format(_localizer["entity.notfound"], typeof(T).Name, entity.Id));

        _dbContext.Entry(existing).CurrentValues.SetValues(entity);
        return Task.CompletedTask;
    }

    public async Task UpdateRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete
    {
        foreach (var entity in entities)
        {
            if (_dbContext.Entry(entity).State == EntityState.Unchanged)
            {
                throw new NothingToUpdateException();
            }

            T? existing = await _dbContext.Set<T>().FindAsync(entity.Id);

            _ = existing ??
                throw new EntityNotFoundException(string.Format(_localizer["entity.notfound"], typeof(T).Name,
                    entity.Id));

            _dbContext.Entry(existing).CurrentValues.SetValues(entity);
            await Task.CompletedTask;
        }
    }

    // Delete

    public Task RemoveAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<T> RemoveByIdAsync<T>(int entityId, CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete
    {
        T? entity = await _dbContext.Set<T>()
            .FindAsync(new object?[] { entityId }, cancellationToken: cancellationToken);
        _ = entity ??
            throw new EntityNotFoundException(string.Format(_localizer["entity.notfound"], typeof(T).Name, entityId));

        _dbContext.Set<T>().Remove(entity);
        return entity;
    }

    public Task RemoveRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete
    {
        foreach (T entity in entities)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        return Task.CompletedTask;
    }

    public Task ClearAsync<T>(
        Expression<Func<T, bool>>? condition = null,
        BaseSpecification<T>? specification = null,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        Filter(condition, specification: specification)
            .ForEachAsync(
                x =>
                {
                    _dbContext.Entry(x).State = EntityState.Deleted;
                },
                cancellationToken: cancellationToken);

    // SaveChanges

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SaveChangesAsync(cancellationToken);

    // Dapper

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null,
        IDbTransaction? transaction = null, CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete =>
        (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
        .AsList();

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null,
        IDbTransaction? transaction = null, CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete
    {
        T? entity = await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);

        return entity ?? throw new EntityNotFoundException(string.Empty);
    }

    public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default) where T : class, IBaseEntity, ISoftDelete
    {
        return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}
