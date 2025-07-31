FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# SADECE Mail.API csproj dosyasını kopyala ve restore et
COPY Mail.API/Mail.API.csproj Mail.API/
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Mail.API/Mail.API.csproj"

# Ardından sadece bu API'nin tüm içeriğini kopyala
COPY Mail.API/ Mail.API/
COPY Shared/ Shared/ 
WORKDIR /src/Mail.API
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Mail.API.dll"]