namespace AvaloniaFunctionKeyTemplate.Shared.Dialogs;

/// <summary>
/// 共通メッセージダイアログを閉じた操作結果。
/// </summary>
public enum MessageDialogResult
{
    /// <summary>タイトルバーなどから結果を選ばず閉じた。</summary>
    None,

    /// <summary>OKを選択した。</summary>
    Ok,

    /// <summary>「はい」を選択した。</summary>
    Yes,

    /// <summary>「いいえ」を選択した。</summary>
    No,
}
