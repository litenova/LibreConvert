FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY . ./
RUN dotnet publish -c release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "LibreConvert.WebApi.dll"]
