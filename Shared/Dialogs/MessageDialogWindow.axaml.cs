using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;

namespace AvaloniaFunctionKeyTemplate.Shared.Dialogs;

/// <summary>
/// Confirm、Information、Warning、Errorを共通表示するモーダルWindow。
/// </summary>
public partial class MessageDialogWindow : Window
{
    private readonly string _copyText = string.Empty;
    private readonly MessageDialogButtons _buttons;

    /// <summary>
    /// XAMLデザイナー用の表示内容でWindowを初期化する。
    /// </summary>
    public MessageDialogWindow()
        : this(new MessageDialogOptions(
            "メッセージ",
            "ここにメッセージを表示します。",
            MessageDialogKind.Information,
            MessageDialogButtons.Ok))
    {
    }

    /// <summary>
    /// 指定された種別、本文、ボタン構成でWindowを初期化する。
    /// </summary>
    /// <param name="options">表示内容とボタン構成。</param>
    public MessageDialogWindow(MessageDialogOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        InitializeComponent();

        Title = options.Title;
        HeadingText.Text = options.Title;
        MessageText.Text = options.Message;
        _copyText = $"{options.Title}{Environment.NewLine}{Environment.NewLine}{options.Message}";
        _buttons = options.Buttons;

        ConfigureKind(options.Kind);
        ConfigureButtons(options.Buttons);

        Opened += OnOpened;
        AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
    }

    /// <summary>
    /// ダイアログ種別に対応する記号と色クラスを設定する。
    /// </summary>
    /// <param name="kind">表示するダイアログ種別。</param>
    private void ConfigureKind(MessageDialogKind kind)
    {
        var (icon, styleClass) = kind switch
        {
            MessageDialogKind.Confirm => ("?", "confirm"),
            MessageDialogKind.Information => ("i", "information"),
            MessageDialogKind.Warning => ("!", "warning"),
            MessageDialogKind.Error => ("×", "error"),
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

        IconText.Text = icon;
        IconBorder.Classes.Add(styleClass);
    }

    /// <summary>
    /// OKまたはYes／Noのうち、指定されたボタンだけを表示する。
    /// </summary>
    /// <param name="buttons">表示するボタン構成。</param>
    private void ConfigureButtons(MessageDialogButtons buttons)
    {
        OkButton.IsVisible = buttons is MessageDialogButtons.Ok;
        YesButton.IsVisible = buttons is MessageDialogButtons.YesNo;
        NoButton.IsVisible = buttons is MessageDialogButtons.YesNo;
    }

    /// <summary>
    /// 表示直後にOKまたは「はい」へキーボードフォーカスを設定する。
    /// </summary>
    /// <param name="sender">表示されたMessageDialogWindow。</param>
    /// <param name="e">Openedイベントの情報。</param>
    private void OnOpened(object? sender, EventArgs e)
    {
        GetPrimaryButton().Focus();
    }

    /// <summary>
    /// Enterで選択中の結果を確定し、Yes／No形式では左右キーでフォーカスを移動する。
    /// Escapeは「いいえ」またはOKとして閉じる。
    /// </summary>
    /// <param name="sender">キー入力を受け取ったMessageDialogWindow。</param>
    /// <param name="e">キー入力の情報。</param>
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyModifiers is not KeyModifiers.None)
        {
            return;
        }

        if (_buttons is MessageDialogButtons.YesNo &&
            e.Key is Key.Left or Key.Right)
        {
            if (YesButton.IsFocused)
            {
                NoButton.Focus();
            }
            else
            {
                YesButton.Focus();
            }

            e.Handled = true;
            return;
        }

        if (e.Key is Key.Enter)
        {
            Close(GetFocusedResult());
            e.Handled = true;
            return;
        }

        if (e.Key is Key.Escape)
        {
            Close(_buttons is MessageDialogButtons.YesNo
                ? MessageDialogResult.No
                : MessageDialogResult.Ok);
            e.Handled = true;
        }
    }

    /// <summary>
    /// タイトルと本文をOSのクリップボードへコピーする。
    /// </summary>
    /// <param name="sender">押されたコピーボタン。</param>
    /// <param name="e">クリックイベントの情報。</param>
    private async void OnCopyClick(object? sender, RoutedEventArgs e)
    {
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard is null)
        {
            CopyButton.Content = "コピー失敗";
            return;
        }

        try
        {
            await clipboard.SetTextAsync(_copyText);
            CopyButton.Content = "コピー済み";
        }
        catch (Exception)
        {
            CopyButton.Content = "コピー失敗";
        }
    }

    /// <summary>
    /// OK結果を返してダイアログを閉じる。
    /// </summary>
    /// <param name="sender">押されたOKボタン。</param>
    /// <param name="e">クリックイベントの情報。</param>
    private void OnOkClick(object? sender, RoutedEventArgs e)
    {
        Close(MessageDialogResult.Ok);
    }

    /// <summary>
    /// Yes結果を返してダイアログを閉じる。
    /// </summary>
    /// <param name="sender">押された「はい」ボタン。</param>
    /// <param name="e">クリックイベントの情報。</param>
    private void OnYesClick(object? sender, RoutedEventArgs e)
    {
        Close(MessageDialogResult.Yes);
    }

    /// <summary>
    /// No結果を返してダイアログを閉じる。
    /// </summary>
    /// <param name="sender">押された「いいえ」ボタン。</param>
    /// <param name="e">クリックイベントの情報。</param>
    private void OnNoClick(object? sender, RoutedEventArgs e)
    {
        Close(MessageDialogResult.No);
    }

    /// <summary>
    /// 現在のボタン構成で最初にフォーカスするボタンを返す。
    /// </summary>
    /// <returns>OKまたは「はい」ボタン。</returns>
    private Button GetPrimaryButton() =>
        _buttons is MessageDialogButtons.YesNo ? YesButton : OkButton;

    /// <summary>
    /// 現在フォーカスされているボタンに対応する結果を返す。
    /// </summary>
    /// <returns>OK、Yes、Noのいずれか。</returns>
    private MessageDialogResult GetFocusedResult()
    {
        if (_buttons is MessageDialogButtons.Ok)
        {
            return MessageDialogResult.Ok;
        }

        return NoButton.IsFocused
            ? MessageDialogResult.No
            : MessageDialogResult.Yes;
    }
}
