namespace MovieRama.Data;

using System.Transactions;

/// <summary>
/// 
/// </summary>
public static class Transactions
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions {
                Timeout = TransactionManager.MaximumTimeout,
                IsolationLevel = IsolationLevel.ReadCommitted
            }, TransactionScopeAsyncFlowOption.Enabled);
    }
}