using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;
using AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;

namespace AvaloniaFunctionKeyTemplate.Shell;

/// <summary>
/// NavigationServiceが選択したページを表示するアプリケーションのShell。
/// </summary>
public partial class MainWindow : Window
{
    private readonly FunctionKeyService? _functionKeyService;

    /// <summary>
    /// XAMLを初期化し、実行時のみNavigationServiceをDataContextへ設定する。
    /// 引数なしコンストラクタにすることでAvaloniaのWindow生成を妨げない。
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            _functionKeyService = ServiceContainer.Resolve<FunctionKeyService>();
            DataContext = ServiceContainer.Resolve<NavigationService>();
            AddHandler(KeyDownEvent, OnFunctionKeyDown, RoutingStrategies.Tunnel);
        }
    }

    /// <summary>
    /// TextBoxなどにフォーカスがあっても修飾キーなしのF1からF12を捕捉する。
    /// CtrlやShift付きのキー入力はDeveloper Toolsなど別用途へ渡す。
    /// </summary>
    /// <param name="sender">キー入力を受け取ったMainWindow。</param>
    /// <param name="e">入力されたキーの情報。</param>
    private void OnFunctionKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyModifiers is KeyModifiers.None &&
            _functionKeyService?.Execute(e.Key) is true)
        {
            e.Handled = true;
        }
    }
}
