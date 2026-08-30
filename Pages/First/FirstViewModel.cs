using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Input;
using AvaloniaFunctionKeyTemplate.Pages.First.Data;
using AvaloniaFunctionKeyTemplate.Shared;
using AvaloniaFunctionKeyTemplate.Shared.FunctionKeys;
using AvaloniaFunctionKeyTemplate.Shared.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaFunctionKeyTemplate.Pages.First;

/// <summary>
/// FirstViewの状態、ToDo操作、各ページへの遷移を管理する。
/// </summary>
public partial class FirstViewModel : ViewModelBase, IFunctionKeyProvider
{
    private readonly TodoItemDao? _todoItemDao;
    private readonly NavigationService? _navigationService;

    /// <summary>
    /// XAMLデザイナーがプレビュー用インスタンスを生成するためのコンストラクタ。
    /// 実行時にはDIコンテナが引数ありコンストラクタを使用する。
    /// </summary>
    public FirstViewModel()
    {
    }

    /// <summary>
    /// 実行時に必要なDAOと画面遷移サービスを受け取る。
    /// </summary>
    /// <param name="todoItemDao">ToDoデータへアクセスするDAO。</param>
    /// <param name="navigationService">表示ページを切り替えるサービス。</param>
    public FirstViewModel(
        TodoItemDao todoItemDao,
        NavigationService navigationService)
    {
        _todoItemDao = todoItemDao;
        _navigationService = navigationService;
    }

    /// <summary>
    /// 画面に表示するToDoの一覧。
    /// </summary>
    public ObservableCollection<TodoItemDto> Items { get; } = [];

    /// <summary>
    /// FirstViewで使用するF1のギャラリー遷移、F5の再読み込み、F12の追加処理を提供する。
    /// </summary>
    public IReadOnlyList<FunctionKeyBinding> FunctionKeys =>
    [
        new(Key.F1, "ギャラリー", NavigateToThirdCommand),
        new(Key.F5, "再読込", LoadItemsCommand),
        new(Key.F12, "追加", AddItemCommand),
    ];

    /// <summary>
    /// 新しく追加するToDoの入力値。
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    public partial string NewItemTitle { get; set; } = string.Empty;

    /// <summary>
    /// DB処理中かどうかを示す。
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    public partial bool IsBusy { get; set; }

    /// <summary>
    /// DB処理の結果を画面へ通知するメッセージ。
    /// </summary>
    [ObservableProperty]
    public partial string StatusMessage { get; set; } = string.Empty;

    /// <summary>
    /// 入力値と実行状態からToDoを追加できるか判定する。
    /// </summary>
    private bool CanAddItem() =>
        !IsBusy && !string.IsNullOrWhiteSpace(NewItemTitle) && _todoItemDao is not null;

    /// <summary>
    /// 実行時のNavigationServiceが設定されているか判定する。
    /// </summary>
    private bool CanNavigateToSecond() => _navigationService is not null;

    /// <summary>
    /// 表示ページをSecondViewへ切り替える。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateToSecond))]
    private void NavigateToSecond() => _navigationService?.NavigateTo(PageId.Second);

    /// <summary>
    /// 表示ページをコントロールギャラリーのThirdViewへ切り替える。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateToSecond))]
    private void NavigateToThird() => _navigationService?.NavigateTo(PageId.Third);

    /// <summary>
    /// テーブルを初期化し、SQLiteからToDo一覧を再取得する。
    /// </summary>
    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        if (_todoItemDao is null)
        {
            return;
        }

        IsBusy = true;

        try
        {
            await _todoItemDao.InitializeAsync();
            var items = await _todoItemDao.GetAllAsync();

            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }

            StatusMessage = $"{Items.Count}件読み込み";
        }
        catch (Exception exception)
        {
            StatusMessage = $"読み込み失敗: {exception.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 入力されたToDoをSQLiteへ追加し、表示一覧を更新する。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private async Task AddItemAsync()
    {
        if (_todoItemDao is null)
        {
            return;
        }

        var title = NewItemTitle.Trim();
        IsBusy = true;

        try
        {
            await _todoItemDao.InitializeAsync();
            await _todoItemDao.InsertAsync(title);
            NewItemTitle = string.Empty;

            var items = await _todoItemDao.GetAllAsync();
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }

            StatusMessage = "追加完了";
        }
        catch (Exception exception)
        {
            StatusMessage = $"追加失敗: {exception.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
