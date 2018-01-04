using System;
using System.Threading;
using System.Threading.Tasks;
using birds;
using birds.Services;
using Microsoft.AspNetCore.SignalR;

namespace bird_aggregator.Hubs
{
    public class SeedLauncher
    {
        private readonly IHubContext<SeedHub> _clients;
        private readonly SemaphoreSlim _seedStartLock = new SemaphoreSlim(1, 1);
        private volatile bool seedCompleted = false;


        public SeedLauncher(IHubContext<SeedHub> clients){
            _clients = clients;
        }

        internal async Task WrapLaunch(Action action, bool anythingSaved){
            if (seedCompleted || anythingSaved)
            {
                await HidePopup();
                return;
            };

            await _seedStartLock.WaitAsync();
            try
            {
                if (seedCompleted || anythingSaved)
                {
                    await HidePopup();
                    return;
                }
                action();
                seedCompleted = true;
            }
            finally
            {
                _seedStartLock.Release();
            }
        }
        internal async Task BroadcastCount(int count)
        {
            await _clients.Clients.All.InvokeAsync("QuantitySet", count);
        }

        internal async Task Log(string message)
        {
            await _clients.Clients.All.InvokeAsync("Log", message);
        }

        internal async Task HidePopup()
        {
            await _clients.Clients.All.InvokeAsync("HidePopup");
        }

        internal async Task BirdSaved()
        {
            await _clients.Clients.All.InvokeAsync("BirdSaved");
        }

        internal async Task BroadcastBirdCount(int count)
        {
            await _clients.Clients.All.InvokeAsync("BirdCount", count);
        }

        internal async Task PhotoSaved()
        {
            await _clients.Clients.All.InvokeAsync("PhotoSaved");
        }
    }
}