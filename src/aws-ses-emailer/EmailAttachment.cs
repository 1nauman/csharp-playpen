using Ardalis.GuardClauses;

namespace aws_ses_emailer;

/// <summary>
/// Represents an email attachment
/// </summary>
public record EmailAttachment
{
    public EmailAttachment(string fileName, string mediaType, Stream content)
    {
        Guard.Against.NullOrEmpty(fileName);
        Guard.Against.NullOrEmpty(mediaType);
        Guard.Against.Null(content);
        
        FileName = fileName;
        MediaType = mediaType;
        Content = content;
    }

    /// <summary>
    /// Attachment content as <see cref="Stream"/>
    /// </summary>
    public Stream Content { get; }

    /// <summary>
    /// File name of the attachment
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Media type of the attachment
    /// </summary>
    public string MediaType { get; } // For example, "application/pdf"
}