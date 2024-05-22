namespace tax_model;

public class Tenant
{
    public long IssuerId { get; set; }
    
    public string Name { get; set; }
    
    public List<CountryTaxConfiguration> TaxConfigurations { get; set; }
}

public class CountryTaxConfiguration
{
    public string Name { get; set; }

    public List<TaxScheme> Schemes { get; set; }
}

public class TaxScheme
{
    public TaxSchemeType Type { get; set; }

    public List<TaxComponent> Components { get; set; }

    public DateOnly StartDate { get; set; }
}

public enum TaxSchemeType
{
    Default,
    IndiaOldRegime,
    IndiaNewRegime
}

public enum TaxConfigurationType
{
    Flat,
    Progressive
}

public enum TaxApplicationType
{
    TaxOnIncome,
    TaxOnTax
}

public class TaxComponent
{
    public string Name { get; set; }

    public TaxConfigurationType ConfigurationType { get; set; }

    public TaxApplicationType ApplicationType { get; set; }

    public decimal? Rate { get; set; }

    public List<TaxSlab>? Slabs { get; set; }

    public List<string> DependentComponents { get; set; }
}

public class TaxSlab
{
    public decimal Min { get; set; }

    public decimal Max { get; set; }

    public decimal Rate { get; set; }
}