using System;
using System.Text.Json;
using BirdAggregator.Migrator.ResponseModels;
using Colorify;
using static BirdAggregator.Migrator.Program;

namespace BirdAggregator.Migrator
{
    public class ConsoleObserver : IObserver<PhotoResponse.Photo>
    {
        public void OnCompleted()
        {
            ColoredConsole.WriteLine("Done!", Colors.txtSuccess);
        }

        public void OnError(Exception error)
        {
            ColoredConsole.WriteLine($"{error}", Colors.bgDanger);
        }

        public void OnNext(PhotoResponse.Photo value)
        {
            ColoredConsole.WriteLine($"{JsonSerializer.Serialize(value)}", Colors.txtMuted);
        }
    }
}