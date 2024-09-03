using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.Azure;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var blobUri = builder.Configuration["BlobUri"];
if (!string.IsNullOrEmpty(blobUri))
{
    builder.Services.AddAzureClients(clientBuilder =>
    {
        // Register clients for each service
        clientBuilder.AddBlobServiceClient(new Uri(blobUri));
        clientBuilder.UseCredential(new DefaultAzureCredential());
    });
}

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Files and folders API",
        Description = "Play around with file and folder performance test using these APIs",
        TermsOfService = new Uri("https://api.contoso.com/terms"),
        License = new OpenApiLicense
        {
            Name = "Use under MIT",
            Url = new Uri("https://opensource.org/licenses/MIT"),
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Files and folders API");
});

app.MapControllers();

await app.RunAsync();
