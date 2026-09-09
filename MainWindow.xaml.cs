#nullable enable

namespace dLab.General.ElementsReport;

using System.Windows;
using System.Windows.Input;
using dLab.General.ElementsReport.ViewModels;

/// <summary>
/// Главное окно модуля. Логики не содержит, данные получает через привязки к
/// <see cref="MainWindowVm"/>.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    /// <param name="viewModel">Модель представления окна.</param>
    public MainWindow(MainWindowVm viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    /// <summary>
    /// Закрывает окно при нажатии клавиши Escape.
    /// </summary>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Key.Escape)
        {
            Close();
        }
    }
}