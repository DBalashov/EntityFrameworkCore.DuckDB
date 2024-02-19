create table test1
(
    test_bigint_null    bigint,
    test_bigint_nn      bigint         not null,

    test_blob_null      blob,
    test_blob_nn        blob           not null,

    test_boolean_null   boolean,
    test_boolean_nn     boolean        not null,

    test_date_null      date,
    test_date_nn        date           not null,

    test_decimal_null   decimal(18, 6),
    test_decimal_nn     decimal(18, 6) not null,
    test_double_null double,
    test_double_nn double not null,
    test_real_null real,
    test_real_nn real not null,

    test_integer_null   integer,
    test_integer_nn     integer        not null,
    test_smallint_null  smallint,
    test_smallint_nn    smallint       not null,

    test_time_null      time,
    test_time_nn        time           not null,

    test_timespan_null      time,
    test_timespan_nn        time           not null,

    test_timestamp_null timestamp,
    test_timestamp_nn   timestamp      not null,

    test_tinyint_null   tinyint,
    test_tinyint_nn     tinyint        not null,

    test_ubigint_null   ubigint,
    test_ubigint_nn     ubigint        not null,

    test_uinteger_null  uinteger,
    test_uinteger_nn    uinteger       not null,

    test_usmallint_null usmallint,
    test_usmallint_nn   usmallint      not null,

    test_utinyint_null  utinyint,
    test_utinyint_nn    utinyint       not null,

    test_uuid_null      uuid,
    test_uuid_nn        uuid           not null,

    test_varchar_null   varchar,
    test_varchar_nn     varchar        not null,
);

-- wuthout NULLs
insert into test1
values (1, 2,
        'HELLO'::blob, 'TEST'::blob,
        true, false,
        '2020-02-03', '2021-04-05',
        1.2, 1.3, 1.4, 1.5, 1.6, 1.7,
        12345678, 23456789, -1234, 5678,
        '12:34:56', '23:45:56', '12:34:56', '23:45:56',
        '2020-02-03 12:34:56', '2021-04-05 23:45:56',
        12, 34,
        1234567890123456789, 9876543210987654321,
        1234567890, 76543210,
        1234, 4567,
        123, 234,
        'abcdeffe-aabb-ccdd-eeff-a1b2c3d4e5f6', 'badcfeff-aabb-ccdd-eeff-a1b2c3d4e5f6',
        'string 1', 'string 2');

-- with NULLs
insert into test1
values (null, 2,
        null, 'Another test'::blob,
        null, false,    
        null, '2021-04-05',
        null, 1.3, null, 1.5, null, 1.7,
        null, 23456789, null, 5678,
        null, '23:45:56', null, '23:45:56',
        null, '2021-04-05 23:45:56',
        null, 34,
        null, 9876543210987654321,
        null, 76543210,
        null, 4567,
        null, 234,
        null, 'badcfeff-aabb-ccdd-eeff-a1b2c3d4e5f6',
        null, 'string 2');
        
        