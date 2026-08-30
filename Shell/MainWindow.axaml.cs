using Avalonia.Controls;
using AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;

namespace AvaloniaFunctionKeyTemplate.Shell;

/// <summary>
/// NavigationServiceが選択したページを表示するアプリケーションのShell。
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// XAMLを初期化し、実行時のみNavigationServiceをDataContextへ設定する。
    /// 引数なしコンストラクタにすることでAvaloniaのWindow生成を妨げない。
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            DataContext = ServiceContainer.Resolve<NavigationService>();
        }
    }
}
