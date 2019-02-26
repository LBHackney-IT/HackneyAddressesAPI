FROM microsoft/dotnet:2.2-sdk

# disable microsoft telemetry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers

COPY ./LBHAddressesAPI.sln ./
COPY ./HackneyAddressesAPI/LBHAddressesAPI.csproj ./HackneyAddressesAPI/
COPY ./LBHAddressesAPITest/LBHAddressesAPITest.csproj ./LBHAddressesAPITest/

RUN dotnet restore ./HackneyAddressesAPI/LBHAddressesAPI.csproj
RUN dotnet restore ./LBHAddressesAPITest/LBHAddressesAPITest.csproj

# Copy everything else and build
COPY . .
RUN dotnet build -c debug -o out ./LBHAddressesAPITest/LBHAddressesAPITest.csproj

CMD dotnet test ./LBHAddressesAPITest/LBHAddressesAPITest.csproj

