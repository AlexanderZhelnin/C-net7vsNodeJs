using apiTest;
using drawer;
using drawer.Models;
using System.Numerics;
using System.Text;

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

    var pr1 = new DrawProperties1()
    {
        Mashtab = pr.Mashtab,
        Scale = pr.Scale,
        LeftTop = new Vector<double>(new double[] { pr.Left + x, pr.Top + y, pr.Left + x, pr.Top + y })
    };

    var rect1 = new Rect()
    {
        Left = rect.Left + x,
        Top = rect.Top + y,
        Bottom = rect.Bottom,
        Right = rect.Right
    };


    var result = Drawer.Build(
        Init.ls,
        ref pr1,
        ref rect1);

    return result.Length;
    #region
    //return System.Text.Json.JsonSerializer.Serialize(result).Length;

    //using var ms = new MemoryStream();
    //await Utf8Json.JsonSerializer.SerializeAsync(ms, result);
    //return ms.Length;

    //var sw = new StringWriter();
    //var w = new JsonTextWriter(sw);
    //w.WriteStartArray();
    //for (var i = 0; i < result.Length; i++)
    //{
    //    w.WriteStartObject();

    //    w.WritePropertyName("LegendId");
    //    w.WriteValue(result[i].LegendId);

    //    w.WritePropertyName("Coords");
    //    w.WriteStartArray();

    //    foreach (var css in result[i].Coords)
    //    {
    //        w.WriteStartArray();
    //        for (var j = 0; j < css.Length; j++)
    //            w.WriteValue(css[j]);
    //        w.WriteEndArray();
    //    }

    //    w.WriteEndArray();

    //    w.WriteEndObject();
    //}

    //w.WriteEndArray();

    //return sw.ToString().Length;

    //var sb = new StringBuilder(1000000);
    //sb.Append('[');
    //for (var i = 0; i < result.Length; i++)
    //{
    //    sb.Append("{\"LegendId\":");
    //    sb.Append(result[i].LegendId);
    //    sb.Append(",\"Coords\":[");
    //    var coords = result[i].Coords;
    //    foreach (var css in coords)
    //    {
    //        sb.Append('[');
    //        for (var j = 0; j < css.Length; j++)
    //        {

    //            sb.Append(css[j]);
    //            sb.Append(',');
    //        }
    //        sb.Append("],");
    //    }
    //    sb.Length--;
    //    sb.Append("]},");
    //}
    //sb.Length--;

    //sb.Append(']');
    //return sb.Length;
    #endregion

});


app.MapGet("/mapJSON", (double x, double y) =>
{
    x /= 100;
    y /= 100;

    var pr1 = new DrawProperties1()
    {
        Mashtab = pr.Mashtab,
        Scale = pr.Scale,
        LeftTop = new Vector<double>(new double[] { pr.Left + x, pr.Top + y, pr.Left + x, pr.Top + y })
    };

    var rect1 = new Rect()
    {
        Left = rect.Left + x,
        Top = rect.Top + y,
        Bottom = rect.Bottom,
        Right = rect.Right
    };

    var result = Drawer.Build(
        Init.ls,
        ref pr1,
        ref rect1);

    return System.Text.Json.JsonSerializer.Serialize(result).Length;    
});

app.MapGet("/mapMyJSON", (double x, double y) =>
{
    x /= 100;
    y /= 100;

    var pr1 = new DrawProperties1()
    {
        Mashtab = pr.Mashtab,
        Scale = pr.Scale,
        LeftTop = new Vector<double>(new double[] { pr.Left + x, pr.Top + y, pr.Left + x, pr.Top + y })
    };

    var rect1 = new Rect()
    {
        Left = rect.Left + x,
        Top = rect.Top + y,
        Bottom = rect.Bottom,
        Right = rect.Right
    };

    var result = Drawer.Build(
        Init.ls,
        ref pr1,
        ref rect1);
        
    return result.ToJson().Length;

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
