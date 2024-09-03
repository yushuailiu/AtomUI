﻿using AtomUI.Data;
using AtomUI.Theme.Styling;
using AtomUI.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.LogicalTree;

namespace AtomUI.Controls;

public class TimePicker : LineEdit
{
   protected override Type StyleKeyOverride => typeof(LineEdit);
   
   #region 公共属性定义
   public static readonly StyledProperty<PlacementMode> PickerPlacementProperty =
      AvaloniaProperty.Register<TimePicker, PlacementMode>(nameof(PickerPlacement), defaultValue: PlacementMode.BottomEdgeAlignedLeft);
   
   /// <summary>
   /// 是否显示指示箭头
   /// </summary>
   public static readonly StyledProperty<bool> IsShowArrowProperty =
      ArrowDecoratedBox.IsShowArrowProperty.AddOwner<TimePicker>();

   /// <summary>
   /// 箭头是否始终指向中心
   /// </summary>
   public static readonly StyledProperty<bool> IsPointAtCenterProperty =
      Flyout.IsPointAtCenterProperty.AddOwner<TimePicker>();
   
   /// <summary>
   /// Defines the <see cref="MinuteIncrement"/> property
   /// </summary>
   public static readonly StyledProperty<int> MinuteIncrementProperty =
      AvaloniaProperty.Register<TimePicker, int>(nameof(MinuteIncrement), 1, coerce: CoerceMinuteIncrement);

   public static readonly StyledProperty<int> SecondIncrementProperty =
      AvaloniaProperty.Register<TimePicker, int>(nameof(SecondIncrement), 1, coerce: CoerceSecondIncrement);

   /// <summary>
   /// Defines the <see cref="ClockIdentifier"/> property
   /// </summary>
   public static readonly StyledProperty<string> ClockIdentifierProperty =
      AvaloniaProperty.Register<TimePicker, string>(nameof(ClockIdentifier), "12HourClock",
                                                    coerce: CoerceClockIdentifier);

   /// <summary>
   /// Defines the <see cref="SelectedTime"/> property
   /// </summary>
   public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty =
      AvaloniaProperty.Register<TimePicker, TimeSpan?>(nameof(SelectedTime),
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       enableDataValidation: true);
   
   public static readonly StyledProperty<double> MarginToAnchorProperty =
      Popup.MarginToAnchorProperty.AddOwner<TimePicker>();

   public static readonly StyledProperty<int> MouseEnterDelayProperty =
      FlyoutStateHelper.MouseEnterDelayProperty.AddOwner<TimePicker>();

   public static readonly StyledProperty<int> MouseLeaveDelayProperty =
      FlyoutStateHelper.MouseLeaveDelayProperty.AddOwner<TimePicker>();
   
   /// <summary>
   /// Gets or sets the desired placement of the popup in relation to the <see cref="PlacementTarget"/>.
   /// </summary>
   public PlacementMode PickerPlacement
   {
      get => GetValue(PickerPlacementProperty);
      set => SetValue(PickerPlacementProperty, value);
   }
   
   public bool IsShowArrow
   {
      get => GetValue(IsShowArrowProperty);
      set => SetValue(IsShowArrowProperty, value);
   }

   public bool IsPointAtCenter
   {
      get => GetValue(IsPointAtCenterProperty);
      set => SetValue(IsPointAtCenterProperty, value);
   }
   
   /// <summary>
   /// Gets or sets the minute increment in the picker
   /// </summary>
   public int MinuteIncrement
   {
      get => GetValue(MinuteIncrementProperty);
      set => SetValue(MinuteIncrementProperty, value);
   }

   public int SecondIncrement
   {
      get => GetValue(SecondIncrementProperty);
      set => SetValue(SecondIncrementProperty, value);
   }

   /// <summary>
   /// Gets or sets the clock identifier, either 12HourClock or 24HourClock
   /// </summary>
   public string ClockIdentifier
   {
      get => GetValue(ClockIdentifierProperty);
      set => SetValue(ClockIdentifierProperty, value);
   }

   /// <summary>
   /// Gets or sets the selected time. Can be null.
   /// </summary>
   public TimeSpan? SelectedTime
   {
      get => GetValue(SelectedTimeProperty);
      set => SetValue(SelectedTimeProperty, value);
   }
   
