# Bird Aggregator

Website that uses Flickr API to aggregate bird shot stats.

## Motivation

I use Flickr as a place to store photos I'm doing, and lately I'm enjoying to ~shoot~ take photos of birds in their natural habitat.
After first hundred of photos, I became wondering of how I can track type visit by date and time, show birds found in some location or simply store a life-list.
Also since I'm lazy, I would like to create an albums of bird photos, aggregated by type, specific trip or whatever comes in handy.

So I decided to get shots by Flickr API and expose some website to get info regarding on what you want so that you would only need to set your name and flickrId and this gallery would work for you.

## Conventions

Names should be aligned according to latest [IOC World Bird List](http://www.worldbirdnames.org/).

Several birds in one photo supported by comma-delimeters.

- _B: Common Goldeneye (Bucephala clangula)_
- _B: Caspian Tern (Hydroprogne caspia), Mute Swan (Cygnus olor)_

## Development

Start locally: `docker-compose up`

### API container bootstrapping

`./deploy/build-api.sh`

https://github.com/dotnet/AspNetCore.Docs/issues/6199
