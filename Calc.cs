using drawer.Models;

namespace drawer;

internal class Calc
{

    /** Преобразование в систему координат экрана */
    public static void Translate(List<double> cs, DrawProperties pr)
    {
        for (var i = 0; i < cs.Count; i += 2)
        {
            cs[i] = (cs[i] - pr.left) * pr.scale;
            cs[i + 1] = (pr.top - cs[i + 1]) * pr.scale;
        }
    }

    public static void Translate(double[] cs, DrawProperties pr)
    {
        for (var i = 0; i < cs.Length; i += 2)
        {
            cs[i] = (cs[i] - pr.left) * pr.scale;
            cs[i + 1] = (pr.top - cs[i + 1]) * pr.scale;
        }
    }

    /** Преобразование в систему координат экрана для описывающего прямоугольника */
    public static void TranslateRect(Rect r, DrawProperties pr)
    {
        r.left = (r.left - pr.left) * pr.scale;
        r.right = (r.right - pr.left) * pr.scale;

        r.bottom = (pr.top - r.bottom) * pr.scale;
        r.top = (pr.top - r.top) * pr.scale;
    }

    /** Преобразование в систему координат экрана для текста */
    public static void translateText(IText txt, DrawProperties pr)
    {
        Translate(txt.coords, pr);
        TranslateRect(txt.rect, pr);
        txt.x = (txt.x - pr.left) * pr.scale;
        txt.y = (pr.top - txt.y) * pr.scale;
    }

    /** Вычислить описывающий прямоугольник для координат */
    public static Rect CalcRect(List<double> coords)
    {

        var left = coords[0];
        var bottom = coords[1];

        var right = coords[0];
        var top = coords[1];

        for (var i = 2; i < coords.Count; i += 2)
        {
            var x = coords[i];
            var y = coords[i + 1];

            if (left > x) left = x;
            if (bottom > y) bottom = y;

            if (right < x) right = x;
            if (top < y) top = y;
        }

        return new Rect { left = left, bottom = bottom, right = right, top = top };
    }

    public static Rect CalcRect(double[] coords)
    {

        var left = coords[0];
        var bottom = coords[1];

        var right = coords[0];
        var top = coords[1];

        for (var i = 2; i < coords.Length; i += 2)
        {
            var x = coords[i];
            var y = coords[i + 1];

            if (left > x) left = x;
            if (bottom > y) bottom = y;

            if (right < x) right = x;
            if (top < y) top = y;
        }

        return new Rect { left = left, bottom = bottom, right = right, top = top };
    }

    /** Вычислить максимальный отрезок */
    public static int CalcMaxLen(double[] coords)
    {
        var result = 0;
        var x1 = coords[0];
        var y1 = coords[1];
        var max = 0.0;


        for (var i = 2; i < coords.Length; i += 2)
        {

            var x2 = coords[i];
            var y2 = coords[i + 1];

            var dx = x2 - x1;
            var dy = y2 - y1;

            var l = Math.Sqrt(dx * dx + dy * dy);
            if (max < l)
            {
                max = l;
                result = i - 2;
            }

        }

        return result;
    }

    /** Удаление точек которые не будут отображаться */
    public static double[] Optimize(double[] mas, double l = 1)
    {
        var count = mas.Length;
        if (count < 60) return mas;

        var coords = new List<double>();
        var lastCoordX1 = mas[0];
        var lastCoordY1 = mas[1];
        var lastCoordX2 = mas[2];
        var lastCoordY2 = mas[3];
        coords.Add(lastCoordX1);
        coords.Add(lastCoordY1);

        for (var i = 4; i < count; i += 2)
            if (!isPointOnLine(lastCoordX1, lastCoordY1, lastCoordX2, lastCoordY2, mas[i], mas[i + 1], l))
            {
                lastCoordX1 = mas[i - 2];
                lastCoordY1 = mas[i - 1];

                lastCoordX2 = mas[i];
                lastCoordY2 = mas[i + 1];
                coords.Add(lastCoordX1);
                coords.Add(lastCoordY1);
            }

        coords.Add(mas[count - 2]);
        coords.Add(mas[count - 1]);

        return coords.ToArray();
    }

    /** Находится ли следующая точка на линии с определённым допуском */
    public static bool isPointOnLine(double pX1, double pY1, double pX2, double pY2, double pX3, double pY3, double l)
    {
        if ((pX3 == pX1 && pX3 == pY1) || (pX3 == pX1 && pX3 == pY1))
            return true;

        var aX = pX2 - pX1;
        var aY = pY2 - pY1;
        // вектор повёрнутый на 90
        var pX4 = -aY + pX3;
        var pY4 = aX + pY3;

        var retX = 0.0;
        var retY = 0.0;
        if (pX2 == pX1)
        {
            retX = pX1;
            if (pY4 == pY3)
                retY = pY3;
            else if (pX4 != pX3)
                retY = (pX1 - pX3) * (pY4 - pY3) / (pX4 - pX3) + pY3;
            else
            {
                //console.log('');
            }
        }
        else if (pY2 == pY1)
        {
            retY = pY1;
            if (pX4 == pX3)
                retX = pX3;
            else if (pY4 != pY3)
                retX = (pY1 - pY3) * (pX4 - pX3) / (pY4 - pY3) + pX3;
            else
            {
                //console.log('');
            }
        }
        else if (pX4 == pX3)
        {
            retX = pX3;
            retY = (pX3 - pX1) * (pY2 - pY1) / (pX2 - pX1) + pY1;

        }
        else if (pY4 == pY3)
        {
            retY = pY3;
            retX = (pY3 - pY1) * (pX2 - pX1) / (pY2 - pY1) + pX1;

        }
        else
        {
            var k1 = (pY2 - pY1) / (pX2 - pX1);
            var k2 = (pY4 - pY3) / (pX4 - pX3);

            retX = (k1 * pX1 - k2 * pX3 + pY3 - pY1) / (k1 - k2);
            retY = (retX - pX1) * k1 + pY1;
        }

        retX -= pX3;
        retY -= pY3;


        return Math.Sqrt(retX * retX + retY * retY) < l;

    }

    /** Преобразование из коэффициента в масштаб */
    public static double scale2Mashtab(double scale)
    {
        return (144 * 1000 / 25.4) / scale;
    }

    /** Преобразование из масштаба в коэффициент */
    public static double mashtab2Scale(double scale)
    {
        return scale * (144 * 1000 / 25.4);
    }

}
