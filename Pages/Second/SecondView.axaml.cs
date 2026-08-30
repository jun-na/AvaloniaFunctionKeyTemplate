using Avalonia.Controls;
using AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;

namespace AvaloniaFunctionKeyTemplate.Pages.Second;

/// <summary>
/// FirstViewへ戻る操作を提供する2番目のページ。
/// </summary>
public partial class SecondView : UserControl
{
    /// <summary>
    /// XAMLを初期化し、実行時のみViewModelをPure.DIから設定する。
    /// 引数なしコンストラクタにすることでAvaloniaのView生成を妨げない。
    /// </summary>
    public SecondView()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            DataContext = AppServices.Resolve<SecondViewModel>();
        }
    }
}
