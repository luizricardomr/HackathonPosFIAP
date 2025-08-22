FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV DOTNET_ENVIRONMENT=Production

RUN mkdir -p /app/uploads && chmod -R 777 /app/uploads

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VideoQRCode.API/VideoQRCode.API.csproj", "src/VideoQRCode.API/"]
COPY ["src/VideoQRCode.Core/VideoQRCode.Core.csproj", "src/VideoQRCode.Core/"]
RUN dotnet restore "./src/VideoQRCode.API/VideoQRCode.API.csproj"
COPY . .
WORKDIR "/src/src/VideoQRCode.API"
RUN dotnet build "./VideoQRCode.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VideoQRCode.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoQRCode.API.dll"]
