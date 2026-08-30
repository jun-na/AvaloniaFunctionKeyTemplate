using System;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace AvaloniaFunctionKeyTemplate.Shared.Dialogs;

/// <summary>
/// 共通メッセージダイアログを用途別の簡潔なメソッドで表示する。
/// </summary>
public sealed class MessageDialogService
{
    /// <summary>
    /// Yes／No形式の確認ダイアログを表示する。
    /// </summary>
    /// <param name="owner">モーダル表示の所有元Window。</param>
    /// <param name="title">ダイアログのタイトル。</param>
    /// <param name="message">確認する本文。</param>
    /// <returns>「はい」を選択した場合はtrue。</returns>
    public async Task<bool> ConfirmAsync(
        Window owner,
        string title,
        string message)
    {
        var result = await ShowAsync(
            owner,
            new MessageDialogOptions(
                title,
                message,
                MessageDialogKind.Confirm,
                MessageDialogButtons.YesNo));

        return result is MessageDialogResult.Yes;
    }

    /// <summary>
    /// 情報ダイアログを表示し、OKで閉じるまで待機する。
    /// </summary>
    /// <param name="owner">モーダル表示の所有元Window。</param>
    /// <param name="title">ダイアログのタイトル。</param>
    /// <param name="message">案内する本文。</param>
    public Task InfoAsync(Window owner, string title, string message) =>
        ShowOkAsync(owner, title, message, MessageDialogKind.Information);

    /// <summary>
    /// 警告ダイアログを表示し、OKで閉じるまで待機する。
    /// </summary>
    /// <param name="owner">モーダル表示の所有元Window。</param>
    /// <param name="title">ダイアログのタイトル。</param>
    /// <param name="message">警告する本文。</param>
    public Task WarningAsync(Window owner, string title, string message) =>
        ShowOkAsync(owner, title, message, MessageDialogKind.Warning);

    /// <summary>
    /// エラーダイアログを表示し、OKで閉じるまで待機する。
    /// </summary>
    /// <param name="owner">モーダル表示の所有元Window。</param>
    /// <param name="title">ダイアログのタイトル。</param>
    /// <param name="message">エラー内容。</param>
    public Task ErrorAsync(Window owner, string title, string message) =>
        ShowOkAsync(owner, title, message, MessageDialogKind.Error);

    /// <summary>
    /// 任意の種別とボタン構成で共通メッセージダイアログを表示する。
    /// </summary>
    /// <param name="owner">モーダル表示の所有元Window。</param>
    /// <param name="options">表示内容とボタン構成。</param>
    /// <returns>ユーザーが選択したボタンの結果。</returns>
    public Task<MessageDialogResult> ShowAsync(
        Window owner,
        MessageDialogOptions options)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(options);

        var dialog = new MessageDialogWindow(options);
        return dialog.ShowDialog<MessageDialogResult>(owner);
    }

    /// <summary>
    /// 指定種別をOKボタン構成で表示する。
    /// </summary>
    /// <param name="owner">モーダル表示の所有元Window。</param>
    /// <param name="title">ダイアログのタイトル。</param>
    /// <param name="message">表示する本文。</param>
    /// <param name="kind">情報、警告、エラーの種別。</param>
    private async Task ShowOkAsync(
        Window owner,
        string title,
        string message,
        MessageDialogKind kind)
    {
        await ShowAsync(
            owner,
            new MessageDialogOptions(
                title,
                message,
                kind,
                MessageDialogButtons.Ok));
    }
}
