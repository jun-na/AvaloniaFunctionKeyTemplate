using System;

namespace AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;

/// <summary>
/// Avaloniaが引数なしで生成するViewから、Pure.DIの生成済みCompositionへアクセスする。
/// </summary>
public static class AppServices
{
    private static AppComposition? _composition;

    /// <summary>
    /// SQLite接続文字列を渡してPure.DIのCompositionを起動時に1度だけ初期化する。
    /// </summary>
    /// <param name="connectionString">TodoItemDaoへ注入するSQLite接続文字列。</param>
    public static void Initialize(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        if (_composition is not null)
        {
            throw new InvalidOperationException("AppServicesは既に初期化されています。");
        }

        _composition = new AppComposition(connectionString);
    }

    /// <summary>
    /// Pure.DIがコンパイル時生成した解決処理から、指定されたルート型を取得する。
    /// </summary>
    /// <typeparam name="TService">取得するサービスまたはViewModelの型。</typeparam>
    /// <returns>Pure.DIで生成されたインスタンス。</returns>
    public static TService Resolve<TService>()
        where TService : class
    {
        var composition = _composition ?? throw new InvalidOperationException(
            "AppServices.Initialize()の実行後にResolveしてください。");

        return composition.Resolve<TService>();
    }
}
