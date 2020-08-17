docker build -f deploy/Dockerfile.spa -t birds-spa .
docker run -it -p 10003:5005 birds-spa