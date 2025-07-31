# Runtime aşaması için temel imaj
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece proje dosyasını kopyala ve restore et
COPY ["Stock.API/Stock.API.csproj", "Stock.API/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Stock.API/Stock.API.csproj"

# Şimdi tüm kaynak kodları al
COPY Stock.API/ Stock.API/
COPY Shared/ Shared/   
WORKDIR /src/Stock.API
RUN dotnet publish "Stock.API.csproj" -c Release -o /app/publish

# Yayın aşaması (final)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Stock.API.dll"]
