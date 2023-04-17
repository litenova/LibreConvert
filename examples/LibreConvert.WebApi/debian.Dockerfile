FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY . ./
RUN dotnet publish -c release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim

# Installing LibreOffice and Java
RUN apt-get --no-install-recommends install libreoffice -y
RUN apt-get install -y libreoffice-java-common

# Microsoft Fonts
RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update
RUN echo "ttf-mscorefonts-installer msttcorefonts/accepted-mscorefonts-eula select true" | debconf-set-selections
RUN apt-get install -y --no-install-recommends fontconfig ttf-mscorefonts-installer
RUN fc-cache -f -v

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV RUNNING_OS="Debian 11"	

WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "LibreConvert.WebApi.dll"]
