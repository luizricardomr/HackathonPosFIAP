# Stage base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV DOTNET_ENVIRONMENT=Production

# Criar pasta de uploads
RUN mkdir -p /app/uploads && chmod -R 777 /app/uploads

# Stage build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY ["src/VideoQRCode.DAO/VideoQRCode.DAO.csproj", "src/VideoQRCode.DAO/"]
COPY ["src/VideoQRCode.Core/VideoQRCode.Core.csproj", "src/VideoQRCode.Core/"]
RUN dotnet restore "./src/VideoQRCode.DAO/VideoQRCode.DAO.csproj"

# Copiar restante do código e compilar
COPY . .
WORKDIR "/src/src/VideoQRCode.DAO"
RUN dotnet build "./VideoQRCode.DAO.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VideoQRCode.DAO.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar app publicado
COPY --from=publish /app/publish .

RUN apt-get update && apt-get install -y \
    ffmpeg \
    libfontconfig1 \
    libfreetype6 \
    libx11-6 \
    libx11-xcb1 \
    libxcb1 \
    libxrender1 \
    libxi6 \
    libglib2.0-0 \
    libgtk-3-0 \
    libharfbuzz0b \
    --no-install-recommends && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["dotnet", "VideoQRCode.DAO.dll"]
