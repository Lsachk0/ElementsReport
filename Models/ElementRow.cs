#nullable enable

namespace dLab.General.ElementsReport.Models;

/// <summary>
/// Строка ведомости: один элемент модели, его площадь и уровень.
/// </summary>
public class ElementRow
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ElementRow"/> class.
    /// </summary>
    /// <param name="name">Имя типа элемента.</param>
    /// <param name="areaSqM">Площадь элемента в квадратных метрах.</param>
    /// <param name="levelName">Название уровня, на котором расположен элемент.</param>
    public ElementRow(string name, double areaSqM, string levelName)
    {
        Name = name;
        AreaSqM = areaSqM;
        LevelName = levelName;
    }

    /// <summary>
    /// Имя типа элемента.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Площадь элемента в квадратных метрах.
    /// </summary>
    public double AreaSqM { get; }

    /// <summary>
    /// Название уровня, на котором расположен элемент.
    /// </summary>
    public string LevelName { get; }
}