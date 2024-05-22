// See https://aka.ms/new-console-template for more information

using tax_model;

var indiaTaxConfiguration = new CountryTaxConfiguration
{
    Name = "India",

    Schemes =
    [
        new TaxScheme
        {
            Type = TaxSchemeType.IndiaOldRegime,
            StartDate = new DateOnly(1999, 01, 01),
            Components =
            [
                new TaxComponent
                {
                    Name = "Income Tax",
                    ConfigurationType = TaxConfigurationType.Progressive,
                    ApplicationType = TaxApplicationType.TaxOnIncome,
                    Slabs = new List<TaxSlab>
                    {
                        new()
                        {
                            Min = 0,
                            Max = 250000,
                            Rate = 0
                        },
                        new()
                        {
                            Min = 250001,
                            Max = 500000,
                            Rate = 0.05m
                        },
                        new()
                        {
                            Min = 500001,
                            Max = 1000000,
                            Rate = 0.1m
                        },
                        new()
                        {
                            Min = 1000001,
                            Max = 2000000,
                            Rate = 0.15m
                        },
                        new()
                        {
                            Min = 2000001,
                            Max = 5000000,
                            Rate = 0.2m
                        },
                        new()
                        {
                            Min = 5000001,
                            Max = decimal.MaxValue,
                            Rate = 0.3m
                        }
                    }
                },
                new TaxComponent
                {
                    Name = "Health and Education Cess",
                    ConfigurationType = TaxConfigurationType.Flat,
                    ApplicationType = TaxApplicationType.TaxOnTax,
                    Rate = 0.04m
                },
                new TaxComponent
                {
                    Name = "Surcharge",
                    ConfigurationType = TaxConfigurationType.Progressive,
                    ApplicationType = TaxApplicationType.TaxOnTax,
                    Slabs = new List<TaxSlab>
                    {
                        new()
                        {
                            Min = 0,
                            Max = 5000000,
                            Rate = 0
                        },
                        new()
                        {
                            Min = 5000001,
                            Max = 10000000,
                            Rate = 0.1m
                        },
                        new()
                        {
                            Min = 10000001,
                            Max = 20000000,
                            Rate = 0.15m
                        },
                        new()
                        {
                            Min = 20000001,
                            Max = 50000000,
                            Rate = 0.25m
                        },
                        new()
                        {
                            Min = 50000001,
                            Max = decimal.MaxValue,
                            Rate = 0.37m
                        }
                    }
                }
            ]
        },
        new TaxScheme
        {
            Type = TaxSchemeType.IndiaOldRegime,
            StartDate = new DateOnly(2020, 04, 01),
            Components =
            [
                new TaxComponent
                {
                    Name = "Income Tax",
                    ConfigurationType = TaxConfigurationType.Progressive,
                    ApplicationType = TaxApplicationType.TaxOnIncome,
                    Slabs = new List<TaxSlab>
                    {
                        new()
                        {
                            Min = 0,
                            Max = 250000,
                            Rate = 0
                        },
                        new()
                        {
                            Min = 250001,
                            Max = 500000,
                            Rate = 0.05m
                        },
                        new()
                        {
                            Min = 500001,
                            Max = 750000,
                            Rate = 0.1m
                        },
                        new()
                        {
                            Min = 750001,
                            Max = 1000000,
                            Rate = 0.15m
                        },
                        new()
                        {
                            Min = 1000001,
                            Max = 1250000,
                            Rate = 0.2m
                        },
                        new()
                        {
                            Min = 1250001,
                            Max = 1500000,
                            Rate = 0.25m
                        },
                        new()
                        {
                            Min = 1500001,
                            Max = 1875000,
                            Rate = 0.3m
                        },
                        new()
                        {
                            Min = 1875001,
                            Max = 2500000,
                            Rate = 0.35m
                        },
                        new()
                        {
                            Min = 2500001,
                            Max = decimal.MaxValue,
                            Rate = 0.4m
                        }
                    }
                },
                new TaxComponent
                {
                    Name = "Health and Education Cess",
                    ConfigurationType = TaxConfigurationType.Flat,
                    ApplicationType = TaxApplicationType.TaxOnTax,
                    Rate = 0.04m
                },
                new TaxComponent
                {
                    Name = "Surcharge",
                    ConfigurationType = TaxConfigurationType.Progressive,
                    ApplicationType = TaxApplicationType.TaxOnTax,
                    Slabs = new List<TaxSlab>
                    {
                        new()
                        {
                            Min = 0,
                            Max = 5000000,
                            Rate = 0
                        },
                        new()
                        {
                            Min = 5000001,
                            Max = 10000000,
                            Rate = 0.1m
                        },
                        new()
                        {
                            Min = 10000001,
                            Max = 20000000,
                            Rate = 0.15m
                        },
                        new()
                        {
                            Min = 20000001,
                            Max = decimal.MaxValue,
                            Rate = 0.25m
                        }
                    }
                }
            ]
        }
    ]
};