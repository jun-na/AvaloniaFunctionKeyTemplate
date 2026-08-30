using System;
using System.Collections.Generic;

namespace AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;

/// <summary>
/// 明示的なファクトリ登録だけで依存関係を解決する軽量DIコンテナ。
/// コンストラクタ探索やActivatorによるリフレクションを使用しない。
/// </summary>
public static class ServiceContainer
{
    private static readonly object _syncRoot = new();
    private static readonly Dictionary<Type, ServiceRegistration> _registrations = [];
    private static bool _isBuilt;

    /// <summary>
    /// アプリケーション内で1つだけ生成するサービスを登録する。
    /// SingletonはBuild時に生成される。
    /// </summary>
    /// <typeparam name="TService">登録するサービスの型。</typeparam>
    /// <param name="factory">サービスを明示的に生成するファクトリ。</param>
    public static void AddSingleton<TService>(Func<TService> factory)
        where TService : class
    {
        Add(factory, ServiceLifetime.Singleton);
    }

    /// <summary>
    /// Resolveのたびに新しく生成するサービスを登録する。
    /// </summary>
    /// <typeparam name="TService">登録するサービスの型。</typeparam>
    /// <param name="factory">サービスを明示的に生成するファクトリ。</param>
    public static void AddTransient<TService>(Func<TService> factory)
        where TService : class
    {
        Add(factory, ServiceLifetime.Transient);
    }

    /// <summary>
    /// 登録内容を確定し、すべてのSingletonを起動時に生成する。
    /// Build後のサービス追加は禁止される。
    /// </summary>
    public static void Build()
    {
        List<ServiceRegistration> singletons = [];

        lock (_syncRoot)
        {
            if (_isBuilt)
            {
                throw new InvalidOperationException("ServiceContainerは既にBuild済みです。");
            }

            _isBuilt = true;

            foreach (var registration in _registrations.Values)
            {
                if (registration.Lifetime is ServiceLifetime.Singleton)
                {
                    singletons.Add(registration);
                }
            }
        }

        foreach (var singleton in singletons)
        {
            singleton.Resolve();
        }
    }

    /// <summary>
    /// 登録されたライフタイムに従ってサービスを取得する。
    /// </summary>
    /// <typeparam name="TService">取得するサービスの型。</typeparam>
    /// <returns>Singletonインスタンス、または新しいTransientインスタンス。</returns>
    public static TService Resolve<TService>()
        where TService : class
    {
        ServiceRegistration registration;

        lock (_syncRoot)
        {
            if (!_isBuilt)
            {
                throw new InvalidOperationException(
                    "ServiceContainer.Build()の実行後にResolveしてください。");
            }

            if (!_registrations.TryGetValue(typeof(TService), out registration!))
            {
                throw new InvalidOperationException(
                    $"{typeof(TService).FullName}は登録されていません。");
            }
        }

        return (TService)registration.Resolve();
    }

    /// <summary>
    /// サービス型、ファクトリ、ライフタイムを内部登録表へ追加する。
    /// </summary>
    /// <typeparam name="TService">登録するサービスの型。</typeparam>
    /// <param name="factory">サービスを生成するファクトリ。</param>
    /// <param name="lifetime">サービスのライフタイム。</param>
    private static void Add<TService>(
        Func<TService> factory,
        ServiceLifetime lifetime)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(factory);

        lock (_syncRoot)
        {
            if (_isBuilt)
            {
                throw new InvalidOperationException(
                    "ServiceContainer.Build()後にサービスを追加できません。");
            }

            var serviceType = typeof(TService);
            if (_registrations.ContainsKey(serviceType))
            {
                throw new InvalidOperationException(
                    $"{serviceType.FullName}は既に登録されています。");
            }

            _registrations.Add(
                serviceType,
                new ServiceRegistration(
                    () => factory() ?? throw new InvalidOperationException(
                        $"{serviceType.FullName}のファクトリがnullを返しました。"),
                    lifetime));
        }
    }

    /// <summary>
    /// 1サービス分のファクトリ、ライフタイム、Singleton状態を保持する。
    /// </summary>
    /// <param name="factory">サービスを生成するファクトリ。</param>
    /// <param name="lifetime">サービスのライフタイム。</param>
    private sealed class ServiceRegistration(
        Func<object> factory,
        ServiceLifetime lifetime)
    {
        private readonly object _syncRoot = new();
        private object? _instance;
        private bool _isCreating;

        /// <summary>
        /// 登録されたサービスのライフタイム。
        /// </summary>
        public ServiceLifetime Lifetime { get; } = lifetime;

        /// <summary>
        /// Transientを新規生成するか、キャッシュ済みSingletonを返す。
        /// </summary>
        /// <returns>解決されたサービスインスタンス。</returns>
        public object Resolve()
        {
            if (Lifetime is ServiceLifetime.Transient)
            {
                return factory();
            }

            lock (_syncRoot)
            {
                if (_instance is not null)
                {
                    return _instance;
                }

                if (_isCreating)
                {
                    throw new InvalidOperationException("Singletonの循環依存を検出しました。");
                }

                _isCreating = true;

                try
                {
                    _instance = factory();
                    return _instance;
                }
                finally
                {
                    _isCreating = false;
                }
            }
        }
    }
}
