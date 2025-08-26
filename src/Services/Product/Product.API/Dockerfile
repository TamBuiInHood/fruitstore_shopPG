FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Product/Product.API/Product.API.csproj", "Services/Product/Product.API/"]
COPY ["BuildingBlocks/Common.Loggin/Common.Loggin.csproj", "BuildingBlocks/Common.Loggin/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
RUN dotnet restore "Services/Product/Product.API/Product.API.csproj"
COPY . .
WORKDIR "/src/Services/Product/Product.API"
RUN dotnet build "Product.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Product.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.API.dll"]
