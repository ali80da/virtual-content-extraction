﻿using Client.Core.Services.Extraction;
using Client.Web.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
{

    // Add services to the container.
    builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

    builder.Services.AddMudServices();

    builder.Services.AddScoped<ITextExtractionService, TextExtractionService>();
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(Client.Shared._Imports).Assembly);

app.Run();
