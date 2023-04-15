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
                
        ls = JsonConvert.DeserializeObject<ILegend[]>(json)!;        

        //ls = ls.ToList()
        //       .OrderBy(l => l.Priority)
        //       .ToArray();
        
        r = new Rect { Left = 1200, Bottom = 50, Right = 4000, Top = 2850 };
        cx = (r.Right + r.Left) / 2;
        cy = (r.Bottom + r.Top) / 2;
    }

    public static void tt() { }

}
