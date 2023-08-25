using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace aws_ses_emailer;

public class AWSEmailService : IEmailService
{
    private readonly IAmazonSimpleEmailServiceV2 _client;
    private readonly ILogger<AWSEmailService> _logger;

    public AWSEmailService(IAmazonSimpleEmailServiceV2 client, ILogger<AWSEmailService> logger)
    {
        Guard.Against.Null(client);
        Guard.Against.Null(logger);

        _client = client;
        _logger = logger;
    }

    public async Task SendAsync(string sender, IEnumerable<string> recipients, string subject,
        string body,
        IEnumerable<EmailAttachment> attachments)
    {
        // parameter null checking
        Guard.Against.NullOrEmpty(sender);
        Guard.Against.Null(recipients);
        Guard.Against.Null(subject);
        Guard.Against.Null(body);
        Guard.Against.Null(attachments);

        var message = new MimeMessage();

        message.From.Add(MailboxAddress.Parse(sender));
        message.To.AddRange(recipients.Select(MailboxAddress.Parse));
        message.Subject = subject;

        var bodyPart = new TextPart(TextFormat.Html) {Text = body};
        var multipart = new Multipart("mixed") {bodyPart};

        foreach (var attachment in attachments)
        {
            var mimePart = new MimePart(attachment.MediaType)
            {
                Content = new MimeContent(attachment.Content),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachment.FileName
            };
            multipart.Add(mimePart);
        }

        message.Body = multipart;

        try
        {
            using var memoryStream = new MemoryStream();
            await message.WriteToAsync(memoryStream);

            memoryStream.Position = 0;
            var rawMessage = new RawMessage {Data = memoryStream};

            var sendRequest = new SendEmailRequest
            {
                Content = new EmailContent {Raw = rawMessage}
            };

            var response = await _client.SendEmailAsync(sendRequest);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogWarning(
                    "Failed to send email. Response: {StatusCode}, MessageId: {MessageId}, {@Metadata}",
                    response.HttpStatusCode, response.MessageId, response.ResponseMetadata);
            }
            else
            {
                _logger.LogInformation("Email sent successfully. MessageId: {MessageId}",
                    response.MessageId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email");
        }
    }
}