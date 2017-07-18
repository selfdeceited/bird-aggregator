 FROM microsoft/aspnetcore-build:2.0.0-preview2-jessie AS builder
 WORKDIR /source

 # caches restore result by copying csproj file separately
 COPY *.csproj .
 COPY package.json .
 COPY webpack.config.js .
 COPY webpack.config.prod.js .

 RUN dotnet restore

 # copies the rest of your code
 COPY . .

 RUN npm install
 RUN npm run webpack -p

 RUN dotnet publish --output /app/ --configuration Release

 # Stage 2
 FROM microsoft/aspnetcore:2.0.0-preview2-jessie
 WORKDIR /app
 COPY --from=builder /app .

 CMD ASPNETCORE_URLS=http://*:$PORT dotnet birds.dll