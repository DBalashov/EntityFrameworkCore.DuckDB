using System.Text.RegularExpressions;

namespace EntityFrameworkCore.DuckDB.Tests;

public class BaseTest
{
    /// <summary> split the input query by Environment.NewLine, extract Groups[1] with regex from last line and compare with mustBe </summary>
    protected bool ValidateFirstGroup(string inputQuery, string regex, string mustBe)
    {
        var input = inputQuery.Split(Environment.NewLine).Last();
        var m     = Regex.Match(input, regex, RegexOptions.IgnoreCase);
        return m.Success && m.Groups[1].Value == mustBe;
    }
}