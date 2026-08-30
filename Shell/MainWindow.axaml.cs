using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;
using AvaloniaFunctionKeyTemplate.Shared.Dialogs;
using AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;

namespace AvaloniaFunctionKeyTemplate.Shell;

/// <summary>
/// NavigationServiceが選択したページを表示するアプリケーションのShell。
/// </summary>
public partial class MainWindow : Window
{
    private readonly FunctionKeyService? _functionKeyService;
    private readonly MessageDialogService? _messageDialogService;
    private readonly MainWindowViewModel? _viewModel;
    private bool _hasStartedUpdateCheck;

    /// <summary>
    /// XAMLを初期化し、実行時のみMainWindowViewModelをDataContextへ設定する。
    /// 引数なしコンストラクタにすることでAvaloniaのWindow生成を妨げない。
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            _functionKeyService = AppServices.Resolve<FunctionKeyService>();
            _messageDialogService = AppServices.Resolve<MessageDialogService>();
            _viewModel = AppServices.Resolve<MainWindowViewModel>();
            DataContext = _viewModel;
            AddHandler(KeyDownEvent, OnFunctionKeyDown, RoutingStrategies.Tunnel);
            Opened += OnOpened;
        }
    }

    /// <summary>
    /// Shellが表示された後に、画面を止めず更新確認を開始する。
    /// </summary>
    /// <param name="sender">表示されたMainWindow。</param>
    /// <param name="e">Openedイベントの情報。</param>
    private async void OnOpened(object? sender, EventArgs e)
    {
        if (_hasStartedUpdateCheck || _viewModel is null)
        {
            return;
        }

        _hasStartedUpdateCheck = true;
        await _viewModel.CheckForUpdatesAsync();
    }

    /// <summary>
    /// FirstView上の更新通知が押されたとき、確認ダイアログを表示して更新を開始する。
    /// </summary>
    /// <param name="sender">更新通知ボタン。</param>
    /// <param name="e">クリックイベントの情報。</param>
    private async void OnUpdateNotificationClick(object? sender, RoutedEventArgs e)
    {
        if (_viewModel is null ||
            _messageDialogService is null ||
            !_viewModel.IsUpdateNotificationVisible ||
            !_viewModel.IsUpdateNotificationEnabled)
        {
            return;
        }

        var shouldUpdate = await _messageDialogService.ConfirmAsync(
            this,
            "アプリケーションの更新",
            $"新しいバージョン {_viewModel.Updates.AvailableVersion} へ更新します。"
            + $"{Environment.NewLine}{Environment.NewLine}"
            + "更新データを取得後、アプリを再起動します。");

        if (shouldUpdate)
        {
            var updateStarted = await _viewModel.UpdateAndRestartAsync();
            if (!updateStarted)
            {
                await _messageDialogService.ErrorAsync(
                    this,
                    "更新エラー",
                    "更新を適用できませんでした。ネットワーク接続を確認して再試行してください。");
            }
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
