// See https://aka.ms/new-console-template for more information

using tax_configuration;

var indiaOldRegime = new RegionalTaxConfigurationV2
{
    Id = Guid.NewGuid().ToString(),
    IssuerId = 1, // Replace with your actual IssuerId
    Region = new TaxRegion { Country = "India" },
    Scheme = TaxScheme.IndiaOldRegime,
    EffectiveDate = new DateRange { Start = new DateOnly(2023, 4, 1), End = new DateOnly(2024, 3, 31) }, // FY 2023-24
    TaxHeadings =
    [
        new TaxHeading
        {
            Name = "Income Tax",
            Basis = TaxBasis.Income,
            Rate = new TaxRate
            {
                Type = TaxRateType.Progressive,
                Progressive =
                [
                    new TaxSlab { LowerLimit = 0, UpperLimit = 250000, Rate = 0 },
                    new TaxSlab { LowerLimit = 250001, UpperLimit = 500000, Rate = 5 },
                    new TaxSlab { LowerLimit = 500001, UpperLimit = 750000, Rate = 10 },
                    new TaxSlab { LowerLimit = 750001, UpperLimit = 1000000, Rate = 15 },
                    new TaxSlab { LowerLimit = 1000001, UpperLimit = 1250000, Rate = 20 },
                    new TaxSlab { LowerLimit = 1250001, UpperLimit = 1500000, Rate = 25 },
                    new TaxSlab { LowerLimit = 1500001, Rate = 30 }
                ]
            }
        },

        // Surcharge
        new TaxHeading
        {
            Name = "Surcharge",
            Basis = TaxBasis.Tax,
            Rate = new TaxRate
            {
                Type = TaxRateType.Progressive,
                Progressive =
                [
                    new TaxSlab { LowerLimit = 0, UpperLimit = 5000000, Rate = 0 },
                    new TaxSlab { LowerLimit = 5000001, UpperLimit = 10000000, Rate = 10 },
                    new TaxSlab { LowerLimit = 10000001, UpperLimit = 20000000, Rate = 15 },
                    new TaxSlab { LowerLimit = 20000001, Rate = 25 }
                ]
            },
            BaseHeads = ["Income Tax"],
            CalculationType = BaseHeadsCalculationType.Direct
        },

        // Health and Education Cess
        new TaxHeading
        {
            Name = "Health and Education Cess",
            Basis = TaxBasis.Tax,
            Rate = new TaxRate { Type = TaxRateType.Flat, Flat = new Percent { Value = 4m } },
            BaseHeads = ["Income Tax", "Surcharge"],
            CalculationType = BaseHeadsCalculationType.Sum
        }
    ]
};

Console.WriteLine(indiaOldRegime);