# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NerdstoreEnterprise.props", "./"]
COPY ["website/NSE.WebApp.MVC/NSE.WebApp.MVC.csproj", "website/NSE.WebApp.MVC/"]
COPY ["building-blocks/NSE.Core/NSE.Core.csproj", "building-blocks/NSE.Core/"]
COPY ["building-blocks/NSE.WebAPI.Core/NSE.WebAPI.Core.csproj", "building-blocks/NSE.WebAPI.Core/"]
RUN dotnet restore "./website/NSE.WebApp.MVC/NSE.WebApp.MVC.csproj"
COPY . .
WORKDIR "/src/website/NSE.WebApp.MVC"
RUN dotnet build "./NSE.WebApp.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NSE.WebApp.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.WebApp.MVC.dll"]