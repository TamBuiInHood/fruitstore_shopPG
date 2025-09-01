FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BuildingBlocks/Common.Loggin/Common.Loggin.csproj", "BuildingBlocks/Common.Loggin/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["Services/Inventory/Inventory.Product.API/Inventory.Product.API.csproj", "Services/Inventory/Inventory.Product.API/"]
RUN dotnet restore "Services/Inventory/Inventory.Product.API/Inventory.Product.API.csproj"
COPY . .

WORKDIR "/src/Services/Inventory/Inventory.Product.API"
RUN dotnet build "Inventory.Product.API.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "Inventory.Product.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Product.API.dll"]
