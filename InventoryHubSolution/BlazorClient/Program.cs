using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Server Url
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5186") });



await builder.Build().RunAsync();

