#nullable enable

namespace dLab.General.ElementsReport.Services;

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using dLab.General.ElementsReport.Abstractions;
using dLab.General.ElementsReport.Helpers;
using dLab.General.ElementsReport.Models;

/// <inheritdoc />
public class ElementsReportService : IElementsReportService
{
    /// <summary>
    /// Документ, по которому строится ведомость.
    /// </summary>
    private readonly Document _document;

    /// <summary>
    /// Creates the report building service.
    /// </summary>
    /// <param name="document">Документ открытой модели.</param>
    public ElementsReportService(Document document)
    {
        _document = document ?? throw new ArgumentNullException(nameof(document));
    }

    /// <inheritdoc />
    public ElementsReportResult BuildReport(BuiltInCategory category)
    {
        var rows = new List<ElementRow>();
        double totalArea = 0;

        foreach (var element in CollectElements(category))
        {
            var area = GetArea(element);
            if (area <= 0)
                continue;

            // Переводим из внутренних единиц Revit (кв. футы) в квадратные метры
            var areaInSquareMeters = UnitHelper.ToSquareMeters(area);

            // Получаем название уровня
            var levelName = GetLevelName(element);

            rows.Add(new ElementRow(element.Name, areaInSquareMeters, levelName));
            totalArea += areaInSquareMeters;
        }

        return new ElementsReportResult(rows, totalArea);
    }

    /// <summary>
    /// Возвращает элементы модели заданной категории, попадающие в ведомость.
    /// </summary>
    /// <param name="category">Выбранная категория Revit.</param>
    /// <returns>Экземпляры элементов заданной категории.</returns>
    private IEnumerable<Element> CollectElements(BuiltInCategory category) =>
        new FilteredElementCollector(_document)
            .OfCategory(category)
            .WhereElementIsNotElementType()
            .ToElements();

    /// <summary>
    /// Возвращает площадь элемента во внутренних единицах Revit.
    /// </summary>
    /// <param name="element">Элемент модели.</param>
    /// <returns>Площадь во внутренних единицах или ноль, если параметр не заполнен.</returns>
    private double GetArea(Element element)
    {
        var parameter = element.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);
        if (parameter is null || !parameter.HasValue)
            return 0;

        return parameter.AsDouble();
    }

    /// <summary>
    /// Возвращает название уровня элемента или строку по умолчанию, если уровень отсутствует.
    /// </summary>
    /// <param name="element">Элемент модели.</param>
    /// <returns>Название уровня или локализованный текст при его отсутствии.</returns>
    private string GetLevelName(Element element)
    {
        // 1. Проверяем напрямую свойство LevelId
        if (element.LevelId != null && element.LevelId != ElementId.InvalidElementId)
        {
            if (_document.GetElement(element.LevelId) is Level level && !string.IsNullOrEmpty(level.Name))
            {
                return level.Name;
            }
        }

        // 2. Если LevelId пуст, проверяем базовый параметр уровня
        var levelParam = element.get_Parameter(BuiltInParameter.FAMILY_LEVEL_PARAM)
                      ?? element.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM)
                      ?? element.get_Parameter(BuiltInParameter.ROOF_BASE_LEVEL_PARAM);

        if (levelParam != null && levelParam.HasValue)
        {
            var levelId = levelParam.AsElementId();
            if (levelId != null && levelId != ElementId.InvalidElementId)
            {
                if (_document.GetElement(levelId) is Level level && !string.IsNullOrEmpty(level.Name))
                {
                    return level.Name;
                }
            }
        }

        // Если уровень не найден — возвращаем текст «Без уровня»
        return Lang.ElementsReport.NoLevelText;
    }
}