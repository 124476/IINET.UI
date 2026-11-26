using System;
using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;

using IINET.UI.Calendar.Controls;

namespace IINET.UI.Demo;

public partial class MainWindow : Window
{
    public ObservableCollection<CalendarDayContent> CalendarMarks { get; } = new()
    {
        new CalendarDayContent(
            new DateTime(2026, 11, 4),
            new CalendarMark("📝", "Заметка")),

        new CalendarDayContent(
            new DateTime(2026, 11, 12),
            new CalendarMark("📚", "Учёба")),

        CalendarDayContent.FromControl(
            new DateTime(2026, 11, 26),
            () =>
            {
                var button = new Button
                {
                    Content = "🎉",
                    FontSize = 14,
                    Padding = new Thickness(2, 0),
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
                };

                button.Click += (_, e) =>
                {
                };

                button.PointerPressed += (_, e) => e.Handled = true;

                return button;
            }),

        CalendarDayContent.FromControl(
            new DateTime(2026, 11, 30),
            () =>
            {
                var panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 3,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment   = VerticalAlignment.Center
                };

                panel.Children.Add(new TextBlock
                {
                    Text = "✅",
                    FontSize = 12,
                    VerticalAlignment = VerticalAlignment.Center
                });

                panel.Children.Add(new TextBlock
                {
                    Text = "done",
                    FontSize = 9,
                    Foreground = Brushes.Gray,
                    VerticalAlignment = VerticalAlignment.Center
                });

                return panel;
            }),

        CalendarDayContent.FromControl(
            new DateTime(2026, 11, 10),
            () =>
            {
                var check = new CheckBox
                {
                    IsChecked = false,
                    Padding = new Thickness(0),
                    MinWidth = 0,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment   = VerticalAlignment.Center
                };

                check.PointerPressed += (_, e) => e.Handled = true;

                check.IsCheckedChanged += (_, _) =>
                {
                    // Реакция на изменение состояния.
                };

                return check;
            }),

        CalendarDayContent.FromControl(
            new DateTime(2026, 11, 18),
            () => new Ellipse
            {
                Width  = 6,
                Height = 6,
                Fill   = new SolidColorBrush(Color.Parse("#EF4444")),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment   = VerticalAlignment.Center
            }),
    };

    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;
    }
}

public sealed class CalendarMark
{
    public CalendarMark(string emoji, string title = "")
    {
        Emoji = emoji;
        Title = title;
    }

    public string Emoji { get; }

    public string Title { get; }
}