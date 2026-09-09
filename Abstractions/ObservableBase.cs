#nullable enable

namespace dLab.General.ElementsReport.Abstractions;

using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
/// База для моделей представления и observable-моделей модуля.
/// Уведомления об изменении auto-property внедряет Fody, вызывать
/// <see cref="OnPropertyChanged"/> руками нужно только для вычисляемых свойств.
/// </summary>
public abstract class ObservableBase : INotifyPropertyChanged
{
    /// <summary>
    /// Возникает при изменении значения свойства.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Уведомляет интерфейс об изменении свойства.
    /// </summary>
    /// <param name="propertyName">Имя свойства, подставляется компилятором.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
