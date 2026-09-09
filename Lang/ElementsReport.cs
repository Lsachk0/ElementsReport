#nullable enable

namespace dLab.General.ElementsReport.Lang;

using System.Globalization;
using System.Reflection;
using System.Resources;

/// <summary>
/// Строки локализации модуля.
/// Значения берутся из файлов ресурсов в папке <c>Lang</c> по текущей культуре интерфейса.
/// </summary>
public static class ElementsReport
{
    /// <summary>
    /// Менеджер ресурсов модуля.
    /// </summary>
    private static readonly ResourceManager ResourceManager = new ResourceManager(
        "dLab.General.ElementsReport.Lang.ElementsReport",
        Assembly.GetExecutingAssembly());

    /// <summary>
    /// Заголовок главного окна.
    /// </summary>
    public static string WindowTitle => GetString(nameof(WindowTitle));

    /// <summary>
    /// Заголовок над таблицей ведомости.
    /// </summary>
    public static string ElementsHeading => GetString(nameof(ElementsHeading));

    /// <summary>
    /// Заголовок колонки с именем типа элемента.
    /// </summary>
    public static string TypeColumn => GetString(nameof(TypeColumn));

    /// <summary>
    /// Заголовок колонки с площадью.
    /// </summary>
    public static string AreaColumn => GetString(nameof(AreaColumn));

    /// <summary>
    /// Подпись перед суммарной площадью.
    /// </summary>
    public static string TotalLabel => GetString(nameof(TotalLabel));

    /// <summary>
    /// Обозначение квадратных метров.
    /// </summary>
    public static string SquareMetersSuffix => GetString(nameof(SquareMetersSuffix));

    /// <summary>
    /// Сообщение о том, что команда запущена без открытой модели.
    /// </summary>
    public static string NoActiveDocumentMessage => GetString(nameof(NoActiveDocumentMessage));
    
    /// <summary>
    /// Название категории стен.
    /// </summary>
    public static string WallsCategory => GetString(nameof(WallsCategory));

    /// <summary>
    /// Название категории перекрытий.
    /// </summary>
    public static string FloorsCategory => GetString(nameof(FloorsCategory));

    /// <summary>
    /// Название категории крыш.
    /// </summary>
    public static string RoofsCategory => GetString(nameof(RoofsCategory));
    public static string LevelColumn => GetString(nameof(LevelColumn));
    public static string NoLevelText => GetString(nameof(NoLevelText));

    /// <summary>
    /// Возвращает строку ресурса по ключу для текущей культуры интерфейса.
    /// </summary>
    /// <param name="key">Ключ ресурса.</param>
    /// <returns>Строка ресурса или пустая строка, если ключ не найден.</returns>
    private static string GetString(string key) =>
        ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? string.Empty;
}
