# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece proje dosyasını al
COPY ["Product.API/Product.API.csproj", "Product.API/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Product.API/Product.API.csproj"

# Şimdi tüm kaynakları al
COPY . .
WORKDIR /src/Product.API
RUN dotnet publish "Product.API.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Product.API.dll"]

