using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
    /// Pure.DIを初期化し、初期ページとメインウィンドウを生成する。
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

            AppServices.Initialize(connectionString);

            var navigationService = AppServices.Resolve<NavigationService>();
            navigationService.NavigateTo(PageId.First);
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
