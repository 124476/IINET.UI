using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace IINET.UI.Calendar.Controls;

public class Calendar : TemplatedControl
{
    // =========================================================
    // Date properties
    // =========================================================

    public static readonly StyledProperty<DateTime> SelectedDateProperty =
        AvaloniaProperty.Register<Calendar, DateTime>(
            nameof(SelectedDate),
            defaultValue: DateTime.Today,
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<DateTime> DisplayDateProperty =
        AvaloniaProperty.Register<Calendar, DateTime>(
            nameof(DisplayDate),
            defaultValue: DateTime.Today);

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        AvaloniaProperty.Register<Calendar, DayOfWeek>(
            nameof(FirstDayOfWeek),
            defaultValue: DayOfWeek.Monday);

    // =========================================================
    // Chrome / navigation appearance
    // =========================================================

    public static readonly StyledProperty<IBrush?> CalendarBackgroundProperty =
        AvaloniaProperty.Register<Calendar, IBrush?>(
            nameof(CalendarBackground),
            Brushes.White);

    public static readonly StyledProperty<IBrush?> HeaderBackgroundProperty =
    AvaloniaProperty.Register<Calendar, IBrush?>(
        nameof(HeaderBackground),
        Brushes.Transparent);

    public static readonly StyledProperty<IBrush?> CalendarBorderBrushProperty =
        AvaloniaProperty.Register<Calendar, IBrush?>(
            nameof(CalendarBorderBrush),
            Brushes.Transparent);

    public static readonly StyledProperty<Thickness> CalendarBorderThicknessProperty =
        AvaloniaProperty.Register<Calendar, Thickness>(
            nameof(CalendarBorderThickness),
            new Thickness(1));

    public static readonly StyledProperty<double> NavigationFontSizeProperty =
        AvaloniaProperty.Register<Calendar, double>(
            nameof(NavigationFontSize),
            18.0);

    public static readonly StyledProperty<IBrush?> NavigationForegroundProperty =
        AvaloniaProperty.Register<Calendar, IBrush?>(
            nameof(NavigationForeground),
            Brushes.Black);

    public static readonly StyledProperty<IBrush?> NavigationHoverForegroundProperty =
        AvaloniaProperty.Register<Calendar, IBrush?>(
            nameof(NavigationHoverForeground),
            Brushes.Black);

    public static readonly StyledProperty<IBrush?> NavigationBackgroundProperty =
        AvaloniaProperty.Register<Calendar, IBrush?>(
            nameof(NavigationBackground),
            Brushes.Transparent);

    public static readonly StyledProperty<IBrush?> NavigationHoverBackgroundProperty =
        AvaloniaProperty.Register<Calendar, IBrush?>(
            nameof(NavigationHoverBackground),
            new SolidColorBrush(Color.Parse("#E5E7EB")));

    // =========================================================
    // Font sizes
    // =========================================================

    public static readonly StyledProperty<double> MonthTitleFontSizeProperty =
        AvaloniaProperty.Register<Calendar, double>(
            nameof(MonthTitleFontSize),
            16.0);

    public static readonly StyledProperty<double> WeekdayFontSizeProperty =
        AvaloniaProperty.Register<Calendar, double>(
            nameof(WeekdayFontSize),
            12.0);

    public static readonly StyledProperty<double> DayFontSizeProperty =
        AvaloniaProperty.Register<Calendar, double>(
            nameof(DayFontSize),
            14.0);

    public static readonly StyledProperty<double> DayContentFontSizeProperty =
        AvaloniaProperty.Register<Calendar, double>(
            nameof(DayContentFontSize),
            12.0);

    // =========================================================
    // Labels
    // =========================================================

    public static readonly StyledProperty<string> WeekdayLabelsProperty =
        AvaloniaProperty.Register<Calendar, string>(
            nameof(WeekdayLabels),
            defaultValue: string.Empty);

    public static readonly StyledProperty<string> MonthNamesProperty =
        AvaloniaProperty.Register<Calendar, string>(
            nameof(MonthNames),
            defaultValue: string.Empty);

    // =========================================================
    // Day colors
    // =========================================================

    public static readonly StyledProperty<Color> MonthTitleForegroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(MonthTitleForeground),
            Color.Parse("#111827"));

