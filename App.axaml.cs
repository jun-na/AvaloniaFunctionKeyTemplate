using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaFunctionKeyTemplate.Pages.First;
using AvaloniaFunctionKeyTemplate.Pages.First.Data;
using AvaloniaFunctionKeyTemplate.Pages.Second;
using AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;
using AvaloniaFunctionKeyTemplate.Shell;
using Microsoft.Data.Sqlite;

namespace AvaloniaFunctionKeyTemplate;

/// <summary>
/// Avaloniaアプリケーションを初期化し、起動時の依存関係を構成する。
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// アプリケーション共通のXAMLリソースを読み込む。
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// デスクトップ用サービスを登録し、初期ページとメインウィンドウを生成する。
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var databaseDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                nameof(AvaloniaFunctionKeyTemplate));
            Directory.CreateDirectory(databaseDirectory);

            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(databaseDirectory, "app.db"),
            }.ToString();

            ConfigureServices(connectionString);

            var navigationService = ServiceContainer.Resolve<NavigationService>();
            navigationService.NavigateTo(PageId.First);
            desktop.MainWindow = ServiceContainer.Resolve<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// サービスの生成方法とライフタイムを登録し、コンテナを確定する。
    /// </summary>
    /// <param name="connectionString">SQLiteへ接続するための接続文字列。</param>
    private static void ConfigureServices(string connectionString)
    {
        ServiceContainer.AddSingleton(() => new TodoItemDao(connectionString));
        ServiceContainer.AddSingleton(() => new NavigationService(CreateView));

        ServiceContainer.AddTransient(() => new FirstViewModel(
            ServiceContainer.Resolve<TodoItemDao>(),
            ServiceContainer.Resolve<NavigationService>()));
        ServiceContainer.AddTransient(() => new SecondViewModel(
            ServiceContainer.Resolve<NavigationService>()));

        ServiceContainer.AddTransient(() => new FirstView());
        ServiceContainer.AddTransient(() => new SecondView());
        ServiceContainer.AddSingleton(() => new MainWindow());

        ServiceContainer.Build();
    }

    /// <summary>
    /// ページ識別子に対応するViewをコンテナから取得する。
    /// Viewの対応関係を明示することでリフレクションを使わない。
    /// </summary>
    /// <param name="pageId">表示対象のページ識別子。</param>
    /// <returns>新しく生成されたページのView。</returns>
    private static Control CreateView(PageId pageId) => pageId switch
    {
        PageId.First => ServiceContainer.Resolve<FirstView>(),
        PageId.Second => ServiceContainer.Resolve<SecondView>(),
        _ => throw new ArgumentOutOfRangeException(nameof(pageId), pageId, null),
    };
}
