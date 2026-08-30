using System.ComponentModel;
using System.Threading.Tasks;
using AvaloniaFunctionKeyTemplate.Shared;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;
using AvaloniaFunctionKeyTemplate.Shared.Updates;

namespace AvaloniaFunctionKeyTemplate.Shell;

/// <summary>
/// Shellが表示する現在ページとアプリ更新通知の状態をまとめる。
/// </summary>
public sealed class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// 画面遷移と更新状態を受け取り、Shell表示の変更通知を連携する。
    /// </summary>
    /// <param name="navigationService">現在ページと表示Viewを管理するサービス。</param>
    /// <param name="appUpdateService">アプリ更新を管理するサービス。</param>
    public MainWindowViewModel(
        NavigationService navigationService,
        AppUpdateService appUpdateService)
    {
        Navigation = navigationService;
        Updates = appUpdateService;

        Navigation.PropertyChanged += OnNavigationPropertyChanged;
        Updates.PropertyChanged += OnUpdatePropertyChanged;
    }

    /// <summary>
    /// Shell中央へ表示するページを管理するサービス。
    /// </summary>
    public NavigationService Navigation { get; }

    /// <summary>
    /// 更新確認とShell上部の通知状態を管理するサービス。
    /// </summary>
    public AppUpdateService Updates { get; }

    /// <summary>
    /// 更新が見つかり、現在の画面がFirstViewの場合だけ通知を表示する。
    /// </summary>
    public bool IsUpdateNotificationVisible =>
        Navigation.CurrentPage is PageId.First && Updates.IsUpdateAvailable;

    /// <summary>
    /// ダウンロード中の通知が重複して押されないよう操作可否を返す。
    /// </summary>
    public bool IsUpdateNotificationEnabled => !Updates.IsUpdating;

    /// <summary>
    /// アプリ起動後の更新確認を開始する。
    /// </summary>
    public Task CheckForUpdatesAsync() => Updates.CheckForUpdatesAsync();

    /// <summary>
    /// 更新データを取得し、適用後の再起動を開始する。
    /// </summary>
    /// <returns>更新処理を開始できた場合はtrue。</returns>
    public Task<bool> UpdateAndRestartAsync() => Updates.UpdateAndRestartAsync();

    /// <summary>
    /// 現在ページが変わったとき、FirstView限定の通知表示を再評価する。
    /// </summary>
    /// <param name="sender">変更通知を発生させたNavigationService。</param>
    /// <param name="e">変更されたプロパティの情報。</param>
    private void OnNavigationPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(NavigationService.CurrentPage))
        {
            OnPropertyChanged(nameof(IsUpdateNotificationVisible));
        }
    }

    /// <summary>
    /// 更新状態が変わったとき、Shellの通知表示と操作可否を更新する。
    /// </summary>
    /// <param name="sender">変更通知を発生させたAppUpdateService。</param>
    /// <param name="e">変更されたプロパティの情報。</param>
    private void OnUpdatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(AppUpdateService.IsUpdateAvailable))
        {
            OnPropertyChanged(nameof(IsUpdateNotificationVisible));
        }

        if (e.PropertyName is nameof(AppUpdateService.IsUpdating))
        {
            OnPropertyChanged(nameof(IsUpdateNotificationEnabled));
        }
    }
}
