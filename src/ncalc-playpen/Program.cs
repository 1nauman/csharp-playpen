// See https://aka.ms/new-console-template for more information

using NCalc;

// var settlementAmountExpr = new Expression("Round([SettlementPrice] * [Units], 2)")
// {
//     Parameters =
//     {
//         ["SettlementPrice"] = new Expression("[FMV]"),
//         ["Units"] = 10
//     }
// };
//
// settlementAmountExpr.EvaluateParameter += (name, args) =>
// {
//     if (name == "FMV")
//     {
//         args.Result = 123.45d;
//     }
// };
//
// var settlementAmount = settlementAmountExpr.Evaluate();
//
// Console.WriteLine("Settlement Amount: {0}", settlementAmount);

const string formula = "Round((FairMarketValue - ExercisePrice) * NumberOfUnits, PerquisiteValuePrecision)";
var e = new Expression(formula)
{
    Parameters =
    {
        ["FairMarketValue"] = 100.0,
        ["ExercisePrice"] = 10.785,
        ["NumberOfUnits"] = 10,
        ["PerquisiteValuePrecision"] = 2
    }
};
var result = (double)e.Evaluate();

Console.WriteLine("Perquisite Value: {0}", result);