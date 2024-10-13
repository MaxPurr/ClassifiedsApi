using System.Transactions;

namespace ClassifiedsApi.AppServices.Common.Services;

/// <summary>
/// Базовый сервис.
/// </summary>
public abstract class ServiceBase
{
    /// <summary>
    /// Метод для создания транзакции.
    /// </summary>
    /// <param name="isolationLevel">Уровень изоляции.</param>
    /// <returns>Транзакция.</returns>
    protected static TransactionScope CreateTransactionScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = isolationLevel
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}