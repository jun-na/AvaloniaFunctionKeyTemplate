using Avalonia.Controls;
using AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;
using Avalonia.Interactivity;

namespace AvaloniaFunctionKeyTemplate.Pages.First;

/// <summary>
/// ToDoの一覧表示と追加操作を提供する最初のページ。
/// </summary>
public partial class FirstView : UserControl
{
    /// <summary>
    /// XAMLを初期化し、実行時のみViewModelをPure.DIから設定する。
    /// 引数なしコンストラクタにすることでAvaloniaのView生成を妨げない。
    /// </summary>
    public FirstView()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            DataContext = AppServices.Resolve<FirstViewModel>();
        }
    }

    /// <summary>
    /// Viewの表示時にToDo一覧の読み込みコマンドを開始する。
    /// </summary>
    /// <param name="sender">イベントを発生させたView。</param>
    /// <param name="e">Loadedイベントの情報。</param>
    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is FirstViewModel viewModel)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
