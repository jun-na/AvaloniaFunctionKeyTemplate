# AvaloniaFunctionKeyTemplate

Avaloniaで業務用デスクトップアプリを作るためのテンプレート。

画面単位でView、ViewModel、画面固有処理をまとめる構成、ファンクションキーバー、画面遷移、自作DIコンテナ、DapperとSQLiteによるデータアクセスを含む。UIはWindows XP風。

## 主な機能

- Avalonia 12 / .NET 10
- `Pages`単位のVertical Slice Architecture
- MainWindowをShellとした画面遷移
- 画面ごとに表示と処理を切り替えるF1〜F12ファンクションキーバー
- SingletonとTransientに対応したリフレクション不使用の自作DIコンテナ
- DapperとSQLiteを使ったDAO / DTOサンプル
- Windows XP風の独自コントロールテーマ
- 単一行TextBoxでEnterを押したときのTab移動
- コントロールギャラリー

## 必要環境

- .NET 10 SDK

## 起動

```sh
dotnet restore
dotnet run
```

SQLiteデータベースは、ユーザーのローカルアプリケーションデータ領域に`AvaloniaFunctionKeyTemplate/app.db`として作成される。

## フォルダ構成

```text
Pages/                      画面単位のView、ViewModel、固有処理
  First/                    SQLite ToDoサンプル
    Data/                   DAO、DTO
  Second/                   画面遷移とファンクションキーのサンプル
  Third/                    コントロールギャラリー
Shared/                     複数画面で共有する基盤
  Behaviors/                UIの共通Behavior
  DependencyInjection/      自作DIコンテナ
  FunctionKeys/             ファンクションキー基盤
  Navigation/               画面遷移基盤
Shell/                      MainWindowとファンクションキーバー
Themes/                     Windows XP風テーマ
```

## テンプレートとして使う

このリポジトリをコピーし、次の項目を新しいアプリ名へ変更する。

1. `AvaloniaFunctionKeyTemplate.csproj`のファイル名
2. C#の`namespace`と`using`
3. axamlの`x:Class`と`xmlns`
4. MainWindowの`Title`
5. `app.manifest`の`assemblyIdentity`
6. `App.axaml.cs`のSQLite保存先フォルダ名

コピー元のGit履歴が不要な場合は`.git`を引き継がない。`bin`と`obj`もコピー対象から外し、名称変更後に`dotnet build`を実行する。

## 画面の追加

1. `Pages`配下に画面用フォルダを作成する
2. ViewとViewModelを配置する
3. `PageId`へ画面識別子を追加する
4. `App.axaml.cs`でViewとViewModelをDIコンテナへ登録する
5. `App.axaml.cs`の`CreateView`へ対応を追加する
6. ViewModelで`IFunctionKeyProvider`を実装し、画面固有のファンクションキーを定義する

## ライセンス

[MIT License](LICENSE)
