docker build -t birds .
docker run -it -p 80:5002 birds
docker tag birds registry.heroku.com/bird-aggregator/web
docker push registry.heroku.com/bird-aggregator/web