using drawer.Models;
using Newtonsoft.Json;

namespace drawer;

public static class Init
{
    public static ILegend[] ls;

    public static Rect r;
    public static double cx;
    public static double cy;
    
    static Init()
    {
        var json = File.ReadAllText(@"C:\Project\angular\angular-canvas\src\assets\primitives.json");
        var dt = DateTime.Now;
        ls = JsonConvert.DeserializeObject<ILegend[]>(json);

        Console.WriteLine((DateTime.Now - dt).TotalMilliseconds + " загрузка json");

        ls = ls.ToList().OrderBy(l => l.priority).ToArray();
        r = new Rect { left = 1200, bottom = 50, right = 4000, top = 2850 };
        cx = (r.right + r.left) / 2;
        cy = (r.bottom + r.top) / 2;
    }

    public static void tt() { }

}
