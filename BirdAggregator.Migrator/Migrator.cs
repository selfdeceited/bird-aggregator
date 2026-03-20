using System.Collections.Concurrent;
using static BirdAggregator.Migrator.Program;
using System.Reactive.Linq;
using BirdAggregator.Migrator.Providers;
using Colorify;

namespace BirdAggregator.Migrator;

public class Migrator(IMigratorObservableProvider migratorObservableProvider) : IMigrator
{
    private readonly ConcurrentDictionary<string, bool> _savedEntities = new();

    public void Run()
    {
        var cts = new CancellationTokenSource();

        ColoredConsole.WriteLine("Migrator started. Press any key to stop it.", Colors.txtInfo);
        migratorObservableProvider.EnsureCollectionsExist()
            .SelectMany(_ => migratorObservableProvider.GetPages())
            .SelectMany(migratorObservableProvider.GetPhotoId)
            .Where(x => migratorObservableProvider.ShouldUpdateDb(x).Wait())
            .Do(LogEntitiesToAdd)
            .SelectMany(migratorObservableProvider.GetPhotoInfoForSave)
            .DistinctUntilChanged()
            .Buffer(TimeSpan.FromMilliseconds(1000))
            .SelectMany(x => migratorObservableProvider
                .SavePhotos(x)
                .Retry(3)
            )
            .Catch<SavePhotoResult[], Exception>(_ =>
            {
                ColoredConsole.WriteLine($"exception occured: {_.Message} {_.StackTrace}", Colors.bgDanger);
                return Observable.Empty<SavePhotoResult[]>();
            })
            .Do(LogDataSaved)
            .DistinctUntilChanged()
            .Subscribe();

        Observable
            .Interval(TimeSpan.FromSeconds(10))
            .Subscribe(_ => migratorObservableProvider.TrackDuplicatePhotos(cts.Token));

        CycleOnExit();
    }

    private void CycleOnExit() // todo: change in --watch mode + on ci call add timer duration.
    {
        bool allSaved;
        Thread.Sleep(7500); // to bootstrap stuff
        do
        {
            Thread.Sleep(500);
            allSaved = !_savedEntities.IsEmpty && _savedEntities.All(x => x.Value);
            if (allSaved)
            {
                ColoredConsole.WriteLine("All photos saved. You may press any key to close this page.", Colors.bgInfo);
            }
        } while (!allSaved);
    }

    private void LogEntitiesToAdd(PhotoId photoId)
    {
        ColoredConsole.WriteLine($"        > photo #{photoId.flickrId} needs to be added", Colors.txtMuted);
        _savedEntities.AddOrUpdate(photoId.flickrId, false, (_, _) => false);
    }

    private void LogDataSaved(SavePhotoResult[] results)
    {
        foreach (var result in results)
        {
            ColoredConsole.WriteLine($"        > data for #{result.Id} saved", Colors.txtSuccess);
            _savedEntities.AddOrUpdate(result.Id, true, (_, _) => true);
        }
    }
}
