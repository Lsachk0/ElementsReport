#nullable enable

namespace dLab.General.ElementsReport.Helpers;

using Autodesk.Revit.DB;

/// <summary>
/// Пересчёт значений из внутренних единиц Revit в единицы отображения.
/// </summary>
public static class UnitHelper
{
    /// <summary>
    /// Переводит площадь из внутренних единиц Revit в квадратные метры.
    /// Внутри Revit площади хранятся в квадратных футах.
    /// </summary>
    /// <param name="internalValue">Площадь во внутренних единицах Revit.</param>
    /// <returns>Площадь в квадратных метрах.</returns>
    public static double ToSquareMeters(double internalValue) =>
        UnitUtils.ConvertFromInternalUnits(internalValue, UnitTypeId.SquareMeters);
}
