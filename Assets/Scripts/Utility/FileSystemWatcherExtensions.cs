using UniRx;
using UniRx.Triggers;
using System.IO;

// .NETのFromEventなら IObservable<TEventArgs>
// .NETのFromEventPatternなら IObservable<EventPattern<TEventArgs>>
// WP7のFromEventなら IObservable<IEvent<TEventArgs>>
// を返す拡張メソッド群を用意する。
// 命名規則はイベント名AsObservableがIntelliSenseの順序的にお薦め
public static class FileSystemWatcherExtensions
{
    public static IObservable<FileSystemEventArgs> CreatedAsObservable(this FileSystemWatcher watcher)
    {
        return Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
            h => (sender, e) => h(e), h => watcher.Created += h, h => watcher.Created -= h);
    }

    public static IObservable<FileSystemEventArgs> DeletedAsObservable(this FileSystemWatcher watcher)
    {
        return Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
            h => (sender, e) => h(e), h => watcher.Deleted += h, h => watcher.Deleted -= h);
    }

    public static IObservable<RenamedEventArgs> RenamedAsObservable(this FileSystemWatcher watcher)
    {
        return Observable.FromEvent<RenamedEventHandler, RenamedEventArgs>(
            h => (sender, e) => h(e), h => watcher.Renamed += h, h => watcher.Renamed -= h);
    }

    public static IObservable<FileSystemEventArgs> ChangedAsObservable(this FileSystemWatcher watcher)
    {
        return Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
            h => (sender, e) => h(e), h => watcher.Changed += h, h => watcher.Changed -= h);
    }
}