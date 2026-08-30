using Avalonia.Controls;
using AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;
using AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;

namespace AvaloniaFunctionKeyTemplate.Shell;

/// <summary>
/// Shell下部にF1からF12までのページ固有操作を表示する。
/// </summary>
public partial class FunctionKeyBar : UserControl
{
    /// <summary>
    /// XAMLを初期化し、実行時のキー状態をPure.DIから設定する。
    /// </summary>
    public FunctionKeyBar()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            DataContext = AppServices.Resolve<FunctionKeyService>();
        }
    }
}
