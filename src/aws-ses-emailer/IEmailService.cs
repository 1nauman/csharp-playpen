namespace aws_ses_emailer;

public interface IEmailService
{
    /// <summary>
    /// Sends email asynchronously
    /// </summary>
    /// <param name="sender"><see cref="string"/></param>
    /// <param name="recipients"><see cref="IEnumerable{T}"/></param>
    /// <param name="subject"><see cref="string"/></param>
    /// <param name="body"><see cref="string"/></param>
    /// <param name="attachments"><see cref="IEnumerable{T}"/></param>
    /// <returns></returns>
    Task SendAsync(string sender, IEnumerable<string> recipients, string subject, string body,
        IEnumerable<EmailAttachment> attachments);
}