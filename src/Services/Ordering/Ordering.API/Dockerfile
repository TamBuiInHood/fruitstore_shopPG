FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Services/Ordering/Ordering.API/Ordering.API.csproj", "Services/Ordering/Ordering.API/"]
COPY ["Services/Ordering/Ordering.Application/Ordering.Application.csproj", "Services/Ordering/Ordering.Application/"]
COPY ["Services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "Services/Ordering/Ordering.Domain/"]
COPY ["Services/Ordering/Ordering.Infastructure/Ordering.Infastructure.csproj", "Services/Ordering/Ordering.Infastructure/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/EventBus/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]

RUN dotnet restore "Services/Ordering/Ordering.API/Ordering.API.csproj"

COPY . .
WORKDIR "/src/Services/Ordering/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build As publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base As final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]