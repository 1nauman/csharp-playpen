using System.ComponentModel.DataAnnotations;

namespace google_recaptcha_enterprise.Models;

public class IndexModel
{
    [Required]
    public string ClientToken { get; set; }
}