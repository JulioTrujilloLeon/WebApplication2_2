using Microsoft.Extensions.Primitives;
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    //app.UseSwagger();
    //app.UseSwaggerUI();
//}

app.MapPost("/", async context =>
{
    StringValues? name = context.Request.Query["name"];

    string error = "";
    string ret = "";

    try
    {
        SoapSimple client = new SoapSimple();
         ret = await client.Send();
    }
    catch (Exception e)
    {
        error += e.Message + " " + e.StackTrace;
    }
    await context.Response.WriteAsync($"Hola, {name ?? "Mundo"}! {error} {ret} v1");
});

//app.MapControllers();

await app.RunAsync();
