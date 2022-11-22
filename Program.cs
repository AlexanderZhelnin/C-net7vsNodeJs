using apiTest;
using drawer;
using drawer.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

Init.tt();

app.MapGet("/", () => "Hello World dotnet!");

app.MapGet("/readfile", () => File.ReadAllText("data.txt"));

app.MapGet("/fibonacci", () =>
{
    var a = 0.0;
    var b = 1.0;
    var c = 0.0;
    for (var i = 2; i < 2000000; i++)
    {
        c = a + b;
        a = b;
        b = c;
    }

    return a.ToString();
});

var pr = new DrawProperties { left = Init.r.left, top = Init.r.top, scale = 0.37037037037037035, mashtab = 100 };
var rect = new Rect { left = 1200, bottom = 50, right = 4000, top = 2850 };
app.MapGet("/map", () =>
{
    return Drawer.Build(Init.ls, pr, rect);
});

const string STR1 = "asrgfsadf12421";
const string STR2 = "asrgfsadf12321";

app.MapGet("/naturalsort", () =>
{
    var result = 0;
    for (var i = 0; i < 10000; i++)
        result += Strings.CompareUnsafe(STR1 + i, STR2 + i);
    
    return result;
});

app.Run();