#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# 1. Build image(https://hub.docker.com/_/microsoft-dotnet-core-sdk/)
FROM mcr.microsoft.com/dotnet/aspnet:7.0.5-alpine3.17-amd64 as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apk add --no-cache fio

FROM mcr.microsoft.com/dotnet/sdk:7.0.203-alpine3.17-amd64 AS build
WORKDIR /src

# Cache nuget restore
COPY ["src/WebApp/WebApp.csproj", "src/WebApp/"]
RUN dotnet restore "src/WebApp/WebApp.csproj"

# Copy sources and compile
COPY . .
WORKDIR "/src/src/WebApp"
RUN dotnet build "WebApp.csproj" -c Release -o /app/build

# 2. Release image
FROM build AS publish
RUN dotnet publish "WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# Copy content from Build image
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.dll"]
