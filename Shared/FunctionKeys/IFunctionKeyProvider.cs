using System.Collections.Generic;

namespace AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;

/// <summary>
/// 表示中のページが使用するファンクションキー設定を提供する。
/// </summary>
public interface IFunctionKeyProvider
{
    /// <summary>
    /// ページ固有のファンクションキー設定。
    /// 未指定のキーは空欄かつ無効になる。
    /// </summary>
    IReadOnlyList<FunctionKeyBinding> FunctionKeys { get; }
}
