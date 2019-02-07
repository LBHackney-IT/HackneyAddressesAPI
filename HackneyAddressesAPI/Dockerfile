FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE ${PORT:-80}

FROM microsoft/dotnet:latest AS build
WORKDIR /src

COPY LBHAddressesAPI.csproj HackneyAddressesAPI/

ARG VERSION_SUFFIX
ENV VERSION_SUFFIX=${VERSION_SUFFIX:-buildNumberUnsupplied}
RUN echo "Build number: $VERSION_SUFFIX"

COPY . .
RUN dotnet restore LBHAddressesAPI.csproj -nowarn:msb3202,nu1503

RUN dotnet build LBHAddressesAPI.csproj -c Release

FROM build AS publish
RUN dotnet publish LBHAddressesAPI.csproj -c Release -o /app --version-suffix "${VERSION_SUFFIX}"

FROM base AS final
WORKDIR /app

ENV CORECLR_ENABLE_PROFILING=1 \
  CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} 

COPY --from=publish /app .


EXPOSE ${PORT:-80}
CMD ASPNETCORE_URLS=http://+:${PORT:-80} dotnet LBHAddressesAPI.dll
