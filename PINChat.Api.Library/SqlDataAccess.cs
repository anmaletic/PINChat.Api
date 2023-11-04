using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace PINChat.Api.Library;

public class SqlDataAccess : IDisposable, ISqlDataAccess
{
    private readonly IConfiguration _config;
    // private readonly ILogger<SqlDataAccess> _logger;     //   todo: implement serilog

    private IDbConnection? _connection;
    private bool _isClosed;
    private IDbTransaction? _transaction;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public void Dispose()
    {
        if (!_isClosed)
            try
            {
                CommitTransaction();
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Commit transaction failed in the dispose method.");
            }

        _transaction = null;
        _connection = null;
    }

    public string GetConnectionString(string name)
    {
        return _config.GetConnectionString(name);
    }

    public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
    {
        var connectionString = GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            var rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure)
                .ToList();

            return rows;
        }
    }

    public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
    {
        var connectionString = GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public void StartTransaction(string connectionStringName)
    {
        var connectionString = GetConnectionString(connectionStringName);

        _connection = new SqlConnection(connectionString);
        _connection.Open();

        _transaction = _connection.BeginTransaction();

        _isClosed = false;
    }

    public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
    {
        _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure,
            transaction: _transaction);
    }

    public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
    {
        var rows = _connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure,
            transaction: _transaction).ToList();

        return rows;
    }


    public void CommitTransaction()
    {
        _transaction?.Commit();
        _connection?.Close();

        _isClosed = true;
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _connection?.Close();

        _isClosed = true;
    }
}