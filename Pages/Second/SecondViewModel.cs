using System.Collections.Generic;
using Avalonia.Input;
using AvaloniaFunctionKeyTemplate.Shared;
using AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaFunctionKeyTemplate.Pages.Second;

/// <summary>
/// SecondViewの状態とFirstViewへの遷移を管理する。
/// </summary>
public partial class SecondViewModel : ViewModelBase, IFunctionKeyProvider
{
    private readonly NavigationService? _navigationService;
    private int _count;

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
    /// SecondViewで使用するF1の戻る処理とF2のカウント処理を提供する。
    /// </summary>
    public IReadOnlyList<FunctionKeyBinding> FunctionKeys =>
    [
        new(Key.F1, "戻る", NavigateToFirstCommand),
        new(Key.F2, "カウント", CountCommand),
    ];

    /// <summary>
    /// F2を実行した結果を画面へ表示する。
    /// </summary>
    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "F2キーでカウント";

    /// <summary>
    /// 実行時のNavigationServiceが設定されているか判定する。
    /// </summary>
    private bool CanNavigateToFirst() => _navigationService is not null;

    /// <summary>
    /// 表示ページをFirstViewへ切り替える。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateToFirst))]
    private void NavigateToFirst() => _navigationService?.NavigateTo(PageId.First);

    /// <summary>
    /// F2またはファンクションキーバーのクリックごとにカウントを増やす。
    /// </summary>
    [RelayCommand]
    private void Count()
    {
        _count++;
        StatusMessage = $"F2を{_count}回実行";
    }
}
