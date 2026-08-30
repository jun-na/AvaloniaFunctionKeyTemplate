using System.Windows.Input;
using Avalonia.Input;

namespace AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;

/// <summary>
/// ファンクションキーバーに表示される1キー分の状態を保持する。
/// </summary>
public sealed class FunctionKeySlot : ViewModelBase
{
    private string _label = string.Empty;
    private ICommand? _command;

    /// <summary>
    /// 対応する物理ファンクションキーを設定する。
    /// </summary>
    /// <param name="key">F1からF12までのキー。</param>
    public FunctionKeySlot(Key key)
    {
        Key = key;
    }

    /// <summary>
    /// 対応する物理ファンクションキー。
    /// </summary>
    public Key Key { get; }

    /// <summary>
    /// キー下段に表示するページ固有の名称。
    /// </summary>
    public string Label
    {
        get => _label;
        private set => SetProperty(ref _label, value);
    }

    /// <summary>
    /// クリックまたは物理キー入力で実行するCommand。
    /// </summary>
    public ICommand? Command
    {
        get => _command;
        private set
        {
            if (SetProperty(ref _command, value))
            {
                OnPropertyChanged(nameof(IsAssigned));
            }
        }
    }

    /// <summary>
    /// 現在のページから処理が割り当てられているかを示す。
    /// </summary>
    public bool IsAssigned => Command is not null;

    /// <summary>
    /// ページから提供された表示名とCommandを設定する。
    /// </summary>
    /// <param name="binding">ページ固有のキー設定。</param>
    internal void Apply(FunctionKeyBinding binding)
    {
        Label = binding.Label;
        Command = binding.Command;
    }

    /// <summary>
    /// ページ遷移前の表示名とCommandを解除する。
    /// </summary>
    internal void Clear()
    {
        Label = string.Empty;
        Command = null;
    }
}
