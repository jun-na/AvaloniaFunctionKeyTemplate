using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace AvaloniaFunctionKeyTemplate.Shared.Behaviors;

/// <summary>
/// 単一行TextBoxでEnterキーを押したとき、Tab順の次の要素へフォーカスを移す添付ビヘイビア。
/// </summary>
public sealed class EnterMovesFocusBehavior : AvaloniaObject
{
    /// <summary>
    /// Enterキーによるフォーカス移動を有効にする添付プロパティ。
    /// </summary>
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<EnterMovesFocusBehavior, TextBox, bool>("IsEnabled");

    /// <summary>
    /// 添付プロパティの変更を監視するクラスハンドラーを登録する。
    /// </summary>
    static EnterMovesFocusBehavior()
    {
        IsEnabledProperty.Changed.AddClassHandler<TextBox>(OnIsEnabledChanged);
    }

    /// <summary>
    /// XAMLから指定されたEnterキー移動の有効状態を取得する。
    /// </summary>
    /// <param name="textBox">設定対象のTextBox。</param>
    /// <returns>Enterキー移動が有効な場合はtrue。</returns>
    public static bool GetIsEnabled(TextBox textBox)
    {
        return textBox.GetValue(IsEnabledProperty);
    }

    /// <summary>
    /// XAMLからEnterキー移動の有効状態を設定する。
    /// </summary>
    /// <param name="textBox">設定対象のTextBox。</param>
    /// <param name="value">Enterキー移動を有効にする場合はtrue。</param>
    public static void SetIsEnabled(TextBox textBox, bool value)
    {
        textBox.SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// 添付プロパティの値に合わせてKeyDownイベントを登録または解除する。
    /// </summary>
    /// <param name="textBox">設定が変更されたTextBox。</param>
    /// <param name="e">添付プロパティの変更内容。</param>
    private static void OnIsEnabledChanged(TextBox textBox, AvaloniaPropertyChangedEventArgs e)
    {
        textBox.KeyDown -= OnKeyDown;

        if (e.NewValue is true)
        {
            textBox.KeyDown += OnKeyDown;
        }
    }

    /// <summary>
    /// 修飾キーなしのEnterキーを受け取り、Tab順の次の要素へフォーカスを移す。
    /// 読み取り専用または複数行のTextBoxではEnterキー本来の動作を優先する。
    /// </summary>
    /// <param name="sender">キー入力を受け取ったTextBox。</param>
    /// <param name="e">キー入力の情報。</param>
    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox ||
            e.Handled ||
            e.Key != Key.Enter ||
            e.KeyModifiers != KeyModifiers.None ||
            textBox.IsReadOnly ||
            textBox.AcceptsReturn)
        {
            return;
        }

        var focusManager = TopLevel.GetTopLevel(textBox)?.FocusManager;
        var moved = focusManager?.TryMoveFocus(
            NavigationDirection.Next,
            new FindNextElementOptions
            {
                FocusedElement = textBox
            });

        if (moved is true)
        {
            e.Handled = true;
        }
    }
}
