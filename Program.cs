using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

var dictionary = File.ReadAllLines("dictionary.txt");

app.MapGet("/passphrase", () =>
    {
        var wordList = new List<string>();
        for (var i = 0; i < 3; i++)
        {
            wordList.Add(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dictionary[Random.Shared.Next(0, dictionary.Length - 1)]));
        }

        wordList[Random.Shared.Next(0, wordList.Count - 1)] += Random.Shared.Next(0, 9).ToString();

        var passphrase = string.Join('-', wordList);

        return new Passphrase(passphrase);
    }).WithName("GetPassphrase")
    .WithOpenApi();

app.Run();

record Passphrase(string passphrase);