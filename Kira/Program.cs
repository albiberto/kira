using Kira;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddLocalization();

builder.AddKiraOptions();
builder.AddKira();
builder.AddRadzen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRequestLocalization(options => options
    .AddSupportedCultures("en", "it-IT")
    .AddSupportedUICultures("en", "it-IT")
    .SetDefaultCulture("en"));

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();