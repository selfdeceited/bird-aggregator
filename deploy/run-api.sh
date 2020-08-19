docker build -f deploy/Dockerfile.api -t birds-api .
docker run -it -p 10001:80 -p 10002:443 birds-api