# 1. Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece csproj dosyasını kopyala ve restore yap
COPY ["Payment.API/Payment.API.csproj", "Payment.API/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Payment.API/Payment.API.csproj"


COPY . .

WORKDIR "/src/Payment.API"
RUN dotnet publish "Payment.API.csproj" -c Release -o /app/publish

# 3. Publish edilen çıktıyı kopyala
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Payment.API.dll"]
