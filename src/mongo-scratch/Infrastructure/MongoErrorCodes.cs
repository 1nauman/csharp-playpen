namespace mongo_scratch.Infrastructure;

public static class MongoErrorCodes
{
    /// <summary>
    ///     https://stackoverflow.com/questions/18032879/mongodb-difference-between-error-code-11000-and-11001
    /// </summary>
    public static readonly int[] DuplicateKeyErrorCodes = { 11000, 11001, 12582 };
}