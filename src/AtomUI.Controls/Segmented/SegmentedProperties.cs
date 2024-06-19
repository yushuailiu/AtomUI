using Avalonia;
using Avalonia.Media;

namespace AtomUI.Controls;

public partial class Segmented
{
   // 获取 Token 值属性开始
   private IBrush? _trackBg;
   private static readonly DirectProperty<Segmented, IBrush?> TrackBgTokenProperty =
      AvaloniaProperty.RegisterDirect<Segmented, IBrush?>(
         nameof(_trackBg),
         o => o._trackBg,
         (o, v) => o._trackBg = v);
   
   private IBrush? _itemSelectedBg;
   private static readonly DirectProperty<Segmented, IBrush?> ItemSelectedBgTokenProperty =
      AvaloniaProperty.RegisterDirect<Segmented, IBrush?>(
         nameof(_itemSelectedBg),
         o => o._itemSelectedBg,
         (o, v) => o._itemSelectedBg = v);
   
   private Thickness _trackPadding;
   private static readonly DirectProperty<Segmented, Thickness> TrackPaddingTokenProperty =
      AvaloniaProperty.RegisterDirect<Segmented, Thickness>(
         nameof(_trackPadding),
         o => o._trackPadding,
         (o, v) => o._trackPadding = v);
   
   private BoxShadows _boxShadowsTertiary;
   private static readonly DirectProperty<Segmented, BoxShadows> BoxShadowsTertiaryTokenProperty =
      AvaloniaProperty.RegisterDirect<Segmented, BoxShadows>(
         nameof(_boxShadowsTertiary),
         o => o._boxShadowsTertiary,
         (o, v) => o._boxShadowsTertiary = v);

   // 获取 Token 值属性结束
   
   // 内部动画属性
   private static readonly StyledProperty<Size> SelectedThumbSizeProperty =
      AvaloniaProperty.Register<Segmented, Size>(nameof(SelectedThumbSize));

   private Size SelectedThumbSize
   {
      get => GetValue(SelectedThumbSizeProperty);
      set => SetValue(SelectedThumbSizeProperty, value);
   }
   
   private static readonly StyledProperty<Point> SelectedThumbPosProperty =
      AvaloniaProperty.Register<Segmented, Point>(nameof(SelectedThumbPos));

   private Point SelectedThumbPos
   {
      get => GetValue(SelectedThumbPosProperty);
      set => SetValue(SelectedThumbPosProperty, value);
   }
   
   public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
      AvaloniaProperty.Register<Segmented, CornerRadius>(nameof(CornerRadius));

   public CornerRadius CornerRadius
   {
      get => GetValue(CornerRadiusProperty);
      set => SetValue(CornerRadiusProperty, value);
   }
   
   public static readonly StyledProperty<CornerRadius> SelectedThumbCornerRadiusProperty =
      AvaloniaProperty.Register<StyledControl, CornerRadius>(nameof(SelectedThumbCornerRadius));
   public CornerRadius SelectedThumbCornerRadius
   {
      get => GetValue(SelectedThumbCornerRadiusProperty);
      set => SetValue(SelectedThumbCornerRadiusProperty, value);
   }
}