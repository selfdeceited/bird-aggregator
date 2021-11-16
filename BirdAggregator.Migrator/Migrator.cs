using System;
using System.Collections.Concurrent;
using System.Linq;
using static BirdAggregator.Migrator.Program;
using System.Reactive.Linq;
using System.Threading.Tasks;
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

        public Task Run()
        {
            var tcs = new TaskCompletionSource();
            ColoredConsole.WriteLine("migrator started.", Colors.txtInfo);
            // todo: if photo exists, but caption is changed - remove photo from the db
            
            _m.EnsureCollectionsExist()
                .SelectMany(_ => _m.GetPages())
                .SelectMany(_m.GetPhotoId)
                .Where(x => _m.ShouldUpdateDb(x).Wait())
                .Do(LogEntitiesToAdd)
                .SelectMany(_m.GetPhotoInfoForSave)
                .SelectMany(x => _m.SavePhoto(x))
                .Catch<SavePhotoResult, Exception>(_ =>
                {
                    ColoredConsole.WriteLine($"exception occured: {_.Message} {_.StackTrace}", Colors.bgDanger);
                    return Observable.Empty<SavePhotoResult>();
                })
                .Do(LogDataSaved)
                .TakeUntil(DateTimeOffset.Now.AddMinutes(5))
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .DistinctUntilChanged()
                
                .Subscribe(_ =>
                {
                    var allSaved = _savedEntities.All(x => true);
                    if (!allSaved)
                        return;
                    
                    ColoredConsole.WriteLine($"All photos saved. You may close this page", Colors.bgInfo);
                    tcs.SetResult();
                });
           
           return tcs.Task;
        }

        private void LogEntitiesToAdd(PhotoId photoId)
        {
            ColoredConsole.WriteLine($"        > photo #{photoId.flickrId} needs to be added", Colors.txtMuted);
            _savedEntities.AddOrUpdate(photoId.flickrId, false, (_, _) => false);
        }


        private void LogDataSaved(SavePhotoResult _)
        {
            ColoredConsole.WriteLine($"        > data for #{_.Id} saved", Colors.txtSuccess);
            _savedEntities.AddOrUpdate(_.Id, true, (_, _) => true);
        }
    }
}