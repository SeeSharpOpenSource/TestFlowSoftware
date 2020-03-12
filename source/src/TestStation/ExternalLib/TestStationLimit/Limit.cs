using System;
using Testflow.Usr;

namespace TestStationLimit
{
    public static class Limit
    {
        //todo I18n
        public static void AssertBoolean(string low, string high, string comparisonType, bool expression, string unit = "")
        {
            bool value;
            bool result = bool.TryParse(low, out value);
            if (!result)
            {
                throw new TestflowAssertException($"Boolean \"==\": expected({low}), real({expression.ToString()})");
            }
            switch (comparisonType)
            {
                case "==":
                    if (value != expression)
                    {
                        throw new TestflowAssertException(
                            $"Boolean \"==\": expected({low}), real({expression.ToString()})");
                    }
                    break;

                case "!=":
                    if (value == expression)
                    {
                        throw new TestflowAssertException(
                            $"Boolean \"!=\": expected({low}), real({expression.ToString()})");
                    }
                    break;
            }
        }

        public static void AssertString(string low, string high, string comparisonType, string expression, string unit = "")
        {
            switch (comparisonType)
            {
                case "EQU":
                    if (!low.Equals(expression))
                    {
                        throw new TestflowAssertException($"String \"==\": expected({low}), real({expression.ToString()})");
                    }
                    break;

                case "NEQ":
                    if (low.Equals(expression))
                    {
                        throw new TestflowAssertException($"String \"!=\": expected({low}), real({expression.ToString()})");
                    }
                    break;
                case "EQU IgnoreCase":
                    if (!low.Equals(expression, StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new TestflowAssertException($"String \"==\": expected({low}), real({expression.ToString()})");
                    }
                    break;

                case "NEQ IgnoreCase":
                    if (low.Equals(expression, StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new TestflowAssertException($"String \"!=\": expected({low}), real({expression.ToString()})");
                    }
                    break;
            }
        }

        const double MinDouble = 1E-10;
        public static void AssertNumeric(string low, string high, string comparisonType, double expression, string unit = "")
        {
            ;
            double lowDouble, highDouble;
            bool lowParseFail = !double.TryParse(low, out lowDouble);
            bool highParseFail = !double.TryParse(high, out highDouble);
            switch (comparisonType)
            {
                case "==":
                    if (lowParseFail || Math.Abs(expression - lowDouble) > MinDouble)
                    {
                        throw new TestflowAssertException($"Numeric \"==\": expected({low}), real({expression.ToString()})");
                    }
                    break;

                case "!=":
                    if (lowParseFail || Math.Abs(expression - lowDouble) < MinDouble)
                    {
                        throw new TestflowAssertException($"Numeric \"!=\": expected({low}), real({expression.ToString()})");
                    }
                    break;
                case ">":
                    if (lowParseFail || !(expression > lowDouble))
                    {
                        throw new TestflowAssertException($"Numeric \">\": expected({low}), real({expression.ToString()})");
                    }
                    break;
                case ">=":
                    if (lowParseFail || !(expression >= lowDouble))
                    {
                        throw new TestflowAssertException($"Numeric \">=\": expected({low}), real({expression.ToString()})");
                    }
                    break;
                case "<":
                    if (lowParseFail || !(expression < lowDouble))
                    {
                        throw new TestflowAssertException($"Numeric \"<\": expected({low}), real({expression.ToString()})");
                    }
                    break;
                case "<=":
                    if (lowParseFail || !(expression <= lowDouble))
                    {
                        throw new TestflowAssertException($"Numeric \"<=\": expected({low}), real({expression.ToString()})");
                    }
                    break;
                case "> <":
                    if(highParseFail || lowParseFail || !((expression > lowDouble) && (expression < highDouble)))
                    {
                        throw new TestflowAssertException($"Numeric \"> <\": expectedLow({low}), expectedHigh({high}), real({expression.ToString()})");
                    }
                    break;
                case ">= <=":
                    if (highParseFail || lowParseFail|| !((expression >= lowDouble) && (expression <= highDouble)))
                    {
                        throw new TestflowAssertException($"Numeric \">= <=\": expectedLow({low}), expectedHigh({high}), real({expression.ToString()})");
                    }
                    break;
                case ">= <":
                    if (highParseFail || lowParseFail || !((expression >= lowDouble) && (expression < highDouble)))
                    {
                        throw new TestflowAssertException($"Numeric \">= <\": expectedLow({low}), expectedHigh({high}), real({expression.ToString()})");
                    }
                    break;
                case "> <=":
                    if (highParseFail || lowParseFail || !((expression > lowDouble) && (expression <= highDouble)))
                    {
                        throw new TestflowAssertException($"Numeric \"> <=\": expectedLow({low}), expectedHigh({high}), real({expression.ToString()})");
                    }
                    break;
            }
        }

        public static string[] GetCompareTypes(string assertType)
        {
            string[] compareTypes = null;
            switch (assertType)
            {
                case "Numeric":
                    compareTypes = new string[] {"==", "!=", ">", ">=", "<", "<=", "> <", ">= <=", ">= <", "> <="};
                    break;
                case "Boolean":
                    compareTypes = new string[] { "==", "!=" };
                    break;
                case "String":
                    compareTypes = new string[] { "EQU", "NEQ", "EQU IgnoreCase", "NEQ IgnoreCase" };
                    break;
            }
            return compareTypes;
        }

        public static void GetHighLowAndUnitEnable(string assertType, string compareType, out bool highEnabled, out bool unitAvailable)
        {
            highEnabled = true;
            unitAvailable = true;
            switch (assertType)
            {
                case "Numeric":
                    unitAvailable = true;
                    switch (compareType)
                    {
                        case "==":
                            highEnabled = false;
                            break;
                        case "!=":
                            highEnabled = false;
                            break;
                        case ">":
                            highEnabled = false;
                            break;
                        case ">=":
                            highEnabled = false;
                            break;
                        case "<":
                            highEnabled = false;
                            break;
                        case "<=":
                            highEnabled = false;
                            break;
                        case "> <":
                            highEnabled = true;
                            break;
                        case ">= <=":
                            highEnabled = true;
                            break;
                        case ">= <":
                            highEnabled = true;
                            break;
                        case "> <=":
                            highEnabled = true;
                            break;
                    }
                    break;
                case "Boolean":
                    highEnabled = false;
                    unitAvailable = false;
                    break;
                case "String":
                    highEnabled = false;
                    unitAvailable = false;
                    break;
            }
        }
        // 获取默认的LowLimit的值
        public static string GetDefaultLowValue(string assertType)
        {
            string defaultValue = string.Empty;
            switch (assertType)
            {
                case "Numeric":
                    defaultValue = "0";
                    break;
                case "Boolean":
                    defaultValue = "True";
                    break;
                case "String":
                    defaultValue = "";
                    break;
            }
            return defaultValue;
        }

        // 获取默认的HighLimit的值
        public static string GetDefaultHighValue(string assertType)
        {
            return assertType.Equals("Numeric") ? "1" : "/";
        }

        public static bool IsValidValue(string assertType, string value)
        {
            bool isValid = false;
            switch (assertType)
            {
                case "Numeric":
                    double numeric;
                    isValid = double.TryParse(value, out numeric);
                    break;
                case "Boolean":
                    bool boolean;
                    isValid = bool.TryParse(value, out boolean);
                    break;
                case "String":
                    isValid = true;
                    break;
            }
            return isValid;
        }
    }
}