using System;
using System.Collections.Concurrent;
using System.Linq;
using static BirdAggregator.Migrator.Program;
using System.Reactive.Linq;
using System.Threading.Tasks;
using BirdAggregator.Migrator.Providers;
using BirdAggregator.Migrator.ResponseModels;
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

            // [ ]: each N minutes
            // [x]: fetch number of pages
            // [ ]: if already running - ignore
            // [x]: for each page, store list of all photoIds and captions
            // [x]: detect diff - what ids with same captions are not in the database.
            // [ ]: if photo exists, but caption is changed - remove photo from the db.
            // [x]: if id is not in the database - populate photo
            // [ ]: if photo has bird that is not in the database - populate bird info
            
            _m.GetPages()
                .SelectMany(_m.GetPhotoId)
                .Where(x => _m.ShouldUpdateDb(x).Wait())
                .Do(LogEntitiesToAdd)
                .SelectMany(_m.GetPhotoWithSizesByPhotoId)
                .Do(x => _m.SavePhoto(x))
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


        private void LogDataSaved((PhotoResponse.Photo photo, Sizes sizes) _)
        {
            ColoredConsole.WriteLine($"        > data for #{_.photo.id} saved", Colors.txtSuccess);
            _savedEntities.AddOrUpdate(_.photo.id, true, (_, _) => true);
        }
    }
}