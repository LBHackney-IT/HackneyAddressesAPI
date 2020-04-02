
FROM microsoft/dotnet:2.2-sdk
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

ENV CORECLR_ENABLE_PROFILING=1 \
  CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
  CORECLR_NEWRELIC_HOME=./newrelic \
  CORECLR_PROFILER_PATH=./newrelic/libNewRelicProfiler.so \
  NEW_RELIC_LICENSE_KEY="${NEW_RELIC_LICENSE_KEY}" \
  NEW_RELIC_APP_NAME="${NEW_RELIC_APP_NAME}"

COPY newrelic ./newrelic

CMD ASPNETCORE_URLS=http://+:${PORT:-3000} dotnet ./out/LBHAddressesAPI.dll

EXPOSE ${PORT:-3000}