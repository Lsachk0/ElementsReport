#nullable enable

namespace dLab.General.ElementsReport;

using System;
using System.Diagnostics;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using dLab.General.ElementsReport.Services;
using dLab.General.ElementsReport.ViewModels;
using JetBrains.Annotations;

/// <summary>
/// Точка входа модуля: открывает окно ведомости по элементам активной модели.
/// </summary>
[Transaction(TransactionMode.Manual)]
[UsedImplicitly]
public class Command : IExternalCommand
{
    /// <summary>
    /// Открытое окно модуля. Повторный вызов команды не создаёт второе окно,
    /// а активирует уже открытое.
    /// </summary>
    private static MainWindow? _mainView;

    /// <inheritdoc />
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            if (_mainView is not null)
            {
                _mainView.Activate();
                _mainView.Focus();

                return Result.Succeeded;
            }

            var uiDocument = commandData.Application.ActiveUIDocument;
            if (uiDocument is null)
            {
                message = Lang.ElementsReport.NoActiveDocumentMessage;

                return Result.Failed;
            }

            ShowMainView(uiDocument.Document);

            return Result.Succeeded;
        }
        catch (Exception exception)
        {
            message = exception.Message;

            return Result.Failed;
        }
    }

    /// <summary>
    /// Собирает модель представления и показывает главное окно модуля.
    /// </summary>
    /// <param name="document">Документ активной модели.</param>
    private static void ShowMainView(Document document)
    {
        var reportService = new ElementsReportService(document);
        var viewModel = new MainWindowVm(reportService);

        _mainView = new MainWindow(viewModel);
        _mainView.Closed += HandleMainViewClosed;

        var interop = new WindowInteropHelper(_mainView);
        interop.Owner = Process.GetCurrentProcess().MainWindowHandle;

        _mainView.ShowDialog();
    }

    /// <summary>
    /// Освобождает ссылку на закрытое окно, чтобы следующий вызов команды открыл новое.
    /// </summary>
    /// <param name="sender">Закрытое окно.</param>
    /// <param name="e">Аргументы события.</param>
    private static void HandleMainViewClosed(object? sender, EventArgs e)
    {
        if (_mainView is not null)
        {
            _mainView.Closed -= HandleMainViewClosed;
            _mainView = null; // Обнуляем ссылку, чтобы следующий вызов создал новое окно
        }
    }
}