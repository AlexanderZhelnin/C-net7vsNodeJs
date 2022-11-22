namespace drawer.Models;

/** Тип графического образа */
public enum GrTypeEnum
{
    /** Отсутствует */
    Empty = 0,
    /** Линия */
    Line = 1,
    /** Полигон */
    Polygon = 2,
    /** Условное обозначение */
    Block = 3,
    /** Кривая типа сплайн */
    Spline = 4,
    /** Кривая типа безье */
    Bezier = 5,
    /** Эллипс */
    Ellipse = 6,
    /** Сектор */
    Pie = 7,
    /** Путь(составной, может состоять из различных графических примитивов) */
    Path = 8,
    /** Замкнутый залитый путь */
    FillPath = 9,
    /** Надпись */
    Text = 10,
    /** Контрол */
    Visual = 12,
    /** Круг */
    Circle = 13
}

/** Стиль границы */
public enum BorderStyleEnum
{
    /** Простая */
    Solid = 0,
    /** Пунктирный */
    Dash = 1,
    /** Без границы */
    Transparent = 2,
    /** Текстурная */
    Textured = 3
}

/** Стиль заливки */
public enum FillStyleEnum
{
    /** Сплошная заливка */
    Solid = 1,
    /** Без заливки */
    Transparent = 2,
    /** Заливка с использованием градиентной заливки */
    LinearGradient = 3,
    /** Заливка с использованием текстуры */
    Textured = 4,
    /** Заливка с использованием штриховки */
    Hatch = 5
}

/** Ориентация для градиентной заливки */
public enum GradientStyle
{
    /** Горизонтальная - Слева на право */
    Left2Right,
    /** Горизонтальная - С право на лево */
    Right2Left,
    /** Вертикальная - Снизу вверх */
    Bottom2Top,
    /** Вертикальная - С верху вниз */
    Top2Bottom,
    /** Диагональная от нижней левой точки к правой верхней */
    LeftBottom2RightTop,
    /** Диагональная от правой верхней к нижней левой точки */
    RightTop2LeftBottom,
    /** Диагональная от правой нижней к верхней левой */
    RightBottom2LeftTop,
    /** Диагональная от верхней левой к правой нижней */
    LeftTop2RightBottom
}


/** Выравнивание текста */
public enum TextPositionEnum
{
    /** По умолчанию Автоматическое расположение текста с анализом */
    Default = 0,
    /** Слева внизу с анализом */
    LeftBottom = 1,
    /** Лева в центре по вертикали с анализом */
    LeftMiddle = 2,
    /** Слева вверху с анализом */
    LeftTop = 3,
    /** Вверху в центре по горизонтали с анализом */
    TopMiddle = 4,
    /** Сверху справа с анализом */
    TopRight = 5,
    /** Справа в центре по вертикали с анализом */
    RightMiddle = 6,
    /** Справа снизу с анализом */
    RightBottom = 7,
    /** Снизу в центре по вертикали с анализом */
    BottomMiddle = 8,
    /** Произвольное положение текста явно заданное с анализом */
    PositionSets = 9,

    /** Слева внизу с анализом */
    LeftBottomInner = -1,
    /** Лева в центре по вертикали с анализом */
    LeftMiddleInner = -2,
    /** Слева вверху с анализом */
    LeftTopInner = -3,
    /** Вверху в центре по горизонтали с анализом */
    TopMiddleInner = -4,
    /** Сверху справа с анализом */
    TopRightInner = -5,
    /** Справа в центре по вертикали с анализом */
    RightMiddleInner = -6,
    /** Справа снизу с анализом */
    RightBottomInner = -7,
    /** Снизу в центре по вертикали с анализом */
    BottomMiddleInner = -8,
}


public enum FontStyleEnum
{
    Regular = 0x0,

    Bold = 0x1,

    Italic = 0x2,

    Underline = 0x4,

    Strikeout = 0x8
}
/** Графические свойства условного обозначения */
public class ILegendBlock
{
    public Int64 id { get; set; }
    public double size { get; set; }
    public bool scaled { get; set; }
}

/** Легенда для заливки */
public class ILegendFill
{
    /** Цвет заливки 1 */
    public string color1 { get; set; }
    /** Цвет заливки 2 */
    public string color2 { get; set; }
    /** Масштабируемость заливки */
    public bool scaled { get; set; }
    /** Стиль заливки */
    public FillStyleEnum style { get; set; }
    // fillHatchStyle:
    /** Стиль Градиентной заливки */
    public GradientStyle gradientStyle { get; set; }
    public ILegendBlock block { get; set; }
}

/** Графические свойства границы */
public class ILegendBorder
{
    /** цвет */
    public string color { get; set; }
    public BorderStyleEnum style { get; set; }
    // dashStyle
    // startCap
    // endCap
    // dashCap
    /** Ориентация блока для границы */
    //Orientated
    /** Масштабируемость границы */
    public bool scaled { get; set; }
    public double size { get; set; }
}

/** Графические свойства шрифта */
public class ILegendFont
{
    public string family { get; set; }
    public double size { get; set; }
    //weight: number;
    public FontStyleEnum style { get; set; }
}

/** Графические свойства надписи */
public class ILegendText
{
    /** Диапазон видимости */
    public IMashtabRange mashtabRange { get; set; }
    /** Опорный масштаб */
    public double mashtabBase { get; set; }
    /** Масштабируемость */
    public bool scaled { get; set; }
    /** Положение */
    public TextPositionEnum position { get; set; }
    /** Цвет */
    public string color { get; set; }
    /** Цвет фона */
    public string backColor { get; set; }
    /** Шрифт */
    public ILegendFont font { get; set; }
    public bool isAnalyze { get; set; }
}

/** Графические свойства */
public class ILegend
{
    /** Уникальный идентификатор */
    public Int64 id { get; set; }
    /** Тип графического образа */
    public GrTypeEnum type { get; set; }
    /** Диапазон видимости */
    public IMashtabRange mashtabRange { get; set; }
    /** Приоритет */
    public int priority { get; set; }
    /** Условное обозначение */
    public ILegendBlock block { get; set; }
    /** Заливка */
    public ILegendFill fill { get; set; }
    /** Граница */
    public ILegendBorder border { get; set; }
    /** Надпись */
    public ILegendText text { get; set; }
    /** Графические примитивы */
    public IPrimitive[] primitives { get; set; }
}
