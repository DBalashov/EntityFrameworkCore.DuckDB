## Contents

* [DateTime / DateTimeOffset](#systemdatetime--systemdatetimeoffset)
* [TimeSpan](#systemtimespan)
* [TimeOnly](#systemtimeonly)
* [Math (decimal/double/float)](#systemmath-decimaldoublefloat)
* [Random](#systemrandom)
* [String](#systemstring)
* [Types: double / float](#types-double--float)
* [Guid](#systemguid)

## Supported types, methods & properties mapping

#### System.DateTime / System.DateTimeOffset

| Methods         | Properties  |
|-----------------|-------------|
| AddYears        | Now         |
| AddMonths       | UtcNow      |
| AddDays         |             |
| AddHours        | DayOfYear   |
| AddMinutes      | DayOfWeek   |
| AddSeconds      | Year        |
| AddMilliseconds | Month       |
| AddMicroseconds | Day         |
|                 | Hour        |
|                 | Minute      |
|                 | Second      |
|                 | Millisecond |
|                 | Microsecond |

#### System.TimeSpan

| Methods | Properties   |
|---------|--------------|
|         | Days         |
|         | Hours        |
|         | Minutes      |
|         | Seconds      |
|         | Milliseconds |
|         | Microseconds |

#### System.TimeOnly

| Methods    | Properties  |
|------------|-------------|
| AddHours   | Hour        |
| AddMinutes | Minute      |
|            | Second      |
|            | Millisecond |
|            | Microsecond |

#### System.Math (decimal/double/float)

| Methods    | Properties |
|------------|------------|
| Abs        |            |
| Acos       |            |    
| Asin       |            |    
| Atan       |            |    
| Atan2      |            |   
| Ceiling    |            |
| Cos        |            |     
| Exp        |            |     
| Floor      |            |   
| Log2       |            |    
| Log10      |            |   
| Log        |            |     
| Pow        |            |     
| Round      |            |   
| Sign       |            |    
| Sin        |            |     
| Sqrt       |            |    
| Tan        |            |     
| Truncate   |            |
| IsFinite   |            |
| IsInfinity |            |
| IsNaN      |            |

#### Types: double / float

| Methods    | Properties |
|------------|------------|
| IsFinite   |            |
| IsInfinity |            |
| IsNaN      |            |

#### System.Random~~~~

* Next()
* Next(int max)
* Next(int min, int max)
* NextDouble()

#### System.String

| Methods                       | Properties |
|-------------------------------|------------|
| StartsWith(string)            | Length     |
| EndsWith(string)              |            |
| IndexOf(string)               |            |
| Substring(startIndex)         |            |
| Substring(startIndex, length) |            |
| ToLower / ToLowerInvariant    |            |
| ToUpper / ToUpperInvariant    |            |

#### System.Guid

| Methods       | Properties |
|---------------|------------|
| NewGuid       |            |
| Parse(string) |            |