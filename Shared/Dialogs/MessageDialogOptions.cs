namespace AvaloniaFunctionKeyTemplate.Shared.Dialogs;

/// <summary>
/// 共通メッセージダイアログへ渡す表示内容とボタン構成。
/// </summary>
/// <param name="Title">タイトルバーとコピー内容へ使用するタイトル。</param>
/// <param name="Message">本文へ表示し、クリップボードへコピーできる文言。</param>
/// <param name="Kind">アイコンと強調色を決める表示種別。</param>
/// <param name="Buttons">OKまたはYes／Noのボタン構成。</param>
public sealed record MessageDialogOptions(
    string Title,
    string Message,
    MessageDialogKind Kind,
    MessageDialogButtons Buttons);
