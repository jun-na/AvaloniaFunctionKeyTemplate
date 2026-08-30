using System;
using System.Collections.Generic;
using Avalonia.Input;

namespace AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;

/// <summary>
/// F1からF12までの表示状態とCommand実行を一元管理する。
/// </summary>
public sealed class FunctionKeyService
{
    private readonly Dictionary<Key, FunctionKeySlot> _slots;

    /// <summary>
    /// 12個の固定スロットを生成する。
    /// </summary>
    public FunctionKeyService()
    {
        _slots = new Dictionary<Key, FunctionKeySlot>
        {
            [Key.F1] = F1,
            [Key.F2] = F2,
            [Key.F3] = F3,
            [Key.F4] = F4,
            [Key.F5] = F5,
            [Key.F6] = F6,
            [Key.F7] = F7,
            [Key.F8] = F8,
            [Key.F9] = F9,
            [Key.F10] = F10,
            [Key.F11] = F11,
            [Key.F12] = F12,
        };
    }

    /// <summary>F1の表示状態。</summary>
    public FunctionKeySlot F1 { get; } = new(Key.F1);

    /// <summary>F2の表示状態。</summary>
    public FunctionKeySlot F2 { get; } = new(Key.F2);

    /// <summary>F3の表示状態。</summary>
    public FunctionKeySlot F3 { get; } = new(Key.F3);

    /// <summary>F4の表示状態。</summary>
    public FunctionKeySlot F4 { get; } = new(Key.F4);

    /// <summary>F5の表示状態。</summary>
    public FunctionKeySlot F5 { get; } = new(Key.F5);

    /// <summary>F6の表示状態。</summary>
    public FunctionKeySlot F6 { get; } = new(Key.F6);

    /// <summary>F7の表示状態。</summary>
    public FunctionKeySlot F7 { get; } = new(Key.F7);

    /// <summary>F8の表示状態。</summary>
    public FunctionKeySlot F8 { get; } = new(Key.F8);

    /// <summary>F9の表示状態。</summary>
    public FunctionKeySlot F9 { get; } = new(Key.F9);

    /// <summary>F10の表示状態。</summary>
    public FunctionKeySlot F10 { get; } = new(Key.F10);

    /// <summary>F11の表示状態。</summary>
    public FunctionKeySlot F11 { get; } = new(Key.F11);

    /// <summary>F12の表示状態。</summary>
    public FunctionKeySlot F12 { get; } = new(Key.F12);

    /// <summary>
    /// 表示ページの設定で全スロットを置き換える。
    /// </summary>
    /// <param name="provider">遷移先ページのファンクションキー提供元。</param>
    public void Activate(IFunctionKeyProvider? provider)
    {
        var bindings = provider?.FunctionKeys ?? [];
        HashSet<Key> assignedKeys = [];

        foreach (var binding in bindings)
        {
            if (!_slots.ContainsKey(binding.Key))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(provider),
                    binding.Key,
                    "F1からF12までのキーだけを登録できます。");
            }

            if (!assignedKeys.Add(binding.Key))
            {
                throw new InvalidOperationException(
                    $"{binding.Key}が重複して登録されています。");
            }
        }

        foreach (var slot in _slots.Values)
        {
            slot.Clear();
        }

        foreach (var binding in bindings)
        {
            _slots[binding.Key].Apply(binding);
        }
    }

    /// <summary>
    /// 指定キーに割り当てられたCommandを実行する。
    /// </summary>
    /// <param name="key">入力された物理キー。</param>
    /// <returns>割り当て済みCommandを実行した場合はtrue。</returns>
    public bool Execute(Key key)
    {
        if (!_slots.TryGetValue(key, out var slot) ||
            slot.Command is null ||
            !slot.Command.CanExecute(null))
        {
            return false;
        }

        slot.Command.Execute(null);
        return true;
    }
}
