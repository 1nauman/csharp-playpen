namespace tax_configuration;

public class RegionalTaxConfigurationV2
{
    /// <summary>
    /// Unique identifier for the tax configuration.
    /// Guid.NewGuid().ToString() can be used.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// TenantId
    /// </summary>
    public long IssuerId { get; set; }

    /// <summary>
    /// Region to which this tax configuration is applicable.
    /// </summary>
    public TaxRegion Region { get; set; }

    /// <summary>
    /// Tax scheme, e.g., Old Regime or New Regime.
    /// </summary>
    public TaxScheme? Scheme { get; set; }

    /// <summary>
    /// Effective date range for the tax configuration.
    /// </summary>
    public DateRange EffectiveDate { get; set; }

    /// <summary>
    /// Tax headings and their rates.
    /// </summary>
    public List<TaxHeading> TaxHeadings { get; set; }
}

public class TaxHeading
{
    public string Name { get; set; }

    public TaxBasis Basis { get; set; }

    public TaxRate Rate { get; set; }

    /// <summary>
    /// Base heads using which this tax heading is calculated.
    /// Like in case of Cess, it is calculated as (IncomeTax + Surcharge (if there is any))
    /// </summary>
    public string[]? BaseHeads { get; set; }

    public BaseHeadsCalculationType? CalculationType { get; set; }
}

public enum BaseHeadsCalculationType
{
    /// <summary>
    /// Applies the tax rate directly on the base head.
    /// </summary>
    Direct,

    /// <summary>
    /// Applies the tax rate on the sum of the base heads.
    /// </summary>
    Sum
}

public enum TaxRateType
{
    Flat,
    Progressive
}

public class TaxRate
{
    public TaxRateType Type { get; set; }

    public Percent? Flat { get; set; }

    public List<TaxSlab>? Progressive { get; set; }
}

public record Percent
{
    public decimal Value { get; init; }
}

public class StakeholderTaxRecord
{
    // can be Guid.NewGuid().ToString()
    public string Id { get; set; }

    public long IssuerId { get; set; }

    public long StakeholderId { get; set; }

    /// <summary>
    /// Tax region, to which this tax rate is applicable.
    /// </summary>
    public string TaxIdentifierCountry { get; set; }

    public StakeholderTaxRate[] TaxRates { get; set; }

    public TaxableIncome? TaxableIncome { get; set; }
}

public class TaxableIncome
{
    /// <summary>
    /// Tax scheme, e.g., Old Regime or New Regime.
    /// </summary>
    public TaxScheme? Scheme { get; set; }

    public Money Income { get; set; }

    public DateOnly PayrollDate { get; set; }
}

public enum TaxScheme
{
    IndiaOldRegime,
    IndiaNewRegime
}

public class TaxRegion
{
    /// <summary>
    /// Country is mandatory
    /// </summary>
    public string Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? County { get; set; }
}

public class DateRange
{
    public DateOnly Start { get; set; }

    public DateOnly? End { get; set; }
}

public class TaxSlab
{
    public decimal LowerLimit { get; set; }

    public decimal? UpperLimit { get; set; }

    public decimal Rate { get; set; }
}

public enum TaxBasis
{
    Income, // Applied to the original income amount
    Tax // Applied to the calculated tax amount (e.g., surcharges)
}

public class StakeholderTaxRate
{
    public string TaxHeading { get; set; }

    public Percent Rate { get; set; }

    public Percent? SeparatedRate { get; set; }
}

/// <summary>
/// IEnumerationValue
/// </summary>
public record Currency
{
    public static Currency USD = new Currency { Code = "USD" };

    public static Currency INR = new Currency { Code = "INR" };

    public string Code { get; init; }
}

public record Money
{
    public decimal Amount { get; init; }

    public Currency Currency { get; init; }
}