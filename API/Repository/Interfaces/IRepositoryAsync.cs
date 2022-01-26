﻿using System.Data;
using System.Linq.Expressions;
using API.Db.Entity.Entity.Interface;
using API.Wrapper;

namespace API.Repository;

public interface IRepositoryAsync
{
    /// <summary>
    /// Get a <see cref="List{T}"/> based on the criteria specified in the parameters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="condition">The condition on which the returned <see cref="List{T}"/> will be filtered.</param>
    /// <param name="orderBy">The <see cref="Func{Q,OQ}"/> of <see cref="IQueryable{T}"/>, <see cref="IOrderedQueryable{T}"/> to order the returned <see cref="List{T}"/> by.</param>
    /// <param name="includes">The <see cref="Expression"/> for navigation properties to be included.</param>
    /// <param name="asNoTracking">The <see cref="bool"/> value which determines whether the returned entity will be tracked by
    /// EF Core context or not. Default value is true i.e tracking is disabled by default.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="List{T}"/>.</returns>
    Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>>? condition = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Expression<Func<T, object>>[]? includes = null, bool asNoTracking = true, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <see cref="List{T}"/> based on the criteria specified in <see cref="BaseSpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="specification">The <see cref="BaseSpecification{T}"/> <see cref="object"/> which contains all the criteria
    /// on which data will be returned.</param>
    /// <param name="asNoTracking">The <see cref="bool"/> value which determines whether the returned entity will be tracked by
    /// EF Core context or not. Default value is true i.e tracking is disabled by default.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="List{T}"/>.</returns>
    Task<List<T>> GetListAsync<T>(BaseSpecification<T>? specification = null, bool asNoTracking = true, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <see cref="List{TProjectedType}"/> based on the criteria specified in the parameters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TProjectedType">The type to which <typeparamref name="T"/> will be projected.</typeparam>
    /// <param name="condition">The condition on which the returned <see cref="List{TProjectedType}"/> will be filtered.</param>
    /// <param name="selectExpression">The <see cref="Expression{Func}"/> to select properties for projection.</param>
    /// <param name="orderBy">The <see cref="Func{Q,OQ}"/> of <see cref="IQueryable{T}"/>, <see cref="IOrderedQueryable{T}"/> to order the returned <see cref="List{T}"/> by.</param>
    /// <param name="includes">The <see cref="Expression{Func}"/>s for navigation properties to be included.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="List{TProjectedType}"/>.</returns>
    Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(Expression<Func<T, bool>>? condition = null, Expression<Func<T, TProjectedType>>? selectExpression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <see cref="List{TProjectedType}"/> based on the criteria specified in <see cref="BaseSpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TProjectedType">The type to which <typeparamref name="T"/> will be projected.</typeparam>
    /// <param name="selectExpression">The <see cref="Expression{Func}"/> to select properties for projection.</param>
    /// <param name="specification">The <see cref="BaseSpecification{T}"/> <see cref="object"/> which contains all the criteria
    /// on which data will be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="List{TProjectedType}"/>.</returns>
    Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(Expression<Func<T, TProjectedType>> selectExpression, BaseSpecification<T>? specification = null, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <see cref="PaginatedResult{T}"/> based on the criteria specified in <see cref="PaginationSpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TDto">The type to which <typeparamref name="T"/> will be mapped.</typeparam>
    /// <param name="specification">The <see cref="PaginationSpecification{T}"/> <see cref="object"/> which contains all the criteria
    /// on which data will be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="PaginatedResult{TDto}"/>.</returns>
    Task<PaginatedResult<TDto>> GetListAsync<T, TDto>(PaginationSpecification<T> specification, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="T"/> based on the <paramref name="entityId"/> which is the primary key value of the entity
    /// if found otherwise <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entityId">The primary key value of the entity to be returned.</param>
    /// <param name="includes">The <see cref="Expression{Func}"/> for navigation properties to be included.</param>
    /// <param name="asNoTracking">The <see cref="bool"/> value which determines whether the returned entity will be tracked by
    /// EF Core context or not. Default value is false i.e tracking is enabled by default.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
    Task<T?> GetByIdAsync<T>(int entityId, Expression<Func<T, object>>[]? includes = null, bool asNoTracking = false, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="TProjectedType"/> based on the <paramref name="entityId"/> which is the primary key value of the entity
    /// if found otherwise <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TProjectedType">The type to which <typeparamref name="T"/> will be projected.</typeparam>
    /// <param name="entityId">The primary key value of the entity to be returned.</param>
    /// <param name="selectExpression">The <see cref="Expression{Func}"/> to select properties for projection.</param>
    /// <param name="includes">The <see cref="Expression{Func}"/> for navigation properties to be included.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="TProjectedType"/>.</returns>
    Task<TProjectedType?> GetByIdAsync<T, TProjectedType>(int entityId, Expression<Func<T, TProjectedType>> selectExpression, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="TDto"/> based on the <paramref name="entityId"/> which is the primary key value of the entity
    /// if found otherwise <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TDto">The type to which <typeparamref name="T"/> will be mapped.</typeparam>
    /// <param name="entityId">The primary key value of the entity to be returned.</param>
    /// <param name="includes">The <see cref="Expression{Func}"/> for navigation properties to be included.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="TDto"/>.</returns>
    Task<TDto> GetByIdAsync<T, TDto>(int entityId, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="T"/> based on the <paramref name="condition"/>
    /// if found otherwise <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="condition">The condition on which the returned <typeparamref name="T"/> will be filtered.</param>
    /// <param name="includes">The <see cref="Expression{Func}"/> for navigation properties to be included.</param>
    /// <param name="asNoTracking">The <see cref="bool"/> value which determines whether the returned entity will be tracked by
    /// EF Core context or not. Default value is false i.e tracking is enabled by default.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
    Task<T?> GetAsync<T>(Expression<Func<T, bool>>? condition = null, Expression<Func<T, object>>[]? includes = null, bool asNoTracking = false, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="T"/> based on the criteria specified in <see cref="BaseSpecification{T}"/>
    /// if found otherwise <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="specification">The <see cref="BaseSpecification{T}"/> <see cref="object"/> which contains all the criteria
    /// on which data will be returned.</param>
    /// <param name="asNoTracking">The <see cref="bool"/> value which determines whether the returned entity will be tracked by
    /// EF Core context or not. Default value is false i.e tracking is enabled by default.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
    Task<T?> GetAsync<T>(BaseSpecification<T>? specification = null, bool asNoTracking = false, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="TProjectedType"/> based on the criteria specified in <paramref name="condition"/>
    /// if found otherwise <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TProjectedType">The type to which <typeparamref name="T"/> will be projected.</typeparam>
    /// <param name="selectExpression">The <see cref="Expression{Func}"/> to select properties for projection.</param>
    /// <param name="condition">The condition on which the returned <typeparamref name="TProjectedType"/> will be filtered.</param>
    /// <param name="includes">The <see cref="Expression{Func}"/> for navigation properties to be included.</param>
    /// <param name="asNoTracking">The <see cref="bool"/> value which determines whether the returned entity will be tracked by
    /// EF Core context or not. Default value is true i.e tracking is disabled by default.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="TProjectedType"/>.</returns>
    Task<TProjectedType?> GetAsync<T, TProjectedType>(Expression<Func<T, TProjectedType>> selectExpression, Expression<Func<T, bool>>? condition = null, Expression<Func<T, object>>[]? includes = null, bool asNoTracking = true, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="TProjectedType"/> based on the criteria specified in <see cref="BaseSpecification{T}"/>
    /// if found otherwise <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TProjectedType">The type to which <typeparamref name="T"/> will be projected.</typeparam>
    /// <param name="selectExpression">The <see cref="Expression{Func}"/> to select properties for projection.</param>
    /// <param name="specification">The <see cref="BaseSpecification{T}"/> <see cref="object"/> which contains all the criteria
    /// on which data will be returned.</param>
    /// <param name="asNoTracking">The <see cref="bool"/> value which determines whether the returned entity will be tracked by
    /// EF Core context or not. Default value is true i.e tracking is disabled by default.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="TProjectedType"/>.</returns>
    Task<TProjectedType?> GetAsync<T, TProjectedType>(Expression<Func<T, TProjectedType>> selectExpression, BaseSpecification<T>? specification = null, bool asNoTracking = true, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get the count of <typeparamref name="T"/> based on the criteria specified in <paramref name="condition"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="condition">The condition on which the returned count of <typeparamref name="T"/> will be filtered.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
    Task<int> GetCountAsync<T>(Expression<Func<T, bool>>? condition = null, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Determine whether <typeparamref name="T"/> exists based on the criteria specified in <paramref name="condition"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="condition">The condition on which the existence of <typeparamref name="T"/> will be determined.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="bool"/>.</returns>
    Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Add a single <typeparamref name="T"/> to the EF Core context to be persisted in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be inserted.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
    Task<int> CreateAsync<T>(T entity, CancellationToken cancellationToken = default)
    where T : class, IBaseEntity, ISoftDelete;

    /// <summary>
    /// Add an <see cref="IEnumerable{T}"/> to the EF Core context to be persisted in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <param name="entities">The entities to be inserted.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see><cref>IList{int}</cref></see>.</returns>
    Task<IList<int>> CreateRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;

    /// <summary>
    /// Update a single <typeparamref name="T"/> in the EF Core context to be persisted in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be updated.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/>.</returns>
    Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;
    
    /// <summary>
    /// Update an <see cref="IEnumerable{T}"/> in the EF Core context to be persisted in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <param name="entities">The entities to be updated.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/>.</returns>
    Task UpdateRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;

    /// <summary>
    /// Remove a single <typeparamref name="T"/> from the EF Core context to be persisted in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be removed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/>.</returns>
    Task RemoveAsync<T>(T entity, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;

    /// <summary>
    /// Remove an <see cref="IEnumerable{T}"/> from the EF Core context to be persisted in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <param name="entities">The entities to be removed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/>.</returns>
    Task RemoveRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;

    /// <summary>
    /// Remove a single <typeparamref name="T"/> based on the <paramref name="entityId"/> from the EF Core context to be persisted in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <param name="entityId">The primary key value of the entity to be removed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
    Task<T> RemoveByIdAsync<T>(int entityId, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;

    Task ClearAsync<T>(Expression<Func<T, bool>>? expression = null, BaseSpecification<T>? specification = null, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;

    /// <summary>
    /// Persist changes made to the EF Core context in the database.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get an <see cref="IReadOnlyList{T}"/> using raw sql string with parameters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="sql">The sql string.</param>
    /// <param name="param">The paramters in the sql string.</param>
    /// <param name="transaction">The transaction to be performed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="IReadOnlyCollection{T}"/>.</returns>
    Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="T"/> using raw sql string with parameters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="sql">The sql string.</param>
    /// <param name="param">The paramters in the sql string.</param>
    /// <param name="transaction">The transaction to be performed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
    Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;

    /// <summary>
    /// Get a <typeparamref name="T"/> using raw sql string with parameters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="sql">The sql string.</param>
    /// <param name="param">The paramters in the sql string.</param>
    /// <param name="transaction">The transaction to be performed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
    Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class,  IBaseEntity, ISoftDelete;
}