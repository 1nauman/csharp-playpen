// See https://aka.ms/new-console-template for more information

using Dumpify;
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
                    new TaxSlab
                    {
                        LowerLimit = 0, UpperLimit = 250000, Rate = new Percent(0)
                    }, // Slab 1: Up to ₹2.5 Lakhs
                    new TaxSlab
                    {
                        LowerLimit = 250001, UpperLimit = 500000, Rate = new Percent(5)
                    }, // Slab 2: ₹2.5 Lakhs to ₹5 Lakhs
                    new TaxSlab
                    {
                        LowerLimit = 500001, UpperLimit = 1000000, Rate = new Percent(20)
                    }, // Slab 3: ₹5 Lakhs to ₹10 Lakhs
                    new TaxSlab { LowerLimit = 1000001, Rate = new Percent(30) } // Slab 4: Above ₹10 Lakhs
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
                    new TaxSlab { LowerLimit = 0, UpperLimit = 5000000, Rate = new Percent(0) },
                    new TaxSlab { LowerLimit = 5000000, UpperLimit = 10000000, Rate = new Percent(10) },
                    new TaxSlab { LowerLimit = 10000000, UpperLimit = 20000000, Rate = new Percent(15) },
                    new TaxSlab { LowerLimit = 20000000, UpperLimit = 50000000, Rate = new Percent(25) },
                    new TaxSlab { LowerLimit = 50000000, Rate = new Percent(37) }
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
DumpConfig.Default.TableConfig.ShowRowSeparators = true;
//indiaOldRegime.Dump();

var usaFederalTaxes = new RegionalTaxConfigurationV2
{
    Id = Guid.NewGuid().ToString(),
    IssuerId = 1, // Replace with your actual IssuerId
    Region = new TaxRegion { Country = "USA" },
    Scheme = null,
    EffectiveDate = new DateRange { Start = new DateOnly(2023, 1, 1), End = new DateOnly(2023, 12, 31) },
    TaxHeadings =
    [
        new TaxHeading
        {
            Name = "Federal Income Tax",
            Basis = TaxBasis.Income,
            Rate = new TaxRate
            {
                Type = TaxRateType.Progressive,
                Progressive =
                [
                    new TaxSlab { LowerLimit = 0, UpperLimit = 10275, Rate = new Percent(10) },
                    new TaxSlab { LowerLimit = 10276, UpperLimit = 41775, Rate = new Percent(12) },
                    new TaxSlab { LowerLimit = 41776, UpperLimit = 89075, Rate = new Percent(22) },
                    new TaxSlab { LowerLimit = 89076, UpperLimit = 170050, Rate = new Percent(24) },
                    new TaxSlab { LowerLimit = 170051, UpperLimit = 215950, Rate = new Percent(32) },
                    new TaxSlab { LowerLimit = 215951, UpperLimit = 539900, Rate = new Percent(35) },
                    new TaxSlab { LowerLimit = 539901, Rate = new Percent(37) }
                ]
            }
        },
        new TaxHeading
        {
            Name = "Social Security Tax",
            Basis = TaxBasis.Income,
            Rate = new TaxRate
            {
                Type = TaxRateType.Flat,
                Flat = new Percent(6.2m)
            }
        },
        new TaxHeading
        {
            Name = "Medicare Tax",
            Basis = TaxBasis.Income,
            Rate = new TaxRate
            {
                Type = TaxRateType.Progressive,
                Progressive =
                [
                    new TaxSlab { LowerLimit = 0, UpperLimit = 200000, Rate = new Percent(1.45m) }, // For single filers
                    new TaxSlab { LowerLimit = 200001, Rate = new Percent(2.35m) }
                ]
            }
        }
    ]
};

var nyStateTaxes = new RegionalTaxConfigurationV2
{
    Id = Guid.NewGuid().ToString(),
    IssuerId = 1, // Replace with your actual IssuerId
    Region = new TaxRegion { Country = "USA" },
    Scheme = null,
    EffectiveDate = new DateRange { Start = new DateOnly(2023, 1, 1), End = new DateOnly(2023, 12, 31) },
    TaxHeadings =
    [
        new TaxHeading
        {
            Name = "NY State Income Tax",
            Basis = TaxBasis.Income,
            Rate = new TaxRate
            {
                Type = TaxRateType.Progressive,
                Progressive =
                [
                    new TaxSlab { LowerLimit = 0, UpperLimit = 8925, Rate = new Percent(4) },
                    new TaxSlab { LowerLimit = 8926, UpperLimit = 12390, Rate = new Percent(4.5m) },
                    new TaxSlab { LowerLimit = 12391, UpperLimit = 16665, Rate = new Percent(5.25m) },
                    new TaxSlab { LowerLimit = 16666, UpperLimit = 22170, Rate = new Percent(5.90m) },
                    new TaxSlab { LowerLimit = 22171, UpperLimit = 85665, Rate = new Percent(5.97m) },
                    new TaxSlab { LowerLimit = 85666, UpperLimit = 267625, Rate = new Percent(6.33m) },
                    new TaxSlab { LowerLimit = 267626, UpperLimit = 1077550, Rate = new Percent(6.85m) },
                    new TaxSlab { LowerLimit = 1077551, Rate = new Percent(10.3m) }
                ]
            }
        }
    ]
};

nyStateTaxes.Dump();