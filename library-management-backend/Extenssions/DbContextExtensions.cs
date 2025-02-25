using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Extensions;

public static class DbContextExtensions
{
    public static async Task<int> IntFromRawSqlAsync(
        this DbContext dbContext,
        string sql,
        params object[] parameters
    )
    {
        if (string.IsNullOrEmpty(sql))
            throw new ArgumentNullException(nameof(sql));

        using (var command = dbContext.Database.GetDbConnection().CreateCommand())
        {
            // Ensure connection is open
            if (command.Connection.State != System.Data.ConnectionState.Open)
                await command.Connection.OpenAsync();

            try
            {
                command.CommandText = sql;
                if (parameters?.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                var result = await command.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                    return 0;

                // Attempt to convert the result to int
                return Convert.ToInt32(result);
            }
            finally
            {
                // Close connection if we opened it
                if (dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Open)
                    await command.Connection.CloseAsync();
            }
        }
    }
}
