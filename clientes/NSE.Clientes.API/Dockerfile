#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["NerdstoreEnterprise.props", "./"]
COPY ["./clientes/NSE.Clientes.API/NSE.Clientes.API.csproj", "clientes/NSE.Clientes.API/"]
COPY ["./building-blocks/NSE.Core/NSE.Core.csproj", "building-blocks/NSE.WebAPI.Core/"]
COPY ["./building-blocks/NSE.WebAPI.Core/NSE.WebAPI.Core.csproj", "building-blocks/NSE.WebAPI.Core/"]
COPY ["./building-blocks/NSE.MessageBus/NSE.MessageBus.csproj", "building-blocks/NSE.MessageBus/"]

RUN dotnet restore "clientes/NSE.Clientes.API/NSE.Clientes.API.csproj"
COPY . .
WORKDIR "clientes/NSE.Clientes.API"
RUN dotnet build "NSE.Clientes.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Clientes.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Clientes.API.dll"]