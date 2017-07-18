docker build -f Dockerfile.build -t birds
docker run -it -p 80:5002 birds