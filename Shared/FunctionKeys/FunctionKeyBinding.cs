using System.Windows.Input;
using Avalonia.Input;

namespace AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;

/// <summary>
/// 1つのファンクションキーに表示名と処理を割り当てる。
/// </summary>
/// <param name="Key">割り当て対象のF1からF12までのキー。</param>
/// <param name="Label">ファンクションキーバーに表示する名称。</param>
/// <param name="Command">クリックまたは物理キー入力で実行するCommand。</param>
public sealed record FunctionKeyBinding(Key Key, string Label, ICommand Command);
