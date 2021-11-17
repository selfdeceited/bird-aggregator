using System;
using System.Collections.Concurrent;
using System.Linq;
using static BirdAggregator.Migrator.Program;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using BirdAggregator.Migrator.Providers;
using Colorify;

namespace BirdAggregator.Migrator
{
    public class Migrator : IMigrator
    {
        private readonly IMigratorObservableProvider _m;

        private readonly ConcurrentDictionary<string, bool> _savedEntities = new();
        
        public Migrator(IMigratorObservableProvider migratorObservableProvider)
        {
            _m = migratorObservableProvider;
        }

        public void Run()
        {
            ColoredConsole.WriteLine("Migrator started. Press any key to stop it.", Colors.txtInfo);
            // todo: if photo exists, but caption is changed - remove photo from the db

            _m.EnsureCollectionsExist()
                .SelectMany(_ => _m.GetPages())
                .SelectMany(_m.GetPhotoId)
                .Where(x => _m.ShouldUpdateDb(x).Wait())
                .Do(LogEntitiesToAdd)
                .SelectMany(_m.GetPhotoInfoForSave)
                .DistinctUntilChanged()
                .Buffer(TimeSpan.FromMilliseconds(1000))
                .SelectMany(x => _m
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
            
            // todo: schedule duplicate photos fixes
            CycleOnExit();
        }

        private void CycleOnExit() //todo: change in --watch mode + on ci call add timer duration.
        {
            bool allSaved;
            Thread.Sleep(7500); // to bootstrap stuff
            do
            {
                Thread.Sleep(500);
                allSaved = _savedEntities.Any() && _savedEntities.All(x => x.Value);
                if (allSaved)
                {
                    ColoredConsole.WriteLine("All photos saved. You may press any key this page.", Colors.bgInfo);
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
}