using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBTypeMappingSource(TypeMappingSourceDependencies           dependencies,
                                     RelationalTypeMappingSourceDependencies relationalDependencies) : RelationalTypeMappingSource(dependencies, relationalDependencies)
{
    readonly Dictionary<Type, RelationalTypeMapping> clrTypeMappings = new()
                                                                       {
                                                                           [typeof(bool)] = new BoolTypeMapping("boolean"),

                                                                           [typeof(sbyte)] = new SByteTypeMapping("tinyint"),
                                                                           [typeof(byte)]  = new ByteTypeMapping("utinyint"),

                                                                           [typeof(short)]  = new ShortTypeMapping("smallint"),
                                                                           [typeof(ushort)] = new UShortTypeMapping("usmallint"),

                                                                           [typeof(int)]  = new IntTypeMapping("integer"),
                                                                           [typeof(uint)] = new UIntTypeMapping("uinteger"),

                                                                           [typeof(Int64)]  = new LongTypeMapping("bigint"),
                                                                           [typeof(UInt64)] = new ULongTypeMapping("ubigint"),

                                                                           [typeof(decimal)] = new DecimalTypeMapping("decimal"),
                                                                           [typeof(double)]  = new DoubleTypeMapping("double"),
                                                                           [typeof(float)]   = new FloatTypeMapping("real"),

                                                                           [typeof(DateTime)]       = new DuckDBDateTimeTypeMapping("timestamp"),
                                                                           [typeof(DateTimeOffset)] = new DuckDBDateTimeOffsetTypeMapping("timestampz"),
                                                                           [typeof(TimeOnly)]       = new TimeOnlyTypeMapping("time"),
                                                                           [typeof(TimeSpan)]       = new DuckDBTimeSpanTypeMapping("time"),

                                                                           [typeof(string)]   = new DuckDBStringTypeMapping("string"),
                                                                           [typeof(byte[])]   = new DuckDBByteArrayTypeMapping("blob"),
                                                                           [typeof(DateOnly)] = new DateOnlyTypeMapping("date"),

                                                                           // [typeof(Guid)] = new GuidTypeMapping("uuid"),
                                                                           // [typeof(Int128)] = new GuidTypeMapping("hugeint")
                                                                           // [typeof(UInt128)] = new GuidTypeMapping("uhugeint")
                                                                       };

    protected override RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        var mapping = base.FindMapping(mappingInfo) ?? findRawMapping(mappingInfo);
        return mapping != null && mappingInfo.StoreTypeName != null
                   ? mapping.Clone(mappingInfo)
                   : mapping;
    }

    RelationalTypeMapping? findRawMapping(RelationalTypeMappingInfo mappingInfo)
    {
        var clrType = mappingInfo.ClrType;
        return clrType != null && clrTypeMappings.TryGetValue(clrType, out var mapping) ? mapping : null;
    }
}