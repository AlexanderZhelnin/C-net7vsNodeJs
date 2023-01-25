using drawer.Models;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace drawer;

internal class Calc
{
    public static Vector<double> NegY = new(new double[] { 1, -1, 1, -1 });
    /** Преобразование в систему координат экрана */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Translate(double[] cs, DrawProperties1 pr)
    {
        var count = cs.Length - cs.Length % 4;

        for (var i = 0; i < count; i += 4)
        {
            var v = new Vector<double>(cs, i);
            var result = (v - pr.LeftTop) * NegY * pr.Scale;
            result.CopyTo(cs, i);
        }

        if (count >= cs.Length) return;

        var left = pr.LeftTop[0];
        var top = pr.LeftTop[1];
        for (var i = count; i < cs.Length; i += 2)
        {
            cs[i] = (cs[i] - left) * pr.Scale;
            cs[i + 1] = (top - cs[i + 1]) * pr.Scale;
        }
    }
          
    /** Удаление точек которые не будут отображаться */
    public static double[] Optimize(double[] mas, double l = 1)
    {
        var count = mas.Length;
        if (count < 5) return mas;

        var coords = new List<double>(mas.Length);

        var (lastCoordX1, lastCoordY1) = (mas[0], mas[1]);
        var (lastCoordX2, lastCoordY2) = (mas[2], mas[3]);

        coords.Add(lastCoordX1);
        coords.Add(lastCoordY1);

        for (var i = 4; i < count; i += 2)
            if (!IsPointOnLine(lastCoordX1, lastCoordY1, lastCoordX2, lastCoordY2, mas[i], mas[i + 1], l))
            {
                (lastCoordX1, lastCoordY1) = (mas[i - 2], mas[i - 1]);
                (lastCoordX2, lastCoordY2) = (mas[i], mas[i + 1]);

                coords.Add(lastCoordX1);
                coords.Add(lastCoordY1);
            }

        coords.Add(mas[count - 2]);
        coords.Add(mas[count - 1]);

        return coords.ToArray();
    }

    //public static bool isPointOnLine1(double pX1, double pY1, double pX2, double pY2, double pX, double pY, double l)
    //{
    //    var a = pX - pX1;
    //    var b = pY - pY1;

    //    var c = pX2 - pX1;
    //    var d = pY2 - pY1;

    //    var lenSQ = c * c + d * d;

    //    var param = (lenSQ != 0)
    //        ? (a * c + b * d) / lenSQ
    //        : -1.0;

    //    double xx, yy;

    //    if (param < 0)
    //        (xx, yy) = (pX1, pY1);
    //    else if (param > 1)
    //        (xx, yy) = (pX2, pY2);
    //    else
    //        (xx, yy) = (pX1 + param * c, pY1 + param * d);

    //    var (dx, dy) = (pX - xx, pY - yy);
    //    return Math.Sqrt(dx * dx + dy * dy) < l;
    //}

    /** Находится ли следующая точка на линии с определённым допуском */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPointOnLine(double pX1, double pY1, double pX2, double pY2, double pX, double pY, double l)
    {
        var vP = Vector128.Create(pX, pY);
        var vP1 = Vector128.Create(pX1, pY1);
        var vP2 = Vector128.Create(pX2, pY2);

        var ab = Sse41.Subtract(vP, vP1);
        var cd = Sse41.Subtract(vP2, vP1);

        var lenSQ = Sse41.DotProduct(cd, cd, 255)[0];//Vector128.Dot(cd, cd);

        var param = (lenSQ != 0)            
            ? Sse41.DotProduct(ab, cd, 255)[0] / lenSQ
            : -1.0f;

        var dP = Sse41.Subtract(vP, Sse41.Add(vP1, Sse41.Multiply(cd, Vector128.Create(param))));


        return Math.Sqrt(Sse41.DotProduct(dP, dP, 255)[0]) < l;

        //var (a, b) = (pX - pX1, pY - pY1);
        //var (c, d) = (pX2 - pX1, pY2 - pY1);

        //var lenSQ = c * c + d * d;
        //if (lenSQ == 0) return true;

        //var param = (a * c + b * d) / lenSQ;

        //double xx, yy;

        //(xx, yy) = (pX1 + param * c, pY1 + param * d);

        //var (dx, dy) = (pX - xx, pY - yy);
        //return Math.Sqrt(dx * dx + dy * dy) < l;

        //if ((pX3 == pX1 && pX3 == pY1) || (pX3 == pX1 && pX3 == pY1))
        //    return true;

        //var aX = pX2 - pX1;
        //var aY = pY2 - pY1;
        //// вектор повёрнутый на 90
        //var pX4 = -aY + pX3;
        //var pY4 = aX + pY3;

        //var retX = 0.0;
        //var retY = 0.0;
        //if (pX2 == pX1)
        //{
        //    retX = pX1;
        //    if (pY4 == pY3)
        //        retY = pY3;
        //    else if (pX4 != pX3)
        //        retY = (pX1 - pX3) * (pY4 - pY3) / (pX4 - pX3) + pY3;
        //}
        //else if (pY2 == pY1)
        //{
        //    retY = pY1;
        //    if (pX4 == pX3)
        //        retX = pX3;
        //    else if (pY4 != pY3)
        //        retX = (pY1 - pY3) * (pX4 - pX3) / (pY4 - pY3) + pX3;
        //}
        //else if (pX4 == pX3)
        //{
        //    retX = pX3;
        //    retY = (pX3 - pX1) * (pY2 - pY1) / (pX2 - pX1) + pY1;

        //}
        //else if (pY4 == pY3)
        //{
        //    retY = pY3;
        //    retX = (pY3 - pY1) * (pX2 - pX1) / (pY2 - pY1) + pX1;

        //}
        //else
        //{
        //    var k1 = (pY2 - pY1) / (pX2 - pX1);
        //    var k2 = (pY4 - pY3) / (pX4 - pX3);

        //    retX = (k1 * pX1 - k2 * pX3 + pY3 - pY1) / (k1 - k2);
        //    retY = (retX - pX1) * k1 + pY1;
        //}

        //retX -= pX3;
        //retY -= pY3;


        //return Math.Sqrt(retX * retX + retY * retY) < l;
    }
}