    public static readonly StyledProperty<Color> DayForegroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(DayForeground),
            Color.Parse("#111827"));

    public static readonly StyledProperty<Color> DayBackgroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(DayBackground),
            Colors.White);

    public static readonly StyledProperty<Color> SelectedDayForegroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(SelectedDayForeground),
            Colors.White);

    public static readonly StyledProperty<Color> SelectedDayBackgroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(SelectedDayBackground),
            Color.Parse("#6366F1"));

    public static readonly StyledProperty<Color> OutsideMonthDayForegroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(OutsideMonthDayForeground),
            Color.Parse("#9CA3AF"));

    public static readonly StyledProperty<Color> OutsideMonthDayBackgroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(OutsideMonthDayBackground),
            Color.Parse("#F9FAFB"));

    public static readonly StyledProperty<Color> WeekdayForegroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(WeekdayForeground),
            Color.Parse("#6B7280"));

    // =========================================================
    // Hover colors
    // =========================================================

    public static readonly StyledProperty<Color> DayHoverBackgroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(DayHoverBackground),
            Color.Parse("#E5E7EB"));

    public static readonly StyledProperty<Color> DayHoverForegroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(DayHoverForeground),
            Color.Parse("#111827"));

    public static readonly StyledProperty<Color> SelectedDayHoverBackgroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(SelectedDayHoverBackground),
            Color.Parse("#4F46E5"));

    public static readonly StyledProperty<Color> SelectedDayHoverForegroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(SelectedDayHoverForeground),
            Colors.White);

    public static readonly StyledProperty<Color> OutsideMonthDayHoverBackgroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(OutsideMonthDayHoverBackground),
            Color.Parse("#F3F4F6"));

    public static readonly StyledProperty<Color> OutsideMonthDayHoverForegroundProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(OutsideMonthDayHoverForeground),
            Color.Parse("#6B7280"));

    // =========================================================
    // Today
    // =========================================================

    public static readonly StyledProperty<Color> TodayBorderColorProperty =
        AvaloniaProperty.Register<Calendar, Color>(
            nameof(TodayBorderColor),
            Color.Parse("#6366F1"));

    public static readonly StyledProperty<double> TodayBorderThicknessProperty =
        AvaloniaProperty.Register<Calendar, double>(
            nameof(TodayBorderThickness),
            1.0);

    // =========================================================
    // Additional day content
    // =========================================================

    public static readonly StyledProperty<IEnumerable<CalendarDayContent>?> DayContentsProperty =
        AvaloniaProperty.Register<Calendar, IEnumerable<CalendarDayContent>?>(
            nameof(DayContents));

    public static readonly StyledProperty<IDataTemplate?> DayContentTemplateProperty =
        AvaloniaProperty.Register<Calendar, IDataTemplate?>(
            nameof(DayContentTemplate));

    // =========================================================
    // CLR properties
    // =========================================================

    public IBrush? CalendarBackground
    {
        get => GetValue(CalendarBackgroundProperty);
        set => SetValue(CalendarBackgroundProperty, value);
    }

    public IBrush? HeaderBackground
    {
        get => GetValue(HeaderBackgroundProperty);
        set => SetValue(HeaderBackgroundProperty, value);
    }

    public IBrush? CalendarBorderBrush
    {
        get => GetValue(CalendarBorderBrushProperty);
        set => SetValue(CalendarBorderBrushProperty, value);
    }

    public Thickness CalendarBorderThickness
    {
        get => GetValue(CalendarBorderThicknessProperty);
        set => SetValue(CalendarBorderThicknessProperty, value);
    }

    public double NavigationFontSize
    {
        get => GetValue(NavigationFontSizeProperty);
        set => SetValue(NavigationFontSizeProperty, value);
    }

    public IBrush? NavigationForeground
    {
        get => GetValue(NavigationForegroundProperty);
        set => SetValue(NavigationForegroundProperty, value);
    }

    public IBrush? NavigationHoverForeground
    {
        get => GetValue(NavigationHoverForegroundProperty);
        set => SetValue(NavigationHoverForegroundProperty, value);
    }

    public IBrush? NavigationBackground
    {
        get => GetValue(NavigationBackgroundProperty);
        set => SetValue(NavigationBackgroundProperty, value);
    }

    public IBrush? NavigationHoverBackground
    {
        get => GetValue(NavigationHoverBackgroundProperty);
        set => SetValue(NavigationHoverBackgroundProperty, value);
    }

    public double MonthTitleFontSize
    {
        get => GetValue(MonthTitleFontSizeProperty);
        set => SetValue(MonthTitleFontSizeProperty, value);
    }

    public double WeekdayFontSize
    {
        get => GetValue(WeekdayFontSizeProperty);
        set => SetValue(WeekdayFontSizeProperty, value);
    }

    public double DayFontSize
    {
        get => GetValue(DayFontSizeProperty);
        set => SetValue(DayFontSizeProperty, value);
    }

    public double DayContentFontSize
    {
        get => GetValue(DayContentFontSizeProperty);
        set => SetValue(DayContentFontSizeProperty, value);
    }

    public Color MonthTitleForeground
    {
        get => GetValue(MonthTitleForegroundProperty);
        set => SetValue(MonthTitleForegroundProperty, value);
    }

    public DateTime SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public DateTime DisplayDate
    {
        get => GetValue(DisplayDateProperty);
        set => SetValue(DisplayDateProperty, value);
    }

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public string WeekdayLabels
    {
        get => GetValue(WeekdayLabelsProperty);
        set => SetValue(WeekdayLabelsProperty, value);
    }

    public string MonthNames
    {
        get => GetValue(MonthNamesProperty);
        set => SetValue(MonthNamesProperty, value);
    }

    public Color DayForeground
    {
        get => GetValue(DayForegroundProperty);
        set => SetValue(DayForegroundProperty, value);
    }

    public Color DayBackground
    {
        get => GetValue(DayBackgroundProperty);
        set => SetValue(DayBackgroundProperty, value);
    }

    public Color SelectedDayForeground
    {
        get => GetValue(SelectedDayForegroundProperty);
        set => SetValue(SelectedDayForegroundProperty, value);
    }

    public Color SelectedDayBackground
    {
        get => GetValue(SelectedDayBackgroundProperty);
        set => SetValue(SelectedDayBackgroundProperty, value);
    }

    public Color OutsideMonthDayForeground
    {
        get => GetValue(OutsideMonthDayForegroundProperty);
        set => SetValue(OutsideMonthDayForegroundProperty, value);
    }

    public Color OutsideMonthDayBackground
    {
        get => GetValue(OutsideMonthDayBackgroundProperty);
        set => SetValue(OutsideMonthDayBackgroundProperty, value);
    }

    public Color WeekdayForeground
    {
        get => GetValue(WeekdayForegroundProperty);
        set => SetValue(WeekdayForegroundProperty, value);
    }

    public Color DayHoverBackground
    {
        get => GetValue(DayHoverBackgroundProperty);
        set => SetValue(DayHoverBackgroundProperty, value);
    }

    public Color DayHoverForeground
    {
        get => GetValue(DayHoverForegroundProperty);
        set => SetValue(DayHoverForegroundProperty, value);
    }

    public Color SelectedDayHoverBackground
    {
        get => GetValue(SelectedDayHoverBackgroundProperty);
        set => SetValue(SelectedDayHoverBackgroundProperty, value);
    }

    public Color SelectedDayHoverForeground
    {
        get => GetValue(SelectedDayHoverForegroundProperty);
        set => SetValue(SelectedDayHoverForegroundProperty, value);
    }

    public Color OutsideMonthDayHoverBackground
    {
        get => GetValue(OutsideMonthDayHoverBackgroundProperty);
        set => SetValue(OutsideMonthDayHoverBackgroundProperty, value);
    }

    public Color OutsideMonthDayHoverForeground
    {
        get => GetValue(OutsideMonthDayHoverForegroundProperty);
        set => SetValue(OutsideMonthDayHoverForegroundProperty, value);
    }

    public Color TodayBorderColor
    {
        get => GetValue(TodayBorderColorProperty);
        set => SetValue(TodayBorderColorProperty, value);
    }

    public double TodayBorderThickness
    {
        get => GetValue(TodayBorderThicknessProperty);
        set => SetValue(TodayBorderThicknessProperty, value);
    }

    public IEnumerable<CalendarDayContent>? DayContents
    {
        get => GetValue(DayContentsProperty);
        set => SetValue(DayContentsProperty, value);
    }

    public IDataTemplate? DayContentTemplate
    {
        get => GetValue(DayContentTemplateProperty);
        set => SetValue(DayContentTemplateProperty, value);
    }

    // =========================================================
    // Template parts
    // =========================================================

    private Control? _previousButton;
    private Control? _nextButton;

    private TextBlock? _previousGlyph;
    private TextBlock? _nextGlyph;

    private TextBlock? _monthTitle;
    private UniformGrid? _weekdaysGrid;
    private UniformGrid? _daysGrid;

    private INotifyCollectionChanged? _observedDayContents;

    public event Action<DateTime>? DateSelected;

    // =========================================================
    // Template
    // =========================================================

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        UnhookNavigation();

        base.OnApplyTemplate(e);

        _previousButton = e.NameScope.Find<Control>("PART_PreviousButton");
        _nextButton     = e.NameScope.Find<Control>("PART_NextButton");
        _previousGlyph  = e.NameScope.Find<TextBlock>("PART_PreviousGlyph");
        _nextGlyph      = e.NameScope.Find<TextBlock>("PART_NextGlyph");
        _monthTitle     = e.NameScope.Find<TextBlock>("PART_MonthTitle");
        _weekdaysGrid   = e.NameScope.Find<UniformGrid>("PART_Weekdays");
        _daysGrid       = e.NameScope.Find<UniformGrid>("PART_Days");

        HookNavigation();

        UpdateCalendar();
        ApplyNavigationAppearance();
    }

    private void HookNavigation()
    {
        // В XAML кнопки — Border. Поддерживаем и Border, и Button,
        // чтобы не зависеть от конкретной реализации в теме.
        if (_previousButton is Button prevBtn)
            prevBtn.Click += OnPreviousMonthClick;
        else if (_previousButton is not null)
            _previousButton.PointerPressed += OnPreviousPointerPressed;

        if (_nextButton is Button nextBtn)
            nextBtn.Click += OnNextMonthClick;
        else if (_nextButton is not null)
            _nextButton.PointerPressed += OnNextPointerPressed;

        if (_previousButton is not null)
        {
            _previousButton.PointerEntered += OnPreviousPointerEntered;
            _previousButton.PointerExited  += OnPreviousPointerExited;
        }

        if (_nextButton is not null)
        {
            _nextButton.PointerEntered += OnNextPointerEntered;
            _nextButton.PointerExited  += OnNextPointerExited;
        }
    }

    private void UnhookNavigation()
    {
        if (_previousButton is Button prevBtn)
            prevBtn.Click -= OnPreviousMonthClick;
        else if (_previousButton is not null)
            _previousButton.PointerPressed -= OnPreviousPointerPressed;

        if (_nextButton is Button nextBtn)
            nextBtn.Click -= OnNextMonthClick;
        else if (_nextButton is not null)
            _nextButton.PointerPressed -= OnNextPointerPressed;

        if (_previousButton is not null)
        {
            _previousButton.PointerEntered -= OnPreviousPointerEntered;
            _previousButton.PointerExited  -= OnPreviousPointerExited;
        }

        if (_nextButton is not null)
        {
            _nextButton.PointerEntered -= OnNextPointerEntered;
            _nextButton.PointerExited  -= OnNextPointerExited;
        }
    }

    private void ApplyNavigationAppearance()
    {
        ApplyNavigationHover(_previousButton, _previousGlyph, hovered: false);
        ApplyNavigationHover(_nextButton,     _nextGlyph,     hovered: false);
    }

    private void ApplyNavigationHover(Control? button, TextBlock? glyph, bool hovered)
    {
        if (button is null)
            return;

        IBrush? background = hovered
            ? NavigationHoverBackground
            : NavigationBackground;

        if (button is Border border)
        {
            border.Background = background;
        }
        else if (button is TemplatedControl templated)
        {
            templated.Background = background;
        }

        if (glyph is not null)
        {
            glyph.Foreground = hovered
                ? NavigationHoverForeground
                : NavigationForeground;
        }
    }

    // =========================================================
    // Property changes
    // =========================================================

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedDateProperty)
        {
            DateTime selectedMonth = new(
                SelectedDate.Year,
                SelectedDate.Month,
                1);

            if (DisplayDate.Year != selectedMonth.Year ||
                DisplayDate.Month != selectedMonth.Month)
            {
                SetCurrentValue(DisplayDateProperty, selectedMonth);
            }
            else
            {
                UpdateDays();
            }

            return;
        }

        if (change.Property == DisplayDateProperty)
        {
            UpdateMonthTitle();
            UpdateDays();
            return;
        }

        if (change.Property == FirstDayOfWeekProperty ||
            change.Property == WeekdayLabelsProperty)
        {
            UpdateWeekdays();
            UpdateDays();
            return;
        }

        if (change.Property == MonthNamesProperty ||
            change.Property == MonthTitleForegroundProperty ||
            change.Property == MonthTitleFontSizeProperty)
        {
            UpdateMonthTitle();
            return;
        }

        if (change.Property == WeekdayFontSizeProperty)
        {
            UpdateWeekdays();
            return;
        }

        if (change.Property == DayFontSizeProperty ||
            change.Property == DayContentFontSizeProperty)
        {
            UpdateDays();
            return;
        }

        if (change.Property == NavigationForegroundProperty ||
            change.Property == NavigationHoverForegroundProperty ||
            change.Property == NavigationBackgroundProperty ||
            change.Property == NavigationHoverBackgroundProperty)
        {
            ApplyNavigationAppearance();
            return;
        }

        if (change.Property == DayContentsProperty)
        {
            if (_observedDayContents is not null)
                _observedDayContents.CollectionChanged -= OnDayContentsChanged;

            _observedDayContents =
                DayContents as INotifyCollectionChanged;

            if (_observedDayContents is not null)
                _observedDayContents.CollectionChanged += OnDayContentsChanged;

            UpdateDays();
            return;
        }

        if (change.Property == DayContentTemplateProperty)
        {
            UpdateDays();
            return;
        }

        if (change.Property == DayForegroundProperty ||
            change.Property == DayBackgroundProperty ||
            change.Property == SelectedDayForegroundProperty ||
            change.Property == SelectedDayBackgroundProperty ||
            change.Property == OutsideMonthDayForegroundProperty ||
            change.Property == OutsideMonthDayBackgroundProperty ||
            change.Property == WeekdayForegroundProperty ||
            change.Property == DayHoverBackgroundProperty ||
            change.Property == DayHoverForegroundProperty ||
            change.Property == SelectedDayHoverBackgroundProperty ||
            change.Property == SelectedDayHoverForegroundProperty ||
            change.Property == OutsideMonthDayHoverBackgroundProperty ||
            change.Property == OutsideMonthDayHoverForegroundProperty ||
            change.Property == TodayBorderColorProperty ||
            change.Property == TodayBorderThicknessProperty)
        {
            UpdateWeekdays();
            UpdateDays();
        }
    }

    private void OnDayContentsChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        UpdateDays();
    }

    // =========================================================
    // Navigation
    // =========================================================

    private void OnPreviousMonthClick(
        object? sender,
        RoutedEventArgs e)
    {
        ChangeMonth(-1);
    }

    private void OnNextMonthClick(
        object? sender,
        RoutedEventArgs e)
    {
        ChangeMonth(1);
    }

    private void OnPreviousPointerPressed(
        object? sender,
        PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(_previousButton).Properties.IsLeftButtonPressed)
        {
            ChangeMonth(-1);
            e.Handled = true;
        }
    }

    private void OnNextPointerPressed(
        object? sender,
        PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(_nextButton).Properties.IsLeftButtonPressed)
        {
            ChangeMonth(1);
            e.Handled = true;
        }
    }

    private void OnPreviousPointerEntered(object? sender, PointerEventArgs e)
        => ApplyNavigationHover(_previousButton, _previousGlyph, hovered: true);

    private void OnPreviousPointerExited(object? sender, PointerEventArgs e)
        => ApplyNavigationHover(_previousButton, _previousGlyph, hovered: false);

    private void OnNextPointerEntered(object? sender, PointerEventArgs e)
        => ApplyNavigationHover(_nextButton, _nextGlyph, hovered: true);

    private void OnNextPointerExited(object? sender, PointerEventArgs e)
        => ApplyNavigationHover(_nextButton, _nextGlyph, hovered: false);

    private void ChangeMonth(int offset)
    {
        DateTime firstDay = new(
            DisplayDate.Year,
            DisplayDate.Month,
            1);

        SetCurrentValue(
            DisplayDateProperty,
            firstDay.AddMonths(offset));
    }

    // =========================================================
    // Rendering
    // =========================================================

    private void UpdateCalendar()
    {
        UpdateMonthTitle();
        UpdateWeekdays();
        UpdateDays();
    }

    private void UpdateMonthTitle()
    {
        if (_monthTitle is null)
            return;

        _monthTitle.FontSize = MonthTitleFontSize;
        _monthTitle.Foreground = new SolidColorBrush(MonthTitleForeground);

        string[] names = MonthNames.Split(
            '|',
            StringSplitOptions.TrimEntries);

        bool hasCustomNames =
            names.Length == 12 &&
            names.All(name => !string.IsNullOrWhiteSpace(name));

        string monthName = hasCustomNames
            ? names[DisplayDate.Month - 1]
            : DisplayDate.ToString(
                "MMMM",
                CultureInfo.CurrentCulture);

        _monthTitle.Text = $"{monthName} {DisplayDate.Year}";
    }

    private void UpdateWeekdays()
    {
        if (_weekdaysGrid is null)
            return;

        _weekdaysGrid.Children.Clear();

        string[]? customLabels = null;

        if (!string.IsNullOrWhiteSpace(WeekdayLabels))
        {
            string[] parsed = WeekdayLabels.Split(
                ',',
                StringSplitOptions.TrimEntries);

            if (parsed.Length == 7 &&
                parsed.All(label => !string.IsNullOrWhiteSpace(label)))
            {
                customLabels = parsed;
            }
        }

        CultureInfo culture = CultureInfo.CurrentCulture;

        for (int i = 0; i < 7; i++)
        {
            DayOfWeek day = (DayOfWeek)(
                ((int)FirstDayOfWeek + i) % 7);

            int mondayBasedIndex = ((int)day + 6) % 7;

            string label = customLabels is not null
                ? customLabels[mondayBasedIndex]
                : culture.DateTimeFormat.GetAbbreviatedDayName(day);

            var textBlock = new TextBlock
            {
                Text = label,
                FontSize = WeekdayFontSize,
                Foreground = new SolidColorBrush(WeekdayForeground),
                FontWeight = FontWeight.Bold,
                HorizontalAlignment =
                    Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment =
                    Avalonia.Layout.VerticalAlignment.Center
            };

            _weekdaysGrid.Children.Add(textBlock);
        }
    }

    private Dictionary<DateTime, List<CalendarDayContent>>
        BuildDayContentDictionary()
    {
        var result =
            new Dictionary<DateTime, List<CalendarDayContent>>();

        if (DayContents is null)
            return result;

        foreach (CalendarDayContent item in DayContents)
        {
            DateTime date = item.Date.Date;

            if (!result.TryGetValue(date, out var items))
            {
                items = new List<CalendarDayContent>();
                result.Add(date, items);
            }

            items.Add(item);
        }

        return result;
    }

    private void UpdateDays()
    {
        if (_daysGrid is null)
            return;

        _daysGrid.Children.Clear();

        DateTime firstDay = new(
            DisplayDate.Year,
            DisplayDate.Month,
            1);

        int offset =
            ((int)firstDay.DayOfWeek -
            (int)FirstDayOfWeek + 7) % 7;

        DateTime gridStart = firstDay.AddDays(-offset);

        var contentsByDate = BuildDayContentDictionary();

        for (int i = 0; i < 42; i++)
        {
            DateTime date = gridStart.AddDays(i);

            bool isCurrentMonth =
                date.Month == DisplayDate.Month &&
                date.Year == DisplayDate.Year;

            bool isSelected =
                date.Date == SelectedDate.Date;

            bool isToday =
                date.Date == DateTime.Today;

            contentsByDate.TryGetValue(date.Date, out var dayContents);

            var dayNumber = new TextBlock
            {
                Text = date.Day.ToString(CultureInfo.CurrentCulture),
                FontSize = DayFontSize,
                FontWeight = isSelected
                    ? FontWeight.Bold
                    : FontWeight.Normal,

                HorizontalAlignment =
                    Avalonia.Layout.HorizontalAlignment.Center,

                VerticalAlignment =
                    Avalonia.Layout.VerticalAlignment.Center
            };

            var cellContent = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Vertical,

                HorizontalAlignment =
                    Avalonia.Layout.HorizontalAlignment.Center,

                VerticalAlignment =
                    Avalonia.Layout.VerticalAlignment.Center,

                Spacing = 1
            };

            cellContent.Children.Add(dayNumber);

            if (dayContents is not null)
            {
                foreach (CalendarDayContent item in dayContents)
                {
                    Control presenterControl;

                    if (item.ControlFactory is not null)
                    {
                        presenterControl = item.ControlFactory();

                        if (presenterControl.HorizontalAlignment == Avalonia.Layout.HorizontalAlignment.Stretch)
                            presenterControl.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;

                        if (presenterControl.VerticalAlignment == Avalonia.Layout.VerticalAlignment.Stretch)
                            presenterControl.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                    }
                    else
                    {
                        presenterControl = new ContentControl
                        {
                            Content = item.Content,
                            ContentTemplate = DayContentTemplate,
                            FontSize = DayContentFontSize,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Center
                        };
                    }

                    cellContent.Children.Add(presenterControl);
                }
            }

            var cell = new Border
            {
                Child = cellContent,

                MinHeight = dayContents is { Count: > 0 } ? 48 : 36,

                Margin = new Thickness(1),
                Padding = new Thickness(0),

                CornerRadius = new CornerRadius(6),

                HorizontalAlignment =
                    Avalonia.Layout.HorizontalAlignment.Stretch,

                VerticalAlignment =
                    Avalonia.Layout.VerticalAlignment.Stretch
            };

            ApplyDayAppearance(
                cell,
                dayNumber,
                isCurrentMonth,
                isSelected,
                isToday,
                isHovered: false);

            cell.PointerEntered += (_, _) =>
            {
                ApplyDayAppearance(
                    cell,
                    dayNumber,
                    isCurrentMonth,
                    isSelected,
                    isToday,
                    isHovered: true);
            };

            cell.PointerExited += (_, _) =>
            {
                ApplyDayAppearance(
                    cell,
                    dayNumber,
                    isCurrentMonth,
                    isSelected,
                    isToday,
                    isHovered: false);
            };

            cell.PointerPressed += (_, e) =>
            {
                if (e.GetCurrentPoint(cell).Properties.IsLeftButtonPressed)
                {
                    SelectDate(date);
                    e.Handled = true;
                }
            };

            _daysGrid.Children.Add(cell);
        }
    }

    // =========================================================
    // Day appearance
    // =========================================================

    private void ApplyDayAppearance(
        Border cell,
        TextBlock dayNumber,
        bool isCurrentMonth,
        bool isSelected,
        bool isToday,
        bool isHovered)
    {
        Color background;
        Color foreground;

        if (isHovered && isSelected)
        {
            background = SelectedDayHoverBackground;
            foreground = SelectedDayHoverForeground;
        }
        else if (isHovered && !isCurrentMonth)
        {
            background = OutsideMonthDayHoverBackground;
            foreground = OutsideMonthDayHoverForeground;
        }
        else if (isHovered)
        {
            background = DayHoverBackground;
            foreground = DayHoverForeground;
        }
        else if (isSelected)
        {
            background = SelectedDayBackground;
            foreground = SelectedDayForeground;
        }
        else if (isCurrentMonth)
        {
            background = DayBackground;
            foreground = DayForeground;
        }
        else
        {
            background = OutsideMonthDayBackground;
            foreground = OutsideMonthDayForeground;
        }

        cell.Background = new SolidColorBrush(background);

        dayNumber.Foreground = new SolidColorBrush(foreground);

        cell.BorderBrush = isToday
            ? new SolidColorBrush(TodayBorderColor)
            : Brushes.Transparent;

        cell.BorderThickness = isToday
            ? new Thickness(TodayBorderThickness)
            : new Thickness(0);
    }

    // =========================================================
    // Selection
    // =========================================================

    private void SelectDate(DateTime date)
    {
        SetCurrentValue(SelectedDateProperty, date.Date);

        DateSelected?.Invoke(date.Date);
    }
}