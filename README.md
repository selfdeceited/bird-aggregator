# Bird Aggregator

Website that uses Flickr API to aggregate birding photo statistics.

## Motivation

I use Flickr as a place to store birdwatching photos. After first hundred of photos, I started to wonder how I can track each visit by date and time, show birds found in some location or simply store a lifelist.

Also since I'm lazy, I would like to create an albums of bird photos, aggregated by type, specific trip or whatever comes in hand.

So I decided to fetch data by Flickr API and expose some website to get this info regarding on what you want. So if you follow the convention below, you would only need to set your name and flickrId and this gallery would launch for your photos.

The lifelist, however, can be checked out at related [cli app](https://github.com/selfdeceited/bird-aggregator-cli) as well.

## Conventions

Names should be aligned according to latest [IOC World Bird List](http://www.worldbirdnames.org/).

Several birds in one photo supported by comma-delimiters.

- _B: Common Goldeneye (Bucephala clangula)_
- _B: Caspian Tern (Hydroprogne caspia), Mute Swan (Cygnus olor)_

I don't support subspecies by now, like  _Buteo Buteo vulpinus_.

## Development

- Start locally: `./utils/run-back.sh` && `./utils/run-front.sh`
- Start locally in containers: `docker-compose up`

## Plans
Right now the quality of the soffware is disturbing, so I'm occupied by tech debt. Also, it's my sandbox.I'm checking some stuff that I don't usually use (yet) in my everyday work, like `RX.NET` or `linaria`, or anything new that's on my tech radar I eager to access.

More info on how to make it great again is here - [TODO.md](./TODO.md)

Later on the ideas for the features will come back, once I'm satisfied with the tech part enough.