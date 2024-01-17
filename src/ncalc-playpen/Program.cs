// See https://aka.ms/new-console-template for more information

using NCalc;

var settlementAmountExpr = new Expression("Round([SettlementPrice] * [Units], 2)")
{
    Parameters =
    {
        ["SettlementPrice"] = new Expression("[FMV]"),
        ["Units"] = 10
    }
};

settlementAmountExpr.EvaluateParameter += (name, args) =>
{
    if (name == "FMV")
    {
        args.Result = 123.45d;
    }
};

var settlementAmount = settlementAmountExpr.Evaluate();

Console.WriteLine("Settlement Amount: {0}", settlementAmount);