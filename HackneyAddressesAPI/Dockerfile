
FROM microsoft/dotnet:latest
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

CMD ASPNETCORE_URLS=http://+:${PORT:-80} dotnet ./out/LBHAddressesAPI.dll

EXPOSE ${PORT:-80}