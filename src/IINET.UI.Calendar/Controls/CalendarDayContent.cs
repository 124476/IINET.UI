using System;
using Avalonia.Controls;

namespace IINET.UI.Calendar.Controls;

/// <summary>
/// Дополнительное содержимое определённого дня календаря.
/// </summary>
public sealed class CalendarDayContent
{
    /// <summary>
    /// Создаёт содержимое на основе модели или обычного значения.
    /// Для отображения используется Calendar.DayContentTemplate.
    /// </summary>
    public CalendarDayContent(DateTime date, object? content)
    {
        Date = date.Date;
        Content = content;
    }

    private CalendarDayContent(DateTime date, Func<Control> controlFactory)
    {
        Date = date.Date;
        ControlFactory = controlFactory;
    }

    public DateTime Date { get; }

    public object? Content { get; }

    /// <summary>
    /// Фабрика, создающая новый Avalonia Control при каждом обновлении календаря.
    /// </summary>
    public Func<Control>? ControlFactory { get; }

    /// <summary>
    /// Создаёт элемент непосредственно из Avalonia-контрола.
    /// Фабрика должна каждый раз возвращать новый экземпляр.
    /// </summary>
    public static CalendarDayContent FromControl(
        DateTime date,
        Func<Control> controlFactory)
    {
        ArgumentNullException.ThrowIfNull(controlFactory);

        return new CalendarDayContent(date, controlFactory);
    }
}
