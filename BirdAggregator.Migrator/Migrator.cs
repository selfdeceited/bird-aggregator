using System;
using static BirdAggregator.Migrator.Program;
using System.Reactive.Linq;
using BirdAggregator.Migrator.Providers;
using BirdAggregator.Migrator.ResponseModels;
using Colorify;

namespace BirdAggregator.Migrator
{
    public class Migrator : IMigrator
    {
        private readonly IMigratorObservableProvider _m;

        public Migrator(IMigratorObservableProvider migratorObservableProvider)
        {
            _m = migratorObservableProvider;
        }

        public void Run()
        {
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
                .Do(x => _m.SavePhoto(x).Wait())
                .Do(LogDataSaved)
                .TakeUntil(DateTimeOffset.Now.AddMinutes(5))
                .Subscribe(OnProcessed);
        }

        private void LogEntitiesToAdd(PhotoId photoId) =>
            ColoredConsole.WriteLine($"        > photo #{photoId.flickrId} needs to be added", Colors.txtMuted);
        
        private void LogDataSaved((PhotoResponse.Photo photo, Sizes sizes) _) =>
            ColoredConsole.WriteLine($"        > data for #{_.photo.id} saved", Colors.txtSuccess);

        private void OnProcessed((PhotoResponse.Photo photo, Sizes sizes) _) =>
            ColoredConsole.WriteLine($"{_.photo.title._content} ({_.photo.dates.taken}) processed", Colors.bgPrimary);
    }
}