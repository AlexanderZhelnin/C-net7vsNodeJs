using drawer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawer;

internal static class Text
{
    //ctx: CanvasRenderingContext2D,
    /** Расчёт данных для отображения текста */
    public static IText? calcText(string text, ILegend l, double[] coords, DrawProperties pr, Func<ILegend, string, (double width, double height)> measureText)
    {
        if (text.Trim().Length == 0) return null;
        if (l.text.mashtabRange.min > pr.mashtab || l.text.mashtabRange.max < pr.mashtab) return null;
        if (l.text.color == "rgba(0, 0, 0, 0)") return null;

        var (width, height) = measureText(l, text);

        var h = width / pr.scale;
        var w = height / pr.scale;

        switch (l.type)
        {
            case GrTypeEnum.Polygon:
                {
                    var rect = Calc.CalcRect(coords);
                    var x = (rect.left + rect.right) / 2 - w / 2;
                    var y = (rect.bottom + rect.top) / 2 - h / 2;
                    var tCoords = new double[] { x, y, x + w, y, x + w, y + h, x, y + h };
                    return new IText
                    {
                        text = text,
                        x = x,
                        y = y,
                        angle = 0,
                        rect = Calc.CalcRect(tCoords),
                        coords = tCoords
                    };
                }

            case GrTypeEnum.Line:
                {
                    var maxI = Calc.CalcMaxLen(coords);

                    var x1 = coords[maxI];
                    var y1 = coords[maxI + 1];
                    var x2 = coords[maxI + 2];
                    var y2 = coords[maxI + 3];

                    var cx = (x1 + x2) / 2;
                    var cy = (y1 + y2) / 2;


                    var dx = x2 - x1;
                    var dy = y2 - y1;

                    var angle = Math.Atan2(dy, dx);

                    // что бы надпись не была к верх ногами
                    if (x1 > x2)
                    {
                        angle += Math.PI;

                        x2 = coords[maxI];
                        y2 = coords[maxI + 1];
                        x1 = coords[maxI + 2];
                        y1 = coords[maxI + 3];

                        dx = -dx;
                        dy = -dy;
                    }

                    // вектор единичной длинны
                    var wX = Math.Cos(angle);
                    var wY = Math.Sin(angle);

                    cx -= (w * wX - wY * h) / 2;
                    cy -= (w * wY + wX * h) / 2;

                    var tCoords = new double[] {
                      cx, cy,
                      cx + w * wX, cy + w * wY,
                      cx + w * wX - wY * h, cy + w * wY + wX * h,
                      cx - wY * h, cy + wX * h };

                    return new IText { text = text, x = cx, y = cy, angle = angle, rect = Calc.CalcRect(tCoords), coords = tCoords };
                }
        }

        return null;
    }

    //public static int removedCount = 0;
    /** Оптимизация вывода текстов, убрать пересечение */
    //public static void optimizeTexts(List<ILayer> result)
    //{
    //    //var indexes = new List<int>();

    //    var dText = new List<IText>();
    //    for (var l = result.Count - 1; l >= 0; l--)
    //    {
    //        var texts = result[l].texts;
    //        //indexes.cl

    //        for (var i = texts.Count - 1; i >= 0; i--)
    //        {
    //            var txt = texts[i];
    //            var rt = txt.rect;

    //            var fined = false;
    //            foreach (var dt in dText)
    //                if (rt.left < dt.rect.right &&
    //                  rt.right > dt.rect.left &&
    //                  rt.bottom > dt.rect.top &&
    //                  rt.top < dt.rect.bottom)
    //                {
    //                    texts.RemoveAt(i);
    //                    fined = true;

    //                    //removedCount++;
    //                    break;
    //                }

    //            if (!fined) dText.Add(txt);
    //        }
    //    }
    //}

}

