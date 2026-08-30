using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Input;
using AvaloniaFunctionKeyTemplate.Shared;
using AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaFunctionKeyTemplate.Pages.Third;

/// <summary>
/// ThirdViewのサンプル状態、操作、FirstViewへの遷移を管理する。
/// </summary>
public partial class ThirdViewModel : ViewModelBase, IFunctionKeyProvider
{
    private readonly NavigationService? _navigationService;
    private int _executionCount;

    /// <summary>
    /// XAMLデザイナーがプレビュー用インスタンスを生成するためのコンストラクタ。
    /// 実行時にはPure.DIが引数ありコンストラクタを使用する。
    /// </summary>
    public ThirdViewModel()
    {
    }

    /// <summary>
    /// 実行時に使用する画面遷移サービスを受け取る。
    /// </summary>
    /// <param name="navigationService">表示ページを切り替えるサービス。</param>
    public ThirdViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    /// <summary>
    /// ListBoxへ表示するサンプル項目。
    /// </summary>
    public ObservableCollection<string> SampleItems { get; } =
    [
        "受注入力",
        "売上照会",
        "顧客マスタ",
        "商品マスタ",
        "月次集計",
    ];

    /// <summary>
    /// ThirdViewで使用するF2のサンプル実行処理とF11の戻る処理を提供する。
    /// </summary>
    public IReadOnlyList<FunctionKeyBinding> FunctionKeys =>
    [
        new(Key.F2, "実行", RunSampleCommand),
        new(Key.F11, "戻る", NavigateToFirstCommand),
    ];

    /// <summary>
    /// TextBoxの双方向バインディングを確認するサンプル文字列。
    /// </summary>
    [ObservableProperty]
    public partial string SampleText { get; set; } = "入力サンプル";

    /// <summary>
    /// ギャラリー内のサンプル操作結果を表示する。
    /// </summary>
    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "F2または「実行」で動作確認";

    /// <summary>
    /// 実行時のNavigationServiceが設定されているか判定する。
    /// </summary>
    private bool CanNavigateToFirst() => _navigationService is not null;

    /// <summary>
    /// 表示ページをFirstViewへ戻す。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateToFirst))]
    private void NavigateToFirst() => _navigationService?.NavigateTo(PageId.First);

    /// <summary>
    /// ボタンまたはF2から呼び出され、実行回数と入力値をステータスへ反映する。
    /// </summary>
    [RelayCommand]
    private void RunSample()
    {
        _executionCount++;
        StatusMessage = $"{_executionCount}回目の実行: {SampleText}";
    }
}
