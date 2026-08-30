# AvaloniaFunctionKeyTemplate

Avaloniaで業務用デスクトップアプリを作るためのテンプレート。

画面単位でView、ViewModel、画面固有処理をまとめる構成、ファンクションキーバー、画面遷移、Pure.DI、DapperとSQLiteによるデータアクセスを含む。UIはWindows XP風。

## 主な機能

- Avalonia 12 / .NET 10
- `Pages`単位のVertical Slice Architecture
- MainWindowをShellとした画面遷移
- 画面ごとに表示と処理を切り替えるF1〜F12ファンクションキーバー
- SingletonとTransientに対応したコンパイル時コード生成DI（Pure.DI）
- DapperとSQLiteを使ったDAO / DTOサンプル
- Windows XP風の独自コントロールテーマ
- 単一行TextBoxでEnterを押したときのTab移動
- Confirm、Info、Warning、Errorに対応した共通メッセージダイアログ
- GitHub Releasesを使った起動時の自動更新確認
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
  DependencyInjection/      Pure.DI CompositionとAvalonia向けResolver
  Dialogs/                  共通メッセージダイアログ
  FunctionKeys/             ファンクションキー基盤
  Navigation/               画面遷移基盤
  Updates/                  自動更新基盤
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
7. `AppComposition.cs`の`UpdateRepositoryUrl`
8. `.github/workflows/release.yml`の`packId`、`mainExe`、表示名

コピー元のGit履歴が不要な場合は`.git`を引き継がない。`bin`と`obj`もコピー対象から外し、名称変更後に`dotnet build`を実行する。

## 画面の追加

1. `Pages`配下に画面用フォルダを作成する
2. ViewとViewModelを配置する
3. `PageId`へ画面識別子を追加する
4. `AppComposition.cs`でViewとViewModelをPure.DIへ登録する
5. `PageViewFactory.cs`へ画面生成の対応を追加する
6. ViewModelで`IFunctionKeyProvider`を実装し、画面固有のファンクションキーを定義する

## 自動更新とリリース

Velopack経由でインストールされたWindows x64版は、起動後にGitHub Releasesの更新を一度確認する。更新がある場合、FirstView表示中だけShell上部へ案内を表示する。案内をクリックして確認ダイアログで「はい」を選ぶと、更新データを取得してアプリを再起動する。

`dotnet run`やIDEから起動したアプリでは更新確認を行わない。

`v`で始まるバージョンタグをpushすると、GitHub ActionsがWindows用インストーラーと更新パッケージをGitHub Releasesへ公開する。

```sh
git tag v1.0.0
git push origin v1.0.0
```

2回目以降はSemVer形式でバージョンを上げる。

```sh
git tag v1.0.1
git push origin v1.0.1
```

## ライセンス

[MIT License](LICENSE)
