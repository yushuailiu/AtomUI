﻿using AtomUI.Data;
using AtomUI.Theme;
using AtomUI.Theme.Data;
using AtomUI.Theme.Styling;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;

namespace AtomUI.Controls;

[ControlThemeProvider]
internal class RangeTimePickerTheme : BaseControlTheme
{
   public const string DecoratedBoxPart = "PART_DecoratedBox";
   public const string RangePickerInnerPart = "PART_RangePickerInner";
   public const string RangeStartTextBoxPart = "PART_RangeStartTextBox";
   public const string RangeEndTextBoxPart = "PART_RangeEndTextBox";
   public const string RangePickerArrowPart = "PART_RangePickerArrow";
   public const string RangePickerIndicatorPart = "PART_RangePickerIndicator";
   
   public RangeTimePickerTheme() : base(typeof(RangeTimePicker)) {}
   
   protected override IControlTemplate BuildControlTemplate()
   {
      return new FuncControlTemplate<RangeTimePicker>((rangeTimePicker, scope) =>
      {
         var indicatorLayout = new Panel();
         var canvas = new Canvas();
         var decoratedBox = BuildRangePickerDecoratedBox(rangeTimePicker, scope);
         var innerBox = BuildPickerContent(rangeTimePicker, scope);
         var pickerIndicator = BuildPickerIndicator(rangeTimePicker, scope);
         canvas.Children.Add(pickerIndicator);
         decoratedBox.Content = innerBox;
         indicatorLayout.Children.Add(decoratedBox);
         indicatorLayout.Children.Add(canvas);
         return indicatorLayout;
      });
   }
   
   protected virtual AddOnDecoratedBox BuildRangePickerDecoratedBox(RangeTimePicker rangeTimePicker, INameScope scope)
   {
      var decoratedBox = new AddOnDecoratedBox
      {
         Name = DecoratedBoxPart,
         Focusable = true
      };
      CreateTemplateParentBinding(decoratedBox, ButtonSpinnerDecoratedBox.StyleVariantProperty, RangeTimePicker.StyleVariantProperty);
      CreateTemplateParentBinding(decoratedBox, ButtonSpinnerDecoratedBox.SizeTypeProperty, RangeTimePicker.SizeTypeProperty);
      CreateTemplateParentBinding(decoratedBox, ButtonSpinnerDecoratedBox.StatusProperty, RangeTimePicker.StatusProperty);
      CreateTemplateParentBinding(decoratedBox, ButtonSpinnerDecoratedBox.LeftAddOnProperty, RangeTimePicker.LeftAddOnProperty);
      CreateTemplateParentBinding(decoratedBox, ButtonSpinnerDecoratedBox.RightAddOnProperty, RangeTimePicker.RightAddOnProperty);
      decoratedBox.RegisterInNameScope(scope);
      return decoratedBox;
   }
   
   private AddOnDecoratedInnerBox BuildPickerContent(RangeTimePicker rangeTimePicker, INameScope scope)
   {
      var rangePickerInnerBox = new AddOnDecoratedInnerBox
      {
         Name = RangePickerInnerPart,
         HorizontalAlignment = HorizontalAlignment.Stretch,
         VerticalContentAlignment = VerticalAlignment.Stretch
      };
      rangePickerInnerBox.RegisterInNameScope(scope);
      CreateTemplateParentBinding(rangePickerInnerBox, ButtonSpinnerInnerBox.LeftAddOnContentProperty, RangeTimePicker.InnerLeftContentProperty);
      CreateTemplateParentBinding(rangePickerInnerBox, ButtonSpinnerInnerBox.RightAddOnContentProperty, RangeTimePicker.InnerRightContentProperty);
      CreateTemplateParentBinding(rangePickerInnerBox, ButtonSpinnerInnerBox.StyleVariantProperty, RangeTimePicker.StyleVariantProperty);
      CreateTemplateParentBinding(rangePickerInnerBox, ButtonSpinnerInnerBox.StatusProperty, RangeTimePicker.StatusProperty);
      CreateTemplateParentBinding(rangePickerInnerBox, ButtonSpinnerInnerBox.SizeTypeProperty, RangeTimePicker.SizeTypeProperty);

      var rangeLayout = new Grid()
      {
         ColumnDefinitions = new ColumnDefinitions()
         {
            new ColumnDefinition(GridLength.Star),
            new ColumnDefinition(GridLength.Auto),
            new ColumnDefinition(GridLength.Star),
         }
      };

      var arrowIcon = new PathIcon()
      {
         Kind = "SwapRightOutlined",
         Name = RangePickerArrowPart
      };

      TokenResourceBinder.CreateGlobalTokenBinding(arrowIcon, PathIcon.HeightProperty, GlobalTokenResourceKey.IconSize);
      TokenResourceBinder.CreateGlobalTokenBinding(arrowIcon, PathIcon.WidthProperty, GlobalTokenResourceKey.IconSize);
      TokenResourceBinder.CreateGlobalTokenBinding(arrowIcon, PathIcon.NormalFilledBrushProperty, GlobalTokenResourceKey.ColorTextQuaternary);

      var rangeStartTextBox = BuildPickerTextBox(RangeStartTextBoxPart);
      CreateTemplateParentBinding(rangeStartTextBox, TextBox.WatermarkProperty, RangeTimePicker.RangeStartWatermarkProperty);
      
      var rangeEndTextBox = BuildPickerTextBox(RangeEndTextBoxPart);
      CreateTemplateParentBinding(rangeEndTextBox, TextBox.WatermarkProperty, RangeTimePicker.RangeEndWatermarkProperty);
      
      rangeStartTextBox.RegisterInNameScope(scope);
      rangeEndTextBox.RegisterInNameScope(scope);
      
      rangeLayout.Children.Add(rangeStartTextBox);
      rangeLayout.Children.Add(arrowIcon);
      rangeLayout.Children.Add(rangeEndTextBox);
      
      Grid.SetColumn(rangeStartTextBox, 0);
      Grid.SetColumn(arrowIcon, 1);
      Grid.SetColumn(rangeEndTextBox, 2);
      
      rangePickerInnerBox.Content = rangeLayout;
      
      return rangePickerInnerBox;
   }

