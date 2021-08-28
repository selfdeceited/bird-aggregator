# Bird Aggregator

Website that uses Flickr API to aggregate bird shot stats.

## Motivation

I use Flickr as a place to store photos I'm doing, and lately I'm enjoying to ~shoot~ take photos of birds in their natural habitat.
After first hundred of photos, I became wondering of how I can track type visit by date and time, show birds found in some location or simply store a life-list.
Also since I'm lazy, I would like to create an albums of bird photos, aggregated by type, specific trip or whatever comes in handy.

So I decided to get shots by Flickr API and expose some website to get info regarding on what you want so that you would only need to set your name and flickrId and this gallery would work for you.

This can be checked out at related [cli app](https://github.com/selfdeceited/bird-aggregator-cli) as well.

## Conventions

Names should be aligned according to latest [IOC World Bird List](http://www.worldbirdnames.org/).

Several birds in one photo supported by comma-delimiters.

- _B: Common Goldeneye (Bucephala clangula)_
- _B: Caspian Tern (Hydroprogne caspia), Mute Swan (Cygnus olor)_

I don't support subspecies by now, like  _Buteo Buteo vulpinus_

## Development

- Start locally: `./utils/run-back.sh` && `./utils/run-front.sh`
- Start locally in containers: `docker-compose up`. There are also `./deploy/build-api.sh` and `./deploy/build-spa.sh` for precise operations (:
    - UPD. since we're on NET6 now, we're on hold on that mostly focused on dev part. Since I hack on M1 now, NET6's kinda the only option apart for rapid development.


## Plans
Right now the quality of the soffware is disturbing, so I'm occupied by tech debt. Also, it's my sandbox.I'm checking some stuff that I don't usually use (yet) in my everyday work, like `RX.NET` or `linaria`, or anything new that's on my tech radar I eager to access.

`./bird-aggregator` folder is obsolete.

More info on how to make it great again is here - [TODO.md](./TODO.md)

Later on the ideas for the features will come back, once I'm satisfied with the tech part enough.