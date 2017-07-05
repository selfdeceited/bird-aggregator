rm -rf bin
dotnet publish -c release
cp ./Dockerfile ./bin/Release/netcoreapp2.0/publish
docker build -t birds ./bin/Release/netcoreapp2.0/publish
docker tag birds registry.heroku.com/bird-aggregator/web
docker push registry.heroku.com/bird-aggregator/web