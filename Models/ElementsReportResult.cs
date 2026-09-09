#nullable enable

namespace dLab.General.ElementsReport.Models;

using System.Collections.Generic;

/// <summary>
/// Результат построения ведомости: строки таблицы и суммарная площадь.
/// </summary>
public readonly struct ElementsReportResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ElementsReportResult"/> struct.
    /// </summary>
    /// <param name="rows">Строки ведомости.</param>
    /// <param name="totalArea">Суммарная площадь по строкам.</param>
    public ElementsReportResult(IReadOnlyList<ElementRow> rows, double totalArea)
    {
        Rows = rows;
        TotalArea = totalArea;
    }

    /// <summary>
    /// Строки ведомости.
    /// </summary>
    public IReadOnlyList<ElementRow> Rows { get; }

    /// <summary>
    /// Суммарная площадь по строкам ведомости.
    /// </summary>
    public double TotalArea { get; }
}