   private Rectangle BuildPickerIndicator(RangeTimePicker rangeTimePicker, INameScope scope)
   {
      var indicator = new Rectangle()
      {
         Name = RangePickerIndicatorPart,
         Opacity = 0
      };
      indicator.RegisterInNameScope(scope);
      return indicator;
   }

   private TextBox BuildPickerTextBox(string name)
   {
      var rangeStartTextBox = new TextBox()
      {
         Name = name,
         VerticalContentAlignment = VerticalAlignment.Center,
         VerticalAlignment = VerticalAlignment.Stretch,
         HorizontalAlignment = HorizontalAlignment.Stretch,
         BorderThickness = new Thickness(0),
         TextWrapping = TextWrapping.NoWrap,
         AcceptsReturn = false,
         EmbedMode = true
      };

      BindUtils.RelayBind(this, DataValidationErrors.ErrorsProperty, rangeStartTextBox, DataValidationErrors.ErrorsProperty);
      CreateTemplateParentBinding(rangeStartTextBox, TextBox.SizeTypeProperty, NumericUpDown.SizeTypeProperty);
      CreateTemplateParentBinding(rangeStartTextBox, TextBox.IsReadOnlyProperty, NumericUpDown.IsReadOnlyProperty);
      CreateTemplateParentBinding(rangeStartTextBox, TextBox.TextProperty, NumericUpDown.TextProperty);
      CreateTemplateParentBinding(rangeStartTextBox, TextBox.WatermarkProperty, NumericUpDown.WatermarkProperty);
      CreateTemplateParentBinding(rangeStartTextBox, TextBox.IsEnableClearButtonProperty, NumericUpDown.IsEnableClearButtonProperty);
      
      return rangeStartTextBox;
   }

   protected override void BuildStyles()
   {
      var commonStyle = new Style(selector => selector.Nesting());
      
      var arrowStyle = new Style(selector => selector.Nesting().Template().Name(RangePickerArrowPart));
      arrowStyle.Add(PathIcon.MarginProperty, TimePickerTokenResourceKey.RangePickerArrowMargin);
      
      commonStyle.Add(arrowStyle);
      
      var largeStyle =
         new Style(selector => selector.Nesting().PropertyEquals(AddOnDecoratedBox.SizeTypeProperty, SizeType.Large));
      largeStyle.Add(AddOnDecoratedBox.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadiusLG);
      commonStyle.Add(largeStyle);

      var middleStyle =
         new Style(selector => selector.Nesting().PropertyEquals(AddOnDecoratedBox.SizeTypeProperty, SizeType.Middle));
      middleStyle.Add(AddOnDecoratedBox.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadius);
      commonStyle.Add(middleStyle);

      var smallStyle =
         new Style(selector => selector.Nesting().PropertyEquals(AddOnDecoratedBox.SizeTypeProperty, SizeType.Small));
      smallStyle.Add(AddOnDecoratedBox.CornerRadiusProperty, GlobalTokenResourceKey.BorderRadiusSM);
      commonStyle.Add(smallStyle);
      
      Add(commonStyle);

      BuildIndicatorStyle(commonStyle);
   }

   private void BuildIndicatorStyle(Style commonStyle)
   {
      {
         var indicatorStyle = new Style(selector => selector.Nesting().Template().Name(RangePickerIndicatorPart));
         indicatorStyle.Add(Rectangle.HeightProperty, TimePickerTokenResourceKey.RangePickerIndicatorThickness);
         commonStyle.Add(indicatorStyle);
      }
      
      var defaultStyle = new Style(selector => selector.Nesting().PropertyEquals(AddOnDecoratedInnerBox.StatusProperty, AddOnDecoratedStatus.Default));
      
      {
         var indicatorStyle = new Style(selector => selector.Nesting().Template().Name(RangePickerIndicatorPart));
         indicatorStyle.Add(Rectangle.FillProperty, GlobalTokenResourceKey.ColorPrimary);
      
         defaultStyle.Add(indicatorStyle);
      }
      commonStyle.Add(defaultStyle);
      
      var errorStyle = new Style(selector => selector.Nesting().PropertyEquals(AddOnDecoratedInnerBox.StatusProperty, AddOnDecoratedStatus.Error));
      
      {
         var indicatorStyle = new Style(selector => selector.Nesting().Template().Name(RangePickerIndicatorPart));
         indicatorStyle.Add(Rectangle.FillProperty, GlobalTokenResourceKey.ColorError);
         errorStyle.Add(indicatorStyle);
      }
      
      commonStyle.Add(errorStyle);
      
      var warningStyle = new Style(selector => selector.Nesting().PropertyEquals(AddOnDecoratedInnerBox.StatusProperty, AddOnDecoratedStatus.Warning));
      
      {
         var indicatorStyle = new Style(selector => selector.Nesting().Template().Name(RangePickerIndicatorPart));
         indicatorStyle.Add(Rectangle.FillProperty, GlobalTokenResourceKey.ColorWarning);
         warningStyle.Add(indicatorStyle);
      }
      
      commonStyle.Add(warningStyle);
   }
}