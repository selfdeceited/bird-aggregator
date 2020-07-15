docker build -f Dockerfile.spa -t birds-spa
docker run -it -p 80:5005 birds-spa