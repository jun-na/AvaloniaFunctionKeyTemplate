using System;
using Avalonia.Controls;
using AvaloniaFunctionKeyTemplate.Shared;

namespace AvaloniaFunctionKeyTemplate.Shared.Navigation;

/// <summary>
/// ページ識別子からViewを生成し、Shellに表示する現在のViewを管理する。
/// </summary>
public sealed class NavigationService : ViewModelBase
{
    private readonly Func<PageId, Control> _viewFactory;
    private Control? _currentView;

    /// <summary>
    /// ページ識別子をViewへ変換する明示的なファクトリを受け取る。
    /// </summary>
    /// <param name="viewFactory">ページに対応するViewを生成するファクトリ。</param>
    public NavigationService(Func<PageId, Control> viewFactory)
    {
        ArgumentNullException.ThrowIfNull(viewFactory);
        _viewFactory = viewFactory;
    }

    /// <summary>
    /// ShellのContentControlに現在表示されるView。
    /// </summary>
    public Control? CurrentView
    {
        get => _currentView;
        private set => SetProperty(ref _currentView, value);
    }

    /// <summary>
    /// 指定ページのViewを生成して現在の表示対象へ設定する。
    /// </summary>
    /// <param name="pageId">遷移先のページ識別子。</param>
    public void NavigateTo(PageId pageId)
    {
        CurrentView = _viewFactory(pageId);
    }
}
