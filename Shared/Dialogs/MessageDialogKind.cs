namespace AvaloniaFunctionKeyTemplate.Shared.Dialogs;

/// <summary>
/// 共通メッセージダイアログの表示種別。
/// </summary>
public enum MessageDialogKind
{
    /// <summary>操作の確認。</summary>
    Confirm,

    /// <summary>通常の案内。</summary>
    Information,

    /// <summary>注意が必要な案内。</summary>
    Warning,

    /// <summary>処理失敗などのエラー。</summary>
    Error,
}
