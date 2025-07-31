FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderAPI/OrderAPI.csproj", "OrderAPI/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "OrderAPI/OrderAPI.csproj"


COPY . .
WORKDIR "/src/OrderAPI"
RUN dotnet build "OrderAPI.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "OrderAPI.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderAPI.dll"]
