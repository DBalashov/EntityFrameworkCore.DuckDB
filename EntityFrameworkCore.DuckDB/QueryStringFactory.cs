using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBQueryStringFactory(IRelationalTypeMappingSource typeMapper) : IRelationalQueryStringFactory
{
    public string Create(DbCommand command)
    {
        if (command.Parameters.Count == 0)
            return command.CommandText;

        var builder = new StringBuilder();
        foreach (DbParameter parameter in command.Parameters) // taken as example from sqlite provider
        {
            var value = parameter.Value;
            builder.Append(".param set ")
                   .Append(parameter.ParameterName)
                   .Append(' ')
                   .AppendLine(value == null || value == DBNull.Value
                                   ? "NULL"
                                   : typeMapper.FindMapping(value.GetType())?.GenerateSqlLiteral(value)
                                  ?? value.ToString());
        }

        return builder.AppendLine()
                      .Append(command.CommandText).ToString();
    }
}