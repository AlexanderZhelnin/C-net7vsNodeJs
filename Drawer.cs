using drawer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawer;

internal static class Drawer
{

    /** Преобразование примитивов с учётом отсечения */
    private static IEnumerable<IObraz> ClipPrimitives(ILegend l, DrawProperties pr, Rect rect)
    {
        foreach (var g in l.primitives)
            if (g.rect.left >= rect.left && g.rect.bottom >= rect.bottom && g.rect.right <= rect.right && g.rect.top <= rect.top)
                // Целиком лежит внутри прямоугольника
                yield return new IObraz { coords = (double[])g.coords.Clone() };
            else if (g.rect.left < rect.right && g.rect.bottom < rect.top && g.rect.right > rect.left && g.rect.top > rect.bottom)
                // Необходимо отсекать
                switch (l.type)
                {
                    case GrTypeEnum.Line:

                        foreach (var cs in Poliline.ClipPolyline(g, rect))
                            yield return new IObraz { coords = cs };
                        break;
                    case GrTypeEnum.Polygon:
                        {
                            var cs = Polygon.ClipPolygon(g, rect);
                            if (cs.Length > 0)
                                yield return new IObraz { coords = cs };
                        }

                        break;
                }
    }

    /** Подготовка данных для отрисовки */
    public static ILayer[] Build(ILegend[] ls, DrawProperties pr, Rect rect)
    {
        var result = new List<ILayer>();
        var mashtab = 1 / pr.scale;

        foreach (var l in ls)
        {
            if (l.mashtabRange.min > pr.mashtab || l.mashtabRange.max < pr.mashtab) continue;
            var mas = new List<double[]>();

            foreach (var obraz in ClipPrimitives(l, pr, rect))
            {
                var csOpt = Calc.Optimize(obraz.coords, mashtab);
                Calc.Translate(csOpt, pr);
                mas.Add(csOpt);
            }

            result.Add(new() { legendId = l.id, coords = mas.ToArray() });
        }

        return result.ToArray();
    }

    /** Подготовка данных для отрисовки */
    public static IEnumerable<ILayer> BuildParallel(ILegend[] ls, DrawProperties pr, Rect rect)
    {
        var mashtab = 1 / pr.scale;

        return ls
            .Where(l => l.mashtabRange.min <= pr.mashtab && l.mashtabRange.max >= pr.mashtab)
            .AsParallel()
            .Select(l =>
            {
                var mas = ClipPrimitives(l, pr, rect)
                    .Select(obraz =>
                    {
                        var csOpt = Calc.Optimize(obraz.coords, mashtab);
                        Calc.Translate(csOpt, pr);
                        return csOpt;
                    });

                return new ILayer() { legendId = l.id, coords = mas.ToArray() };
            });
    }
}
