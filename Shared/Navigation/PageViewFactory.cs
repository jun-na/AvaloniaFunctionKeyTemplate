using System;
using Avalonia.Controls;
using AvaloniaFunctionKeyTemplate.Pages.First;
using AvaloniaFunctionKeyTemplate.Pages.Second;
using AvaloniaFunctionKeyTemplate.Pages.Third;

namespace AvaloniaFunctionKeyTemplate.Shared.Navigation;

/// <summary>
/// ページ識別子に対応する引数なしViewを明示的に生成する。
/// </summary>
public sealed class PageViewFactory
{
    private readonly Func<FirstView> _firstViewFactory;
    private readonly Func<SecondView> _secondViewFactory;
    private readonly Func<ThirdView> _thirdViewFactory;

    /// <summary>
    /// Pure.DIが生成した各ViewのTransientファクトリを受け取る。
    /// </summary>
    /// <param name="firstViewFactory">FirstViewを生成するファクトリ。</param>
    /// <param name="secondViewFactory">SecondViewを生成するファクトリ。</param>
    /// <param name="thirdViewFactory">ThirdViewを生成するファクトリ。</param>
    public PageViewFactory(
        Func<FirstView> firstViewFactory,
        Func<SecondView> secondViewFactory,
        Func<ThirdView> thirdViewFactory)
    {
        _firstViewFactory = firstViewFactory;
        _secondViewFactory = secondViewFactory;
        _thirdViewFactory = thirdViewFactory;
    }

    /// <summary>
    /// 指定されたページのViewを新しく生成する。
    /// </summary>
    /// <param name="pageId">生成するページの識別子。</param>
    /// <returns>ページに対応するUserControl。</returns>
    public Control Create(PageId pageId) => pageId switch
    {
        PageId.First => _firstViewFactory(),
        PageId.Second => _secondViewFactory(),
        PageId.Third => _thirdViewFactory(),
        _ => throw new ArgumentOutOfRangeException(nameof(pageId), pageId, null),
    };
}
