using System.ComponentModel.DataAnnotations;

namespace google_recaptcha.Models;

public class TestModel
{
    [Required]
    public string ClientToken { get; set; }

    public string? Message { get; set; }
}