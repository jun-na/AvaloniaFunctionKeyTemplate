namespace AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;

/// <summary>
/// DIコンテナがサービスインスタンスを保持する期間。
/// </summary>
public enum ServiceLifetime
{
    /// <summary>
    /// Build時に1回だけ生成し、以後同じインスタンスを返す。
    /// </summary>
    Singleton,

    /// <summary>
    /// Resolveのたびに新しいインスタンスを返す。
    /// </summary>
    Transient,
}
