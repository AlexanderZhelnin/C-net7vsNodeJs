using drawer.Models;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace drawer;

internal static class Polygon
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNextIndex(int curIndex, int len)
    {
        curIndex += 2;
        if (curIndex >= len) curIndex = 0;
        return curIndex;
    }

    private static double[] ClipLeft(double[] coords, double left)
    {
        if (coords.Length == 0) return coords;

        var pl = new List<double>(coords.Length * 2);
        var curIndex = 0;

        var (px1, py1) = (coords[0], coords[1]);

        if (px1 >= left)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        var len = coords.Length / 2;
        for (var i = 1; i <= len; i++)
        {
            curIndex = GetNextIndex(curIndex, coords.Length);
            var (px2, py2) = (coords[curIndex], coords[curIndex + 1]);

            if (px1 >= left && px2 >= left)
            {
                pl.Add(px2);
                pl.Add(py2);
            }
            else if (px1 < left && px2 > left)
            {
                pl.Add(left);
                pl.Add((left - px1) * (py2 - py1) / (px2 - px1) + py1);

                pl.Add(px2);
                pl.Add(py2);
            }
            else if (px1 > left && px2 < left)
            {
                // pl.Add(px1);
                // pl.Add(py1);

                pl.Add(left);
                pl.Add((left - px1) * (py2 - py1) / (px2 - px1) + py1);
            }
            px1 = px2;
            py1 = py2;
        }

        return pl.ToArray();
    }
    private static double[] ClipRight(double[] coords, double right)
    {
        if (coords.Length == 0) return coords;
                
        var pl = new List<double>(coords.Length * 2);
        var curIndex = 0;

        var (px1, py1) = (coords[0], coords[1]);

        if (px1 <= right)
        {
            pl.Add(px1);
            pl.Add(py1);
        }
        var len = coords.Length / 2;

        for (var i = 0; i < len; i++)
        {
            curIndex = GetNextIndex(curIndex, coords.Length);

            var px2 = coords[curIndex];
            var py2 = coords[curIndex + 1];

            if (px1 <= right && px2 <= right)
            {
                pl.Add(px2);
                pl.Add(py2);
            }
            else if (px1 > right && px2 < right)
            {
                pl.Add(right);
                pl.Add((right - px1) * (py2 - py1) / (px2 - px1) + py1);

                pl.Add(px2);
                pl.Add(py2);
            }
            else if (px1 < right && px2 > right)
            {
                pl.Add(right);
                pl.Add((right - px1) * (py2 - py1) / (px2 - px1) + py1);
            }
            px1 = px2;
            py1 = py2;
        }

        return pl.ToArray();
    }
    private static double[] ClipBottom(double[] coords, double bottom)
    {
        if (coords.Length == 0) return coords;
        
        var pl = new List<double>(coords.Length * 2);
        var curIndex = 0;

        var (px1, py1) = (coords[0], coords[1]);

        if (py1 >= bottom)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        var len = coords.Length / 2;
        for (var i = 0; i < len; i++)
        {
            curIndex = GetNextIndex(curIndex, coords.Length);
            var (px2, py2) = (coords[curIndex], coords[curIndex + 1]);

            if (py1 >= bottom && py2 >= bottom)
            {
                pl.Add(px2);
                pl.Add(py2);
            }
            else if (py1 < bottom && py2 > bottom)
            {
                pl.Add((bottom - py1) * (px2 - px1) / (py2 - py1) + px1);
                pl.Add(bottom);

                pl.Add(px2);
                pl.Add(py2);
            }
            else if (py1 > bottom && py2 < bottom)
            {
                // pl.Add(px1);
                // pl.Add(py1);

                pl.Add((bottom - py1) * (px2 - px1) / (py2 - py1) + px1);
                pl.Add(bottom);

                // res.Add(pl);

                // pl = [];
            }
            px1 = px2;
            py1 = py2;
        }

        return pl.ToArray();
    }
    private static double[] ClipTop(double[] coords, double top)
    {
        if (coords.Length == 0) return coords;
        
        var pl = new List<double>(coords.Length * 2);
        var curIndex = 0;

        var (px1, py1) = (coords[0], coords[1]);

        if (py1 <= top)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        var len = coords.Length / 2;
        for (var i = 0; i < len; i++)
        {
            curIndex = GetNextIndex(curIndex, coords.Length);
            var (px2, py2) = (coords[curIndex], coords[curIndex + 1]);

            if (py1 <= top && py2 <= top)
            {
                pl.Add(px2);
                pl.Add(py2);
            }
            else if (py1 < top && py2 > top)
            {
                // pl.Add(px1);
                // pl.Add(py1);

                pl.Add((top - py1) * (px2 - px1) / (py2 - py1) + px1);
                pl.Add(top);
            }
            else if (py1 > top && py2 < top)
            {
                pl.Add((top - py1) * (px2 - px1) / (py2 - py1) + px1);
                pl.Add(top);

                pl.Add(px2);
                pl.Add(py2);
            }

            px1 = px2;
            py1 = py2;
        }

        return pl.ToArray();
    }

    /** Отсечение полигона по прямоугольнику */
    public static double[] ClipPolygon(IPrimitive g, Rect rect)
    {
        double[] res;

        if (g.Rect.Left < rect.Left)
            res = ClipLeft(g.Coords, rect.Left);
        else
            res = (double[])g.Coords.Clone();

        if (g.Rect.Bottom < rect.Bottom)
            res = ClipBottom(res, rect.Bottom);

        if (g.Rect.Right > rect.Right)
            res = ClipRight(res, rect.Right);

        if (g.Rect.Top > rect.Top)
            res = ClipTop(res, rect.Top);

        return res.ToArray();
    }
}

