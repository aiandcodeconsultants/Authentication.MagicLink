using Authentication.MagicLink.Extensions;
using Authentication.MagicLink.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services
    .AddAuthenticationMagicLink(builder.Configuration)
    .AddEmailProvider<MailKitEmailService>(); // +

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Map("/secure", ([FromServices] IHttpContextAccessor httpContextAccessor)
    => $"This is a secure page ... welcome {httpContextAccessor.HttpContext.User.Identity.Name}")
    .RequireAuthorization();

app.MapMagicLink();
app.MapRazorPages();

app.Run();
