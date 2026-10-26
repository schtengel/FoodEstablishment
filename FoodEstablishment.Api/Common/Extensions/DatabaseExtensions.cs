using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FoodEstablishment.Api.Common;

public static class DatabaseExceptions
{
    public static bool IsUniqueViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current != null; current = current.InnerException)
        {
            if (current is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
                return true;
        }

        return false;
    }
}