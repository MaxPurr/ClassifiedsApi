using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Accounts;
using ClassifiedsApi.AppServices.Helpers;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc/>
public class AccountRepository : IAccountRepository
{
    private readonly ISqlRepository<User, ApplicationDbContext> _repository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountRepository"/>.
    /// </summary>
    /// <param name="repository">Глуппый репозиторий <see cref="ISqlRepository{TEntity, TContext}"/>.</param>
    /// <param name="mapper">Маппер <see cref="IMapper"/>.</param>
    public AccountRepository(ISqlRepository<User, ApplicationDbContext> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    /// <inheritdoc/>
    public async Task<Guid> RegisterAsync(AccountRegister accountRegister, CancellationToken token)
    {
        var user = _mapper.Map<User>(accountRegister);
        await _repository.AddAsync(user, token);
        return user.Id;
    }

    public async Task<AccountInfo> GetInfoAsync(AccountVerify accountVerify, CancellationToken token)
    {
        var passwordHash = CryptoHelper.GetBase64Hash(accountVerify.Password);
        var accountInfo = await _repository.GetByPredicate(user => 
                user.Login == accountVerify.Login && 
                user.PasswordHash == passwordHash
            )
            .Include(user => user.Roles)
            .ProjectTo<AccountInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);
        if (accountInfo == null)
        {
            throw new IncorrectCredentialsException();
        }
        return accountInfo;
    }
}