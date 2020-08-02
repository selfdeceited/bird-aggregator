docker build -f deploy/Dockerfile.api -t birds-api .
docker run -it -p 80:5006 birds-api