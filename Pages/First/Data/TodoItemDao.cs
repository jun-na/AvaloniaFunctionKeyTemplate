using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace AvaloniaFunctionKeyTemplate.Pages.First.Data;

/// <summary>
/// Dapperを使ってSQLiteのToDoデータを読み書きする。
/// </summary>
/// <param name="connectionString">SQLiteへ接続するための接続文字列。</param>
public sealed class TodoItemDao(string connectionString)
{
    /// <summary>
    /// ToDoテーブルが存在しない場合に作成する。
    /// </summary>
    /// <param name="cancellationToken">処理のキャンセルを通知するトークン。</param>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS todo_items (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                title TEXT NOT NULL
            );
            """;

        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    /// <summary>
    /// 登録済みのToDoを新しい順に取得する。
    /// </summary>
    /// <param name="cancellationToken">処理のキャンセルを通知するトークン。</param>
    /// <returns>取得したToDoの一覧。</returns>
    public async Task<IReadOnlyList<TodoItemDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                id AS Id,
                title AS Title
            FROM todo_items
            ORDER BY id DESC;
            """;

        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);
        var items = await connection.QueryAsync<TodoItemDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return items.AsList();
    }

    /// <summary>
    /// ToDoを1件追加する。
    /// </summary>
    /// <param name="title">追加するToDoのタイトル。</param>
    /// <param name="cancellationToken">処理のキャンセルを通知するトークン。</param>
    /// <returns>SQLiteが採番したID。</returns>
    public async Task<long> InsertAsync(
        string title,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO todo_items (title)
            VALUES (@Title)
            RETURNING id;
            """;

        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<long>(
            new CommandDefinition(
                sql,
                new { Title = title },
                cancellationToken: cancellationToken));
    }

    /// <summary>
    /// 各DB操作で使用する新しい接続を生成する。
    /// </summary>
    /// <returns>未オープンのSQLite接続。</returns>
    private SqliteConnection CreateConnection() => new(connectionString);
}
