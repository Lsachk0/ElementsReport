#nullable enable

namespace dLab.General.ElementsReport.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Autodesk.Revit.DB;
using dLab.General.ElementsReport.Abstractions;
using dLab.General.ElementsReport.Commands;
using dLab.General.ElementsReport.Models;

/// <summary>
/// Элемент выпадающего списка категорий.
/// </summary>
public class CategoryOption
{
    public string DisplayName { get; }
    public BuiltInCategory Category { get; }

    public CategoryOption(string displayName, BuiltInCategory category)
    {
        DisplayName = displayName;
        Category = category;
    }
}

/// <summary>
/// Модель представления главного окна: строки ведомости, итог и сообщение об ошибке.
/// О Revit не знает, данные получает от <see cref="IElementsReportService"/>.
/// </summary>
public class MainWindowVm : ObservableBase
{
    private readonly IElementsReportService _reportService;
    private ICommand? _initializeCommand;
    private CategoryOption _selectedCategory;
    private string _minAreaText = string.Empty;

    /// <summary>
    /// Creates the view model of the main window.
    /// </summary>
    /// <param name="reportService">Сервис построения ведомости.</param>
    public MainWindowVm(IElementsReportService reportService)
    {
        _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        Rows = new ObservableCollection<ElementRow>();

        // Доступные категории для выбора
        Categories = new List<CategoryOption>
        {
            new(Lang.ElementsReport.WallsCategory, BuiltInCategory.OST_Walls),
            new(Lang.ElementsReport.FloorsCategory, BuiltInCategory.OST_Floors),
            new(Lang.ElementsReport.RoofsCategory, BuiltInCategory.OST_Roofs)
        };

        // По умолчанию выбираем "Стены"
        _selectedCategory = Categories[0];
    }

    /// <summary>
    /// Список категорий для выпадающего списка.
    /// </summary>
    public IReadOnlyList<CategoryOption> Categories { get; }

    /// <summary>
    /// Выбранная пользователем категория.
    /// </summary>
    public CategoryOption SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory == value)
                return;

            _selectedCategory = value;
            OnPropertyChanged();
            Load(); // Перечитываем отчет при смене категории
        }
    }

    /// <summary>
    /// Текст фильтра минимальной площади, введённый пользователем.
    /// </summary>
    public string MinAreaText
    {
        get => _minAreaText;
        set
        {
            if (_minAreaText == value)
                return;

            _minAreaText = value;
            OnPropertyChanged();
            Load(); // Пересчитываем фильтрацию при изменении текста
        }
    }

    /// <summary>
    /// Строки ведомости.
    /// </summary>
    public ObservableCollection<ElementRow> Rows { get; }

    /// <summary>
    /// Команда загрузки данных, привязана к открытию окна.
    /// </summary>
    public ICommand InitializeCommand =>
        _initializeCommand ??= new RelayCommand(Load, onError: HandleError);

    /// <summary>
    /// Суммарная площадь по строкам ведомости в квадратных метрах.
    /// </summary>
    public double TotalArea { get; private set; }

    /// <summary>
    /// Текст сообщения об ошибке. Пустая строка означает, что ошибки нет.
    /// </summary>
    public string ErrorMessage { get; private set; } = string.Empty;

    /// <summary>
    /// Загружает ведомость и обновляет содержимое окна с учетом фильтра.
    /// </summary>
    private void Load()
    {
        ErrorMessage = string.Empty;
        Rows.Clear();

        var report = _reportService.BuildReport(SelectedCategory.Category);

        // Парсим ввод. Если введено не число (или запятая/точка), считаем порог = 0
        double minArea = 0;
        if (!string.IsNullOrWhiteSpace(MinAreaText))
        {
            // Подменяем точку на запятую для корректного парсинга
            var normalizedText = MinAreaText.Replace('.', ',');
            double.TryParse(normalizedText, out minArea);
        }

        // Фильтруем элементы
        var filteredRows = report.Rows.Where(r => r.AreaSqM >= minArea).ToList();

        foreach (var row in filteredRows)
            Rows.Add(row);

        // Считаем итог по отфильтрованному списку
        TotalArea = filteredRows.Sum(r => r.AreaSqM);
    }

    /// <summary>
    /// Показывает пользователю сообщение об ошибке, возникшей в команде.
    /// </summary>
    /// <param name="exception">Возникшее исключение.</param>
    private void HandleError(Exception exception) => ErrorMessage = exception.Message;
}