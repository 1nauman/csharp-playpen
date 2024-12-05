// See https://aka.ms/new-console-template for more information

using NCalc;

const string formula = "(FairMarketValue * NumberOfUnits * ConversionRatio) - (ExercisePrice * NumberOfUnits)";
var e = new Expression(formula)
{
    Parameters =
    {
        ["FairMarketValue"] = 100.0m,
        ["ExercisePrice"] = 10.0m,
        ["NumberOfUnits"] = 10m,
        ["ConversionRatio"] = 1.0m
        //["PerquisiteValuePrecision"] = 2
    }
};
var result = (decimal)e.Evaluate();

Console.WriteLine("Perquisite Value: {0}", result);