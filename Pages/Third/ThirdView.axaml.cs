using Avalonia.Controls;
using AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;

namespace AvaloniaFunctionKeyTemplate.Pages.Third;

/// <summary>
/// Windows XP風テーマを適用したコントロールを一覧表示するページ。
/// </summary>
public partial class ThirdView : UserControl
{
    /// <summary>
    /// XAMLを初期化し、実行時のみViewModelをDIコンテナから設定する。
    /// 引数なしコンストラクタにすることでAvaloniaのView生成を妨げない。
    /// </summary>
    public ThirdView()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            DataContext = ServiceContainer.Resolve<ThirdViewModel>();
        }
    }
}
