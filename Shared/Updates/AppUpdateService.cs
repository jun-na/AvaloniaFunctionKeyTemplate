using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Velopack;
using Velopack.Sources;

namespace AvaloniaFunctionKeyTemplate.Shared.Updates;

/// <summary>
/// GitHub Releasesからアプリの更新を確認し、ダウンロード、適用、再起動を管理する。
/// </summary>
public sealed class AppUpdateService : ViewModelBase
{
    private readonly UpdateManager _updateManager;
    private UpdateInfo? _updateInfo;
    private bool _hasChecked;
    private bool _isUpdateAvailable;
    private bool _isUpdating;
    private string _availableVersion = string.Empty;
    private string _notificationMessage = string.Empty;

    /// <summary>
    /// GitHub Releasesを更新元とするUpdateManagerを生成する。
    /// </summary>
    /// <param name="repositoryUrl">更新パッケージを公開するGitHubリポジトリのURL。</param>
    public AppUpdateService(string repositoryUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryUrl);
        _updateManager = new UpdateManager(
            new GithubSource(repositoryUrl, null, false));
    }

    /// <summary>
    /// 更新通知を表示できる状態かどうかを示す。
    /// </summary>
    public bool IsUpdateAvailable
    {
        get => _isUpdateAvailable;
        private set => SetProperty(ref _isUpdateAvailable, value);
    }

    /// <summary>
    /// 更新データをダウンロードしているかどうかを示す。
    /// </summary>
    public bool IsUpdating
    {
        get => _isUpdating;
        private set => SetProperty(ref _isUpdating, value);
    }

    /// <summary>
    /// 更新先のバージョン文字列。
    /// </summary>
    public string AvailableVersion
    {
        get => _availableVersion;
        private set => SetProperty(ref _availableVersion, value);
    }

    /// <summary>
    /// Shell上部へ表示する更新案内または進捗メッセージ。
    /// </summary>
    public string NotificationMessage
    {
        get => _notificationMessage;
        private set => SetProperty(ref _notificationMessage, value);
    }

    /// <summary>
    /// Velopackから起動されたインストール済みアプリで、起動後に一度だけ更新を確認する。
    /// dotnet runやIDEからの実行時は自己更新を行わない。
    /// </summary>
    public async Task CheckForUpdatesAsync()
    {
        if (_hasChecked || !_updateManager.IsInstalled)
        {
            return;
        }

        _hasChecked = true;

        try
        {
            _updateInfo = await _updateManager.CheckForUpdatesAsync();
            if (_updateInfo is null)
            {
                return;
            }

            AvailableVersion = _updateInfo.TargetFullRelease.Version.ToString();
            NotificationMessage =
                $"新しいバージョン {AvailableVersion} を利用できます。クリックして更新";
            IsUpdateAvailable = true;
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"更新確認に失敗: {exception}");
        }
    }

    /// <summary>
    /// 確認済みの更新をダウンロードし、アプリを終了して更新適用後に再起動する。
    /// </summary>
    /// <returns>更新処理を開始できた場合はtrue。失敗した場合はfalse。</returns>
    public async Task<bool> UpdateAndRestartAsync()
    {
        if (_updateInfo is null || IsUpdating)
        {
            return false;
        }

        IsUpdating = true;
        NotificationMessage = "更新データをダウンロード中... 0%";

        try
        {
            await _updateManager.DownloadUpdatesAsync(
                _updateInfo,
                progress => Dispatcher.UIThread.Post(
                    () => NotificationMessage =
                        $"更新データをダウンロード中... {progress}%"),
                CancellationToken.None);

            NotificationMessage = "更新を適用して再起動中...";
            _updateManager.ApplyUpdatesAndRestart(_updateInfo.TargetFullRelease);
            return true;
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"更新適用に失敗: {exception}");
            NotificationMessage = "更新に失敗しました。クリックして再試行";
            IsUpdating = false;
            return false;
        }
    }
}
