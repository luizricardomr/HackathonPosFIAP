# Stage base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV DOTNET_ENVIRONMENT=Production

# Instalar ffmpeg e bibliotecas nativas para SkiaSharp
RUN apt-get update && apt-get install -y \
    ffmpeg \
    libfontconfig1 \
    libfreetype6 \
    libx11-6 \
    libxcomposite1 \
    libxext6 \
    libxrender1 \
    libxrandr2 \
    libgtk-3-0 \
    libharfbuzz0b \
    libpng16-16 \
    libjpeg62-turbo \
    libglib2.0-0 \
    libc6 \
    libxcb1 \
    libgl1 \
    libx11-xcb1 \
    libxkbcommon0 \
    libxkbcommon-x11-0 \
    --no-install-recommends && rm -rf /var/lib/apt/lists/*

# Criar pasta uploads com permissão total
RUN mkdir -p /app/uploads && chmod -R 777 /app/uploads

# Stage build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar csproj e restaurar
COPY ["src/VideoQRCode.DAO/VideoQRCode.DAO.csproj", "src/VideoQRCode.DAO/"]
COPY ["src/VideoQRCode.Core/VideoQRCode.Core.csproj", "src/VideoQRCode.Core/"]
RUN dotnet restore "./src/VideoQRCode.DAO/VideoQRCode.DAO.csproj"

# Copiar restante do código e buildar
COPY . .
WORKDIR "/src/src/VideoQRCode.DAO"
RUN dotnet build "./VideoQRCode.DAO.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VideoQRCode.DAO.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "VideoQRCode.DAO.dll"]