   public double MarginToAnchor
   {
      get => GetValue(MarginToAnchorProperty);
      set => SetValue(MarginToAnchorProperty, value);
   }

   public int MouseEnterDelay
   {
      get => GetValue(MouseEnterDelayProperty);
      set => SetValue(MouseEnterDelayProperty, value);
   }

   public int MouseLeaveDelay
   {
      get => GetValue(MouseLeaveDelayProperty);
      set => SetValue(MouseLeaveDelayProperty, value);
   }
   
   #endregion

   private TextBoxInnerBox? _textBoxInnerBox;
   private TimePickerFlyout? _pickerFlyout;
   private FlyoutStateHelper _flyoutStateHelper;

   static TimePicker()
   {
      HorizontalAlignmentProperty.OverrideDefaultValue<TimePicker>(HorizontalAlignment.Left);
      VerticalAlignmentProperty.OverrideDefaultValue<TimePicker>(VerticalAlignment.Top);
      IsEnableClearButtonProperty.OverrideDefaultValue<TimePicker>(true);
   }

   public TimePicker()
   {
      _flyoutStateHelper = new FlyoutStateHelper()
      {
         TriggerType = FlyoutTriggerType.Click
      };
   }

   protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
   {
      base.OnApplyTemplate(e);
      if (InnerRightContent is null) {
         InnerRightContent = new PathIcon()
         {
            Kind = "ClockCircleOutlined"
         };
      }
      if (_pickerFlyout is null) {
         _pickerFlyout = new TimePickerFlyout(this);
         _flyoutStateHelper.Flyout = _pickerFlyout;
      }
      _textBoxInnerBox = e.NameScope.Get<TextBoxInnerBox>(TextBoxTheme.TextBoxInnerBoxPart);
      _flyoutStateHelper.AnchorTarget = _textBoxInnerBox;
      TokenResourceBinder.CreateGlobalTokenBinding(this, MarginToAnchorProperty, GlobalTokenResourceKey.MarginXXS);
      SetupFlyoutProperties();

   }
   
   protected void SetupFlyoutProperties()
   {
      if (_pickerFlyout is not null) {
         BindUtils.RelayBind(this, PickerPlacementProperty, _pickerFlyout, TimePickerFlyout.PlacementProperty);
         BindUtils.RelayBind(this, IsShowArrowProperty, _pickerFlyout);
         BindUtils.RelayBind(this, IsPointAtCenterProperty, _pickerFlyout);
         BindUtils.RelayBind(this, MarginToAnchorProperty, _pickerFlyout);
      }
   }
   
   protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
   {
      base.OnAttachedToLogicalTree(e);
      BindUtils.RelayBind(this, MouseEnterDelayProperty, _flyoutStateHelper, FlyoutStateHelper.MouseEnterDelayProperty);
      BindUtils.RelayBind(this, MouseLeaveDelayProperty, _flyoutStateHelper, FlyoutStateHelper.MouseLeaveDelayProperty);
   }
   
   protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
   {
      base.OnAttachedToVisualTree(e);
      _flyoutStateHelper.NotifyAttachedToVisualTree();
   }

   protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
   {
      base.OnDetachedFromVisualTree(e);
      _flyoutStateHelper.NotifyDetachedFromVisualTree();
   }
   
   private static int CoerceMinuteIncrement(AvaloniaObject sender, int value)
   {
      if (value < 1 || value > 59) {
         throw new ArgumentOutOfRangeException(null, "1 >= MinuteIncrement <= 59");
      }
      return value;
   }

   private static int CoerceSecondIncrement(AvaloniaObject sender, int value)
   {
      if (value < 1 || value > 59) {
         throw new ArgumentOutOfRangeException(null, "1 >= SecondIncrement <= 59");
      }

      return value;
   }

   private static string CoerceClockIdentifier(AvaloniaObject sender, string value)
   {
      if (!(string.IsNullOrEmpty(value) || value == "12HourClock" || value == "24HourClock")) {
         throw new ArgumentException("Invalid ClockIdentifier", default(string));
      }

      return value;
   }
}