
FROM microsoft/dotnet:latest
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out



COPY newrelic ./newrelic

CMD ASPNETCORE_URLS=http://+:${PORT:-3000} dotnet ./out/LBHAddressesAPI.dll

EXPOSE ${PORT:-3000}