# https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.chiseled

FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY . ./
RUN dotnet publish -c release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:7.0-jammy-chiseled

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "LibreConvert.WebApi.dll"]
