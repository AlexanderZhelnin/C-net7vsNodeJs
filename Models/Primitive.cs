namespace drawer.Models;

/** Приметив графического образа */
public class IPrimitive
{
    /** Координаты приметива  */
    public double[] Coords { get; set; } = null!;
    public double TextCoordX { get; set; }
    public double TextCoordY { get; set; }
    /** Угол поворота */
    public double TextAngle { get; set; }
    /** Описывающий прямоугольник */
    public Rect Rect { get; set; }
    /** Имя */
    public string Name { get; set; } = null!;
}
