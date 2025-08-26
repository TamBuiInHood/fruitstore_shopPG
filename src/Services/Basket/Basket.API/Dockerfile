FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Basket/Basket.API/Basket.API.csproj", "Services/Basket/Basket.API/"]
COPY ["BuildingBlocks/Common.Loggin/Common.Loggin.csproj", "BuildingBlocks/Common.Loggin/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/EventBus/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/Basket/Basket.API/Basket.API.csproj"

COPY . .
WORKDIR "/src/Services/Basket/Basket.API"
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]