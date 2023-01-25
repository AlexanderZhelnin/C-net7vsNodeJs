using drawer.Models;

namespace drawer;

public static class Poliline
{
    private static List<double[]> ClipLeft(double[] coords, double left)
    {
        var res = new List<double[]>();
        if (coords.Length == 0) return res;

        var pl = new List<double>(coords.Length * 2);

        var (px1, py1) = (coords[0], coords[1]);

        if (px1 >= left)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        for (var i = 2; i < coords.Length; i += 2)
        {
            var (px2, py2) = (coords[i], coords[i + 1]);

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
                pl.Add(left);
                pl.Add((left - px1) * (py2 - py1) / (px2 - px1) + py1);

                res.Add(pl.ToArray());
                pl = new List<double>(coords.Length * 2);

            }
            (px1, py1) = (px2, py2);
        }
        if (pl.Count > 0) res.Add(pl.ToArray());
        return res;
    }
    private static List<double[]> ClipRight(double[] coords, double right)
    {
        var res = new List<double[]>();
        if (coords.Length == 0) return res;

        var pl = new List<double>(coords.Length * 2);

        var (px1, py1) = (coords[0], coords[1]);

        if (px1 <= right)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        for (var i = 2; i < coords.Length; i += 2)
        {
            var (px2, py2) = (coords[i], coords[i + 1]);

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

                pl = new List<double>(coords.Length * 2);
            }
            (px1, py1) = (px2, py2);

        }
        if (pl.Count > 0) res.Add(pl.ToArray());
        return res;
    }
    private static List<double[]> ClipBottom(double[] coords, double bottom)
    {
        var res = new List<double[]>();
        if (coords.Length == 0) return res;

        var pl = new List<double>(coords.Length * 2);

        var (px1, py1) = (coords[0], coords[1]);

        if (py1 >= bottom)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        for (var i = 2; i < coords.Length; i += 2)
        {
            var (px2, py2) = (coords[i], coords[i + 1]);

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

                pl = new List<double>(coords.Length * 2);
            }
            (px1, py1) = (px2, py2);
        }
        if (pl.Count > 0) res.Add(pl.ToArray());
        return res;
    }
    private static List<double[]> ClipTop(double[] coords, double top)
    {
        var res = new List<double[]>();
        if (coords.Length == 0) return res;

        var pl = new List<double>(coords.Length * 2);

        var (px1, py1) = (coords[0], coords[1]);

        if (py1 <= top)
        {
            pl.Add(px1);
            pl.Add(py1);
        }

        for (var i = 2; i < coords.Length; i += 2)
        {
            var (px2, py2) = (coords[i], coords[i + 1]);

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
                pl = new List<double>(coords.Length * 2);
            }
            else if (py1 > top && py2 < top)
            {
                pl.Add((top - py1) * (px2 - px1) / (py2 - py1) + px1);
                pl.Add(top);

                pl.Add(px2);
                pl.Add(py2);


            }
            (px1, py1) = (px2, py2);
        }
        if (pl.Count > 0) res.Add(pl.ToArray());
        return res;
    }


    /** Отсечение полилинии по прямоугольнику */
    public static List<double[]> ClipPolyline(IPrimitive g, Rect rect)
    {
        var res = (g.Rect.Left < rect.Left)
            ? ClipLeft(g.Coords, rect.Left)
            : new List<double[]> { (double[])g.Coords.Clone() };

        if (g.Rect.Bottom < rect.Bottom)
        {
            var tmp = new List<double[]>(res.Count * 2);
            foreach (var cs in res)
                tmp.AddRange(ClipBottom(cs, rect.Bottom));
            res = tmp;
        }

        if (g.Rect.Right > rect.Right)
        {
            var tmp = new List<double[]>(res.Count * 2);
            foreach (var cs in res)
                tmp.AddRange(ClipRight(cs, rect.Right));
            res = tmp;
        }

        if (g.Rect.Top > rect.Top)
        {
            var tmp = new List<double[]>(res.Count * 2);
            foreach (var cs in res)
                tmp.AddRange(ClipTop(cs, rect.Top));
            res = tmp;
        }

        return res;
    }
}

