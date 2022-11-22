using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawer.Models;

/** Приметив графического образа */
public class IPrimitive
{
    /** Координаты приметива  */
    public double[] coords { get; set; }
    public double textCoordX { get; set; }
    public double textCoordY { get; set; }
    /** Угол поворота */
    public double textAngle { get; set; }
    /** Описывающий прямоугольник */
    public Rect rect { get; set; }
    /** Имя */
    public string name { get; set; }
}

