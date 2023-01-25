using System.Numerics;

namespace drawer.Models;

public struct DrawProperties
{
    /** Левый-верхний угол отрисовки */
    public double Left { get; set; }
    public double Top { get; set; }
    /** Коэффициент масштабирования */
    public double Scale { get; set; }
    /** Картографический масштаб, например 1:500 mashtab = 500 */
    public double Mashtab { get; set; }
}

public struct DrawProperties1
{
    public Vector<double> LeftTop { get; set; }
    /** Коэффициент масштабирования */
    public double Scale { get; set; }
    /** Картографический масштаб, например 1:500 mashtab = 500 */
    public double Mashtab { get; set; }
}

/** Результирующий слой для отображения */
public class ILayer
{
    /** Уникальный идентификатор */
    public Int64 LegendId { get; set; }
    /** Координаты для отрисовки */
    public double[][] Coords { get; set; } = null!;
}
/** Данные для отображения */
public struct IObraz
{
    public double[] Coords { get; set; }
}
