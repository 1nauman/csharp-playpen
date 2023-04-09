using google_recaptcha_enterprise;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var recaptchaConfiguration = new GoogleReCaptchaEnterpriseConfiguration();
builder.Configuration.GetRequiredSection(GoogleReCaptchaEnterpriseConfiguration.SectionName)
    .Bind(recaptchaConfiguration);
builder.Services.AddSingleton<GoogleReCaptchaEnterpriseConfiguration>(recaptchaConfiguration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();