﻿using AtomUI.Theme.Styling;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace AtomUI.Controls;

public class LoadingIndicatorAdorner : TemplatedControl, IControlCustomStyle
{
    private readonly IControlCustomStyle _customStyle;
    private LoadingIndicator? _loadingIndicator;

    public EventHandler<LoadingIndicatorCreatedEventArgs>? IndicatorCreated;

    public LoadingIndicatorAdorner()
    {
        _customStyle = this;
    }

    void IControlCustomStyle.HandleTemplateApplied(INameScope scope)
    {
        _loadingIndicator = scope.Find<LoadingIndicator>(LoadingIndicatorAdornerTheme.LoadingIndicatorPart);
        if (_loadingIndicator is not null)
        {
            IndicatorCreated?.Invoke(this, new LoadingIndicatorCreatedEventArgs(_loadingIndicator));
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _customStyle.HandleTemplateApplied(e.NameScope);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var offsetX = (finalSize.Width - _loadingIndicator!.DesiredSize.Width) / 2;
        var offsetY = (finalSize.Height - _loadingIndicator.DesiredSize.Height) / 2;
        Canvas.SetLeft(_loadingIndicator, offsetX);
        Canvas.SetTop(_loadingIndicator, offsetY);
        return base.ArrangeOverride(finalSize);
    }
}

public class LoadingIndicatorCreatedEventArgs : EventArgs
{
    public LoadingIndicatorCreatedEventArgs(LoadingIndicator indicator)
    {
        LoadingIndicator = indicator;
    }

    public LoadingIndicator LoadingIndicator { get; set; }
}