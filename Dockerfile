FROM microsoft/dotnet:2.0.0-preview2-runtime

WORKDIR /app  
COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet birds.dll