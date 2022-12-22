namespace multiple_eula_query;

public record Eula(int Id, DateTime CreatedOn, string Content, int? IssuerId = null, bool Active = true);