using static BirdAggregator.Migrator.Program;
using static Colorify.Colors;
using System.Reactive;
using System.Reactive.Linq;
using System;
using System.Threading;

namespace BirdAggregator.Migrator
{
    public interface IMigrator
    {
        void Run();
    }

    public class Migrator : IMigrator
    {
        public Migrator() {

        }

        public void Run()
        {
            ColoredConsole.WriteLine("migrator started.", Colorify.Colors.txtInfo);
            ParallelExecutionTest();
        }

        public async void ParallelExecutionTest()
        {
            var o = Observable.CombineLatest(
                Observable.Start(() => { ColoredConsole.WriteLine($"Executing 1st on Thread: {Thread.CurrentThread.ManagedThreadId}", Colorify.Colors.bgInfo); return "Result A"; }),
                Observable.Start(() => { ColoredConsole.WriteLine($"Executing 2nd on Thread: {Thread.CurrentThread.ManagedThreadId}", Colorify.Colors.bgInfo); return "Result B"; }),
                Observable.Start(() => { ColoredConsole.WriteLine($"Executing 3rd on Thread: {Thread.CurrentThread.ManagedThreadId}", Colorify.Colors.bgInfo); return "Result C"; })
            ).Finally(() => Console.WriteLine("Done!"));

            foreach (string r in await o.FirstAsync())
                ColoredConsole.WriteLine(r, Colorify.Colors.txtInfo);
        }
    }
}