#nullable enable

namespace dLab.General.ElementsReport.Commands;

using System;
using System.Windows.Input;

/// <summary>
/// Синхронная команда. Исключение внутри обработчика не улетает в WPF,
/// а уходит в переданный обработчик ошибок.
/// </summary>
public class RelayCommand : ICommand
{
    /// <summary>
    /// Действие, выполняемое командой.
    /// </summary>
    private readonly Action _execute;

    /// <summary>
    /// Условие доступности команды.
    /// </summary>
    private readonly Func<bool>? _canExecute;

    /// <summary>
    /// Обработчик исключения, возникшего при выполнении команды.
    /// </summary>
    private readonly Action<Exception>? _onError;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="execute">Действие, выполняемое командой.</param>
    /// <param name="canExecute">Условие доступности команды.</param>
    /// <param name="onError">
    /// Обработчик исключения. Если не передан, исключение пробрасывается вызывающему коду.
    /// </param>
    public RelayCommand(Action execute, Func<bool>? canExecute = null, Action<Exception>? onError = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
        _onError = onError;
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <inheritdoc />
    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        try
        {
            _execute();
        }
        catch (Exception exception)
        {
            if (_onError is null)
                throw;

            _onError(exception);
        }
    }
}
