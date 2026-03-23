using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient("auth", client =>
    client.BaseAddress = new Uri("http://localhost:5017")
);

builder.Services.AddHttpClient("property", client =>
    client.BaseAddress = new Uri("http://localhost:5062")
);

builder.Services.AddHttpClient("analytics", client =>
    client.BaseAddress = new Uri("http://localhost:5227")
);

var app = builder.Build();

app.UseCors("AllowAll");

// Proxy routes for microservices
app.MapWhen(context => context.Request.Path.StartsWithSegments("/auth-api"), appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
        var client = httpClientFactory.CreateClient("auth");
        
        var newPath = context.Request.Path.Value!.Replace("/auth-api", "");
        var url = $"{newPath}{context.Request.QueryString}";
        
        var request = new HttpRequestMessage(new HttpMethod(context.Request.Method), url);
        
        if (context.Request.Method != "GET" && context.Request.Method != "DELETE")
        {
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(body))
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        }
        
        foreach (var header in context.Request.Headers)
        {
            if (!header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase) &&
                !header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
        
        var response = await client.SendAsync(request);
        context.Response.StatusCode = (int)response.StatusCode;
        
        foreach (var header in response.Content.Headers)
            context.Response.Headers[header.Key] = header.Value.ToArray();
        
        await response.Content.CopyToAsync(context.Response.Body);
    });
});

app.MapWhen(context => context.Request.Path.StartsWithSegments("/property-api"), appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
        var client = httpClientFactory.CreateClient("property");
        
        var newPath = context.Request.Path.Value!.Replace("/property-api", "");
        var url = $"{newPath}{context.Request.QueryString}";
        
        var request = new HttpRequestMessage(new HttpMethod(context.Request.Method), url);
        
        if (context.Request.Method != "GET" && context.Request.Method != "DELETE")
        {
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(body))
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        }
        
        foreach (var header in context.Request.Headers)
        {
            if (!header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase) &&
                !header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
        
        var response = await client.SendAsync(request);
        context.Response.StatusCode = (int)response.StatusCode;
        
        foreach (var header in response.Content.Headers)
            context.Response.Headers[header.Key] = header.Value.ToArray();
        
        await response.Content.CopyToAsync(context.Response.Body);
    });
});

app.MapWhen(context => context.Request.Path.StartsWithSegments("/analytics-api"), appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
        var client = httpClientFactory.CreateClient("analytics");
        
        var newPath = context.Request.Path.Value!.Replace("/analytics-api", "");
        var url = $"{newPath}{context.Request.QueryString}";
        
        var request = new HttpRequestMessage(new HttpMethod(context.Request.Method), url);
        
        if (context.Request.Method != "GET" && context.Request.Method != "DELETE")
        {
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(body))
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        }
        
        foreach (var header in context.Request.Headers)
        {
            if (!header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase) &&
                !header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
        
        var response = await client.SendAsync(request);
        context.Response.StatusCode = (int)response.StatusCode;
        
        foreach (var header in response.Content.Headers)
            context.Response.Headers[header.Key] = header.Value.ToArray();
        
        await response.Content.CopyToAsync(context.Response.Body);
    });
});

app.Run();
