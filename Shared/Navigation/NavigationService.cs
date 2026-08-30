using System;
using Avalonia.Controls;
using AvaloniaFunctionKeyTemplate.Shared;
using AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;

namespace AvaloniaFunctionKeyTemplate.Shared.Navigation;

/// <summary>
/// ページ識別子からViewを生成し、現在のViewとファンクションキー設定を切り替える。
/// </summary>
public sealed class NavigationService : ViewModelBase
{
    private readonly Func<PageId, Control> _viewFactory;
    private readonly FunctionKeyService _functionKeyService;
    private Control? _currentView;

    /// <summary>
    /// ページ識別子をViewへ変換する明示的なファクトリを受け取る。
    /// </summary>
    /// <param name="viewFactory">ページに対応するViewを生成するファクトリ。</param>
    /// <param name="functionKeyService">表示ページのファンクションキー設定を管理するサービス。</param>
    public NavigationService(
        Func<PageId, Control> viewFactory,
        FunctionKeyService functionKeyService)
    {
        ArgumentNullException.ThrowIfNull(viewFactory);
        ArgumentNullException.ThrowIfNull(functionKeyService);
        _viewFactory = viewFactory;
        _functionKeyService = functionKeyService;
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
        var view = _viewFactory(pageId);
        _functionKeyService.Activate(view.DataContext as IFunctionKeyProvider);
        CurrentView = view;
    }
}
