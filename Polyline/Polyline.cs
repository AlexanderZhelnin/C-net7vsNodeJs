using drawer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawer;

public static class Poliline
{
    private static List<double[]> ClipLeft(double[] coords, double left)
    {
        var res = new List<double[]>();
        if (coords.Length == 0) return res;

        var pl = new List<double>();

        var px1 = coords[0];
        var py1 = coords[1];

        if (px1 >= left)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        for (var i = 2; i < coords.Length; i += 2)
        {
            var px2 = coords[i];
            var py2 = coords[i + 1];

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

                res.Add(pl.ToArray());
                pl = new List<double>();

            }
            px1 = px2;
            py1 = py2;
        }
        if (pl.Count > 0) res.Add(pl.ToArray());
        return res;
    }
    private static List<double[]> ClipRight(double[] coords, double right)
    {
        var res = new List<double[]>();
        if (coords.Length == 0) return res;

        var pl = new List<double>();

        var px1 = coords[0];
        var py1 = coords[1];

        if (px1 <= right)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        for (var i = 2; i < coords.Length; i += 2)
        {
            var px2 = coords[i];
            var py2 = coords[i + 1];

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
                // pl.Add(px1);
                // pl.Add(py1);

                pl.Add(right);
                pl.Add((right - px1) * (py2 - py1) / (px2 - px1) + py1);

                res.Add(pl.ToArray());

                pl = new List<double>();
            }
            px1 = px2;
            py1 = py2;
        }
        if (pl.Count > 0) res.Add(pl.ToArray());
        return res;
    }
    private static double[][] ClipBottom(double[] coords, double bottom)
    {
        var res = new List<double[]>();
        if (coords.Length == 0) return res.ToArray();

        var pl = new List<double>();

        var px1 = coords[0];
        var py1 = coords[1];

        if (py1 >= bottom)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        for (var i = 2; i < coords.Length; i += 2)
        {

            var px2 = coords[i];
            var py2 = coords[i + 1];

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
                pl.Add(px1);
                pl.Add(py1);

                pl.Add((bottom - py1) * (px2 - px1) / (py2 - py1) + px1);
                pl.Add(bottom);

                res.Add(pl.ToArray());

                pl = new List<double>();
            }
            px1 = px2;
            py1 = py2;
        }
        if (pl.Count > 0) res.Add(pl.ToArray());
        return res.ToArray();
    }
    private static List<double[]> ClipTop(double[] coords, double top)
    {
        var res = new List<double[]>();
        if (coords.Length == 0) return res;

        var pl = new List<double>();

        var px1 = coords[0];
        var py1 = coords[1];

        if (py1 <= top)
        {
            pl.Add(px1);
            pl.Add(py1);
        }


        for (var i = 2; i < coords.Length; i += 2)
        {
            var px2 = coords[i];
            var py2 = coords[i + 1];

            if (py1 <= top && py2 <= top)
            {
                pl.Add(px2);
                pl.Add(py2);
            }
            else if (py1 < top && py2 > top)
            {
                pl.Add(px1);
                pl.Add(py1);

                pl.Add((top - py1) * (px2 - px1) / (py2 - py1) + px1);
                pl.Add(top);
                
                res.Add(pl.ToArray());
                pl = new List<double>();
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
        if (pl.Count > 0) res.Add(pl.ToArray());
        return res;
    }


    /** Отсечение полилинии по прямоугольнику */
    public static List<double[]> ClipPolyline(IPrimitive g, Rect rect)
    {
        var res=new List<double[]>();

        if (g.rect.left < rect.left)
            res = ClipLeft(g.coords, rect.left);
        else
            res.Add((double[])g.coords.Clone());

        if (g.rect.bottom < rect.bottom)
        {
            var tmp = new List<double[]>();
            foreach (var cs in res)
                tmp.AddRange(ClipBottom(cs, rect.bottom));
            res = tmp;
        }

        if (g.rect.right > rect.right)
        {
            var tmp = new List<double[]>();
            foreach (var cs in res)
                tmp.AddRange(ClipRight(cs, rect.right));
            res = tmp;
        }

        if (g.rect.top > rect.top)
        {
            var tmp = new List<double[]>();
            foreach (var cs in res)
                tmp.AddRange(ClipTop(cs, rect.top));
            res = tmp;
        }

        return res;
    }


}

