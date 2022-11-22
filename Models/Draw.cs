using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawer.Models;

public class DrawProperties
{
    /** Левый-верхний угол отрисовки */
    public double left { get; set; }
    public double top { get; set; }
    /** Коэффициент масштабирования */
    public double scale { get; set; }
    /** Картографический масштаб, например 1:500 mashtab = 500 */
    public double mashtab { get; set; }
}

public class IText
{
    /** Текст вывода */
    public string text { get; set; }
    /** Точка вставки по X */
    public double x { get; set; }
    /** Точка вставки по Y */
    public double y { get; set; }
    /** Угол поворота */
    public double angle { get; set; }
    /** Описывающий прямоугольник */
    public Rect rect { get; set; }
    /** Координаты прямоугольника в который вписан текст */
    public double[] coords { get; set; }
}

/** Результирующий слой для отображения */
public class ILayer
{
    /** Уникальный идентификатор */
    public Int64 legendId { get; set; }
    /** Координаты для отрисовки */
    public double[][] coords { get; set; }
    /** Шрифт */
    // public string font { get; set; }
    /** Тексты */
    // public List<IText> texts { get; set; }
}
/** Данные для отображения */
public class IObraz
{
    public double[] coords { get; set; }
    public IText? text { get; set; }

}
