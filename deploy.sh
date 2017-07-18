docker build -t birds .
docker tag birds registry.heroku.com/bird-aggregator/web
docker push registry.heroku.com/bird-aggregator/web