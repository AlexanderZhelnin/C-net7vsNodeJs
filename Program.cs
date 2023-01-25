using apiTest;
using drawer;
using drawer.Models;
using System.Numerics;


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

var pr = new DrawProperties { Left = Init.r.Left, Top = Init.r.Top, Scale = 0.37037037037037035, Mashtab = 100 };
var rect = new Rect { Left = 1200, Bottom = 50, Right = 4000, Top = 2850 };
app.MapGet("/map", (double x, double y) =>
{
    x /= 100;
    y /= 100;

    var result = Drawer.Build(
        Init.ls,
        new()
        {
            Mashtab = pr.Mashtab,
            Scale = pr.Scale,
            LeftTop = new Vector<double>(new double[] { pr.Left + x, pr.Top + y, pr.Left + x, pr.Top + y })         
        },
        new()
        {
            Left = rect.Left + x,
            Top = rect.Top + y,
            Bottom = rect.Bottom,
            Right = rect.Right
        });

    return System.Text.Json.JsonSerializer.Serialize(result).Length;
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