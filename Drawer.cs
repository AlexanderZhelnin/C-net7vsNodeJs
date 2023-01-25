using drawer.Models;
using System.Text;

namespace drawer;

internal static class Drawer
{
    /** Преобразование примитивов с учётом отсечения */
    private static IEnumerable<IObraz> ClipPrimitives(ILegend l, Rect rect)
    {
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
    public static ILayer[] Build(ILegend[] ls, DrawProperties1 pr, Rect rect)
    {
        var result = new List<ILayer>();
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
}
