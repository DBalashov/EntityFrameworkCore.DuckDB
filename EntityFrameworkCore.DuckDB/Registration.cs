using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.DuckDB.Provider;

public static class UseExtension
{
    public static DbContextOptionsBuilder UseDuckDB(this DbContextOptionsBuilder optionsBuilder,
                                                    string?                      connectionString = null)
    {
        var extension = (DuckDBOptionsExtension) GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
        ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);
        return optionsBuilder;
    }

    static DuckDBOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder options) => options.Options.FindExtension<DuckDBOptionsExtension>() ?? new DuckDBOptionsExtension();

    public static IServiceCollection AddEntityFrameworkDuckDBProvider(this IServiceCollection serviceCollection)
    {
        var builder = new EntityFrameworkRelationalServicesBuilder(serviceCollection)
                     .TryAdd<LoggingDefinitions, DuckDBLoggingDefinitions>()
                     .TryAdd<IDatabaseProvider, DatabaseProvider<DuckDBOptionsExtension>>()
                     .TryAdd<IRelationalTypeMappingSource, DuckDBTypeMappingSource>()
                     .TryAdd<ISqlGenerationHelper, DuckDBSqlGenerationHelper>()
                     .TryAdd<IRelationalAnnotationProvider, DuckDBAnnotationProvider>()
                     .TryAdd<IModelValidator, DuckDBModelValidator>()
                     .TryAdd<IProviderConventionSetBuilder, DuckDBConventionSetBuilder>()
                     .TryAdd<IModificationCommandBatchFactory, DuckDBModificationCommandBatchFactory>()
                     .TryAdd<IRelationalConnection, DuckDBRelationalConnection>()
                     .TryAdd<IRelationalQueryStringFactory, DuckDBQueryStringFactory>()
                     .TryAdd<IMethodCallTranslatorProvider, DuckDBMethodCallTranslatorProvider>()
                     .TryAdd<IAggregateMethodCallTranslatorProvider, DuckDBAggregateMethodCallTranslatorProvider>()
                     .TryAdd<IMemberTranslatorProvider, DuckDBMemberTranslatorProvider>()
                     .TryAdd<IQuerySqlGeneratorFactory, DuckDBQuerySqlGeneratorFactory>()
                     .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, DuckDBQueryableMethodTranslatingExpressionVisitorFactory>()
                     .TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, DuckDBSqlTranslatingExpressionVisitorFactory>()
                     .TryAdd<IQueryTranslationPostprocessorFactory, DuckDBQueryTranslationPostprocessorFactory>()
                     .TryAdd<IUpdateSqlGenerator, DuckDBUpdateSqlGenerator>();
                     //.TryAdd<ISqlExpressionFactory, DuckDBSqlExpressionFactory>();

        // .TryAdd<IMigrationsSqlGenerator, YdbMigrationsSqlGenerator>()
        // .TryAdd<IRelationalDatabaseCreator, YdbDatabaseCreator>()
        // .TryAdd<IHistoryRepository, YdbHistoryRepository>()

        builder.TryAddCoreServices();

        return serviceCollection;
    }
}