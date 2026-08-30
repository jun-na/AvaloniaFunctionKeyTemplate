using Avalonia;
using Avalonia.Input;
using AvaloniaUI.DiagnosticsSupport;
using System;

namespace AvaloniaFunctionKeyTemplate;

/// <summary>
/// デスクトップアプリケーションのエントリーポイント。
/// </summary>
sealed class Program
{
    /// <summary>
    /// Avaloniaを初期化し、クラシックデスクトップライフタイムで起動する。
    /// </summary>
    /// <param name="args">コマンドライン引数。</param>
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    /// <summary>
    /// 実行時とXAMLデザイナーが共用するAvalonia設定を構築する。
    /// </summary>
    /// <returns>プラットフォーム設定済みのAppBuilder。</returns>
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools(options =>
                options.Gesture = new KeyGesture(
                    Key.F12,
                    KeyModifiers.Control | KeyModifiers.Shift))
#endif
            .WithInterFont()
            .LogToTrace();
}
