docker build -t birds .
docker tag birds registry.heroku.com/bird-aggregator/web
heroku container:login
heroku container:push web -a bird-aggregator
heroku container:release web -a bird-aggregator