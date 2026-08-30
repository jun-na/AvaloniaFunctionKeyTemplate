namespace AvaloniaFunctionKeyTemplate.Pages.First.Data;

/// <summary>
/// todo_itemsテーブルの検索結果を受け取るデータ転送オブジェクト。
/// </summary>
public sealed class TodoItemDto
{
    /// <summary>
    /// ToDoの一意なID。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ToDoの表示名。
    /// </summary>
    public string Title { get; set; } = string.Empty;
}
