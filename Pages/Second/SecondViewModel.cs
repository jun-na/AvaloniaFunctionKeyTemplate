using AvaloniaFunctionKeyTemplate.Shared;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaFunctionKeyTemplate.Pages.Second;

/// <summary>
/// SecondViewの状態とFirstViewへの遷移を管理する。
/// </summary>
public partial class SecondViewModel : ViewModelBase
{
    private readonly NavigationService? _navigationService;

    /// <summary>
    /// XAMLデザイナーがプレビュー用インスタンスを生成するためのコンストラクタ。
    /// 実行時にはDIコンテナが引数ありコンストラクタを使用する。
    /// </summary>
    public SecondViewModel()
    {
    }

    /// <summary>
    /// 実行時に使用する画面遷移サービスを受け取る。
    /// </summary>
    /// <param name="navigationService">表示ページを切り替えるサービス。</param>
    public SecondViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    /// <summary>
    /// 実行時のNavigationServiceが設定されているか判定する。
    /// </summary>
    private bool CanNavigateToFirst() => _navigationService is not null;

    /// <summary>
    /// 表示ページをFirstViewへ切り替える。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateToFirst))]
    private void NavigateToFirst() => _navigationService?.NavigateTo(PageId.First);
}
