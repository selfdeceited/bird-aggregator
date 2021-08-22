using static BirdAggregator.Migrator.Program;
using System.Reactive.Linq;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Migrator.Services;
using Colorify;

namespace BirdAggregator.Migrator
{
    public interface IMigrator
    {
        Task Run();
    }

    public class Migrator : IMigrator
    {
        private IMigrationExecutor _m;

        public Migrator(IMigrationExecutor migrationExecutor)
        {
            _m = migrationExecutor;
        }

        public async Task Run()
        {
            ColoredConsole.WriteLine("migrator started.", Colorify.Colors.txtInfo);

            // each N minutes
            // fetch number of pages
            // if already running - ignore
            
            // for each page, store list of all photoIds and captions

            // detect diff - what ids with same captions are not in the database.
            // if photo exists, but caption is changed - remove photo from the db.

            // if id is not in the database - populate photo
            // if photo has bird that is not in the database - populate bird info


            var routine = Observable
                .FromAsync(_m.GetPages)
                .SelectMany(p => Observable.Range(0, p))
                .SelectMany(pageNumber => Observable
                    .FromAsync(ct => _m.GetPhotoInfoForPage(pageNumber, ct))
                    .SelectMany(o => o))
                .Where(x =>
                    Observable
                        .FromAsync(ct => _m.RequireDatabaseUpdate(x, ct))
                        .Wait()
                )
                .Do(x =>
                    ColoredConsole.WriteLine($"        > photo {x.title} ({x.flickrId}) needs to be added",
                        Colors.txtMuted))
                .Select(x => Observable
                    .FromAsync(ct => _m.GetPhotoInfo(x, ct))
                )
                .SelectMany(x => x)
                .Do(x => Observable
                    .FromAsync(ct => _m.SavePhotoInformation(x, ct))
                    .Wait()
                )
                .Do(photo =>
                {
                    ColoredConsole.WriteLine($"        > data for photo {photo.title._content} ({photo.dates.taken}) saved",
                        Colors.txtSuccess);
                });


            foreach (var _ in routine)
            {
                ColoredConsole.WriteLine($"            {_.title._content}", Colors.bgMuted);
            }
        }
    }
}