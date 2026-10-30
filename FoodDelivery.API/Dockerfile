# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FoodDelivery.API/FoodDelivery.API.csproj", "FoodDelivery.API/"]
COPY ["FoodDelivery.BLL/FoodDelivery.BLL.csproj", "FoodDelivery.BLL/"]
COPY ["FoodDelivery.DAL/FoodDelivery.DAL.csproj", "FoodDelivery.DAL/"]
COPY ["FoodDelivery.Models/FoodDelivery.Models.csproj", "FoodDelivery.Models/"]
COPY ["FoodDelivery.Shared.Contracts/FoodDelivery.Shared.Contracts.csproj", "FoodDelivery.Shared.Contracts/"]
COPY ["FoodDelivery.Infrastructure/FoodDelivery.Infrastructure.csproj", "FoodDelivery.Infrastructure/"]
RUN dotnet restore "./FoodDelivery.API/FoodDelivery.API.csproj"
COPY . .
WORKDIR "/src/FoodDelivery.API"
RUN dotnet build "./FoodDelivery.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FoodDelivery.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodDelivery.API.dll"]