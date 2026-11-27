using System;
using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

using IINET.UI.Calendar.Controls;

namespace IINET.UI.Demo;

public partial class MainWindow : Window
{
    public ObservableCollection<CalendarDayContent> CalendarMarks { get; } = new()
    {
        Element(new DateTime(2026, 11, 4), () =>
            new Button
            {
                Content = "📝"
            }),

        Element(new DateTime(2026, 11, 12), () =>
            new TextBlock
            {
                Text = "📅"
            }),

        Element(new DateTime(2026, 11, 14), () =>
            new StackPanel
            {
                Children =
                {
                    new TextBlock 
                    { 
                        Text = "📚",
                        Foreground = new SolidColorBrush(
                            Color.Parse("#10B981"))
                    },

                    new Button
                    {
                        Content = "Учёба",
                        Foreground = new SolidColorBrush(
                            Color.Parse("#10B981"))
                    }
                }
            }),

        Element(new DateTime(2026, 11, 18), () =>
            new CheckBox
            {
                IsChecked = false,
                HorizontalAlignment = HorizontalAlignment.Center
            }),

        Element(new DateTime(2026, 11, 20), () =>
            new Border
            {
                Width = 8,
                Height = 8,
                CornerRadius = new CornerRadius(8),
                Background = Brushes.Red
            })
    };

    private static CalendarDayContent Element(
        DateTime date,
        Func<Control> factory)
    {
        return CalendarDayContent.FromControl(date, factory);
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }
}
