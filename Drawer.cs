using drawer.Models;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace drawer;

internal static class Drawer
{
    /** Преобразование примитивов с учётом отсечения */
    //private static IEnumerable<IObraz> ClipPrimitives(ILegend l, Rect rect)
    //{
    //    foreach (var g in l.Primitives)
    //        //if (g.Rect.Left >= rect.Left && g.Rect.Bottom >= rect.Bottom && g.Rect.Right <= rect.Right && g.Rect.Top <= rect.Top)
    //        //{
    //            // Целиком лежит внутри прямоугольника
    //            yield return new IObraz { Coords = (double[])g.Coords.Clone() };
    //        //}
    //        //else if (g.Rect.Left < rect.Right && g.Rect.Bottom < rect.Top && g.Rect.Right > rect.Left && g.Rect.Top > rect.Bottom)
    //        //{
    //        //    // Необходимо отсекать
    //        //    switch (l.Type)
    //        //    {
    //        //        case GrTypeEnum.Line:
    //        //            foreach (var cs in Poliline.ClipPolyline(g, rect))
    //        //            {
    //        //                yield return new IObraz { Coords = cs };
    //        //            }
    //        //            break;
    //        //        case GrTypeEnum.Polygon:
    //        //            {
    //        //                var cs = Polygon.ClipPolygon(g, rect);
    //        //                if (cs.Length > 0)
    //        //                    yield return new IObraz { Coords = cs };
    //        //            }
    //        //            break;
    //        //    }
    //        //}
    //}

    private static IEnumerable<IObraz> ClipPrimitives(ILegend l, Rect rect)
    {
        var result = new List<IObraz>(l.Primitives.Length);
        foreach (var g in l.Primitives)
            if (g.Rect.Left >= rect.Left && g.Rect.Bottom >= rect.Bottom && g.Rect.Right <= rect.Right && g.Rect.Top <= rect.Top)
            {
                // Целиком лежит внутри прямоугольника
                yield return new IObraz { Coords = (double[])g.Coords.Clone() };
        }
        else if (g.Rect.Left < rect.Right && g.Rect.Bottom < rect.Top && g.Rect.Right > rect.Left && g.Rect.Top > rect.Bottom)
            {
                // Необходимо отсекать
                switch (l.Type)
                {
                    case GrTypeEnum.Line:
                        foreach (var cs in Poliline.ClipPolyline(g, rect))
                        {
                            yield return new IObraz { Coords = cs };
                        }
                        break;
                    case GrTypeEnum.Polygon:
                        {
                            var cs = Polygon.ClipPolygon(g, rect);
                            if (cs.Length > 0)
                                yield return new IObraz { Coords = cs };
                        }
                        break;
                }
            }
    }

    /** Подготовка данных для отрисовки */
    public static ILayer[] Build(ILegend[] ls, ref DrawProperties1 pr, ref Rect rect)
    {
        var result = new List<ILayer>(ls.Length);
        var mashtab = 1 / pr.Scale;

        foreach (var l in ls)
        {
            if (l.MashtabRange.Min > pr.Mashtab || l.MashtabRange.Max < pr.Mashtab) continue;
            
            var mas = new List<double[]>();

            foreach (var obraz in ClipPrimitives(l, rect))
            {                
                var csOpt = Calc.Optimize(obraz.Coords, mashtab);
                Calc.Translate(csOpt, pr);
                mas.Add(csOpt);
            }

            result.Add(new() { LegendId = l.Id, Coords = mas.ToArray() });
        }

        return result.ToArray();
    }

    /** Подготовка данных для отрисовки */
    //public static IEnumerable<ILayer> BuildParallel(ILegend[] ls, DrawProperties pr, Rect rect)
    //{
    //    var mashtab = 1 / pr.Scale;

    //    return ls
    //        .Where(l => l.MashtabRange.Min <= pr.Mashtab && l.MashtabRange.Max >= pr.Mashtab)
    //        .AsParallel()
    //        .Select(l =>
    //        {
    //            var mas = ClipPrimitives(l, rect)
    //                .Select(obraz =>
    //                {
    //                    var csOpt = Calc.Optimize(obraz.coords, mashtab);
    //                    Calc.Translate(csOpt, pr);
    //                    return csOpt;
    //                });

    //            return new ILayer() { legendId = l.Id, coords = mas.ToArray() };
    //        });
    //}


    public static string ToJson(this ILayer[] ls)
    {
        var sb = new StringBuilder(1000000);
        sb.Append('[');
        for (var i = 0; i < ls.Length; i++)
        {
            sb.Append("{\"LegendId\":");
            sb.Append(ls[i].LegendId);
            sb.Append(",\"Coords\":[");
                        
            foreach (var css in ls[i].Coords)
            {
                sb.Append('[');
                for (var j = 0; j < css.Length; j++)
                {
                    //sb.Append(css[j]);

                    bool bNegative = false;
                    if (css[j] < 0) { css[j] = -css[j]; bNegative = true; }

                    var fv = Math.Floor(css[j]);
                    var ii = (Int64)fv;
                    var f = (Int64)Math.Floor((css[j] - fv) * 10000000000.0);

                    if (bNegative) sb.Append('-');

                    sb.Append(ii);
                    sb.Append('.');
                    sb.Append(f);
                    sb.Append(',');
                }

                if (css.Length > 0) sb.Length--;
                sb.Append("],");
            }

            if (ls[i].Coords.Length > 0) sb.Length--;
            sb.Append("]},");
        }

        if (ls.Length > 0) sb.Length--;
        sb.Append(']');

        return sb.ToString();
    }
}
