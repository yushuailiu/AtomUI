﻿using System.Globalization;
using AtomUI.Controls.Utils;
using AtomUI.Media;
using AtomUI.Theme.Data;
using AtomUI.Theme.Styling;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using AvaloniaButton = Avalonia.Controls.Button;

namespace AtomUI.Controls.CalendarView;

[PseudoClasses(StdPseudoClass.Pressed,
    StdPseudoClass.Disabled,
    StdPseudoClass.Selected,
    StdPseudoClass.InActive,
    TodayPC,
    BlackoutPC,
    DayfocusedPC)]
internal sealed class CalendarDayButton : AvaloniaButton
{
    internal const string TodayPC = ":today";
    internal const string BlackoutPC = ":blackout";
    internal const string DayfocusedPC = ":dayfocused";

    /// <summary>
    /// Gets or sets the Calendar associated with this button.
    /// </summary>
    internal Calendar? Owner { get; set; }
    
    /// <summary>
    /// Default content for the CalendarDayButton.
    /// </summary>
    private const int DefaultContent = 1;

    private bool _ignoringMouseOverState;
    private bool _isBlackout;

    private bool _isCurrent;
    private bool _isInactive;
    private bool _isSelected;
    private bool _isToday;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="T:Avalonia.Controls.Primitives.CalendarDayButton" />
    /// class.
    /// </summary>
    public CalendarDayButton()
    {
        //Focusable = false;
        SetCurrentValue(ContentProperty, DefaultContent.ToString(CultureInfo.CurrentCulture));
    }

    internal int Index { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the button is the focused
    /// element on the Calendar control.
    /// </summary>
    internal bool IsCurrent
    {
        get => _isCurrent;

        set
        {
            if (_isCurrent != value)
            {
                _isCurrent = value;
                SetPseudoClasses();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this is a blackout date.
    /// </summary>
    internal bool IsBlackout
    {
        get => _isBlackout;

        set
        {
            if (_isBlackout != value)
            {
                _isBlackout = value;
                SetPseudoClasses();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this button represents
    /// today.
    /// </summary>
    internal bool IsToday
    {
        get => _isToday;

        set
        {
            if (_isToday != value)
            {
                _isToday = value;
                SetPseudoClasses();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the button is inactive.
    /// </summary>
    internal bool IsInactive
    {
        get => _isInactive;

        set
        {
            if (_isInactive != value)
            {
                _isInactive = value;
                SetPseudoClasses();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the button is selected.
    /// </summary>
    internal bool IsSelected
    {
        get => _isSelected;

        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                SetPseudoClasses();
            }
        }
    }

    /// <summary>
    /// Ensure the button is not in the MouseOver state.
    /// </summary>
    /// <remarks>
    /// If a button is in the MouseOver state when a Popup is closed (as is
    /// the case when you select a date in the DatePicker control), it will
    /// continue to think it's in the mouse over state even when the Popup
    /// opens again and it's not.  This method is used to forcibly clear the
    /// state by changing the CommonStates state group.
    /// </remarks>
    internal void IgnoreMouseOverState()
    {
        // TODO: Investigate whether this needs to be done by changing the
        // state everytime we change any state, or if it can be done once
        // to properly reset the control.

        _ignoringMouseOverState = false;

        // If the button thinks it's in the MouseOver state (which can
        // happen when a Popup is closed before the button can change state)
        // we will override the state so it shows up as normal.
        if (IsPointerOver)
        {
            _ignoringMouseOverState = true;
            SetPseudoClasses();
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        SetPseudoClasses();
        Transitions ??= new Transitions
        {
            AnimationUtils.CreateTransition<SolidColorBrushTransition>(BackgroundProperty,
                GlobalTokenResourceKey.MotionDurationFast),
            AnimationUtils.CreateTransition<SolidColorBrushTransition>(ForegroundProperty,
                GlobalTokenResourceKey.MotionDurationFast)
        };
    }

    private void SetPseudoClasses()
    {
        if (_ignoringMouseOverState)
        {
            PseudoClasses.Set(StdPseudoClass.Pressed, IsPressed);
            PseudoClasses.Set(StdPseudoClass.Disabled, !IsEnabled);
        }

        PseudoClasses.Set(StdPseudoClass.Selected, IsSelected);
        PseudoClasses.Set(StdPseudoClass.InActive, IsInactive);
        PseudoClasses.Set(TodayPC, IsToday);
        PseudoClasses.Set(BlackoutPC, IsBlackout);
        PseudoClasses.Set(DayfocusedPC, IsCurrent && IsEnabled);
    }

    /// <summary>
    /// Occurs when the left mouse button is pressed (or when the tip of the
    /// stylus touches the tablet PC) while the mouse pointer is over a
    /// UIElement.
    /// </summary>
    public event EventHandler<PointerPressedEventArgs>? CalendarDayButtonMouseDown;

    /// <summary>
    /// Occurs when the left mouse button is released (or the tip of the
    /// stylus is removed from the tablet PC) while the mouse (or the
    /// stylus) is over a UIElement (or while a UIElement holds mouse
    /// capture).
    /// </summary>
    public event EventHandler<PointerReleasedEventArgs>? CalendarDayButtonMouseUp;

    /// <summary>
    /// Provides class handling for the MouseLeftButtonDown event that
    /// occurs when the left mouse button is pressed while the mouse pointer
    /// is over this control.
    /// </summary>
    /// <param name="e">The event data. </param>
    /// <exception cref="System.ArgumentNullException">
    /// e is a null reference (Nothing in Visual Basic).
    /// </exception>
    /// <remarks>
    /// This method marks the MouseLeftButtonDown event as handled by
    /// setting the MouseButtonEventArgs.Handled property of the event data
    /// to true when the button is enabled and its ClickMode is not set to
    /// Hover.  Since this method marks the MouseLeftButtonDown event as
    /// handled in some situations, you should use the Click event instead
    /// to detect a button click.
    /// </remarks>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            CalendarDayButtonMouseDown?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Provides handling for the MouseLeftButtonUp event that occurs when
    /// the left mouse button is released while the mouse pointer is over
    /// this control.
    /// </summary>
    /// <param name="e">The event data.</param>
    /// <exception cref="System.ArgumentNullException">
    /// e is a null reference (Nothing in Visual Basic).
    /// </exception>
    /// <remarks>
    /// This method marks the MouseLeftButtonUp event as handled by setting
    /// the MouseButtonEventArgs.Handled property of the event data to true
    /// when the button is enabled and its ClickMode is not set to Hover.
    /// Since this method marks the MouseLeftButtonUp event as handled in
    /// some situations, you should use the Click event instead to detect a
    /// button click.
    /// </remarks>
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (e.InitialPressMouseButton == MouseButton.Left)
        {
            CalendarDayButtonMouseUp?.Invoke(this, e);
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        TokenResourceBinder.CreateGlobalTokenBinding(this, BorderThicknessProperty,
            GlobalTokenResourceKey.BorderThickness, BindingPriority.Template,
            new RenderScaleAwareThicknessConfigure(this));
    }
}