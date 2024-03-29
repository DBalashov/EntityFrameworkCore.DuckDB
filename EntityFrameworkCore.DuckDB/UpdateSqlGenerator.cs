﻿using Microsoft.EntityFrameworkCore.Update;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBUpdateSqlGenerator(UpdateSqlGeneratorDependencies dependencies) : UpdateSqlGenerator(dependencies)
{
    public override string GenerateNextSequenceValueOperation(string name, string? schema) => throw new NotSupportedException("SequencesNotSupported");
}