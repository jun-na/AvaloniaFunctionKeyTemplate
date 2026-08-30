using AvaloniaFunctionKeyTemplate.Pages.First;
using AvaloniaFunctionKeyTemplate.Pages.First.Data;
using AvaloniaFunctionKeyTemplate.Pages.Second;
using AvaloniaFunctionKeyTemplate.Pages.Third;
using AvaloniaFunctionKeyTemplate.Shared.Dialogs;
using AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;
using AvaloniaFunctionKeyTemplate.Shared.Updates;
using AvaloniaFunctionKeyTemplate.Shell;
using Pure.DI;

namespace AvaloniaFunctionKeyTemplate.Shared.DependencyInjection;

/// <summary>
/// Pure.DIがコンパイル時に生成するアプリケーション全体の依存オブジェクト構成。
/// </summary>
internal partial class AppComposition
{
    /// <summary>
    /// Velopack形式の更新パッケージを公開するGitHubリポジトリ。
    /// </summary>
    private const string UpdateRepositoryUrl =
        "https://github.com/jun-na/AvaloniaFunctionKeyTemplate";

    /// <summary>
    /// Singleton、Transient、実行時引数、解決可能なルートをPure.DIへ宣言する。
    /// このメソッドは実行されず、ソースジェネレーターが内容を解析する。
    /// </summary>
    private static void Setup() =>
        DI.Setup(nameof(AppComposition))
            .Arg<string>("connectionString")

            .Bind<TodoItemDao>()
                .As(Lifetime.Singleton)
                .To(context =>
                {
                    context.Inject(out string connectionString);
                    return new TodoItemDao(connectionString);
                })
            .Bind<MessageDialogService>()
                .As(Lifetime.Singleton)
                .To<MessageDialogService>()
            .Bind<FunctionKeyService>()
                .As(Lifetime.Singleton)
                .To<FunctionKeyService>()
            .Bind<AppUpdateService>()
                .As(Lifetime.Singleton)
                .To(_ => new AppUpdateService(UpdateRepositoryUrl))
            .Bind<PageViewFactory>()
                .As(Lifetime.Singleton)
                .To<PageViewFactory>()
            .Bind<NavigationService>()
                .As(Lifetime.Singleton)
                .To<NavigationService>()
            .Bind<MainWindowViewModel>()
                .As(Lifetime.Singleton)
                .To<MainWindowViewModel>()

            .Bind<FirstView>().To<FirstView>()
            .Bind<SecondView>().To<SecondView>()
            .Bind<ThirdView>().To<ThirdView>()
            .Bind<FirstViewModel>().To<FirstViewModel>()
            .Bind<SecondViewModel>().To<SecondViewModel>()
            .Bind<ThirdViewModel>().To<ThirdViewModel>()

            .Root<NavigationService>()
            .Root<FunctionKeyService>()
            .Root<MessageDialogService>()
            .Root<MainWindowViewModel>()
            .Root<FirstViewModel>()
            .Root<SecondViewModel>()
            .Root<ThirdViewModel>();
}
