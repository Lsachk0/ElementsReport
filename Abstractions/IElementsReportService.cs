#nullable enable

namespace dLab.General.ElementsReport.Abstractions;

using Autodesk.Revit.DB;
using dLab.General.ElementsReport.Models;

/// <summary>
/// Построение ведомости по элементам открытой модели.
/// Единственное место модуля, которое обращается к документу Revit.
/// </summary>
public interface IElementsReportService
{
    /// <summary>
    /// Собирает элементы модели и считает ведомость.
    /// Метод обязан выполняться на потоке Revit API.
    /// </summary>
    /// <param name="category">Категория элементов Revit (Стены, Перекрытия, Крыши и т.д.).</param>
    /// <returns>Строки ведомости и суммарная площадь.</returns>
    ElementsReportResult BuildReport(BuiltInCategory category);
}
