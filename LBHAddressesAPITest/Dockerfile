FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:latest AS build
WORKDIR /src
#COPY LBHAddressesAPI.sln ./
COPY LBHAddressesAPITest.csproj LBHAddressesAPITest/
COPY . .
RUN dotnet restore LBHAddressesAPITest.csproj -nowarn:msb3202,nu1503

WORKDIR /src/LBHAddressesAPITest
RUN dotnet build -c Debug -o /app
