#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MicroService.Basic.API/MicroService.Basic.API.csproj", "MicroService.Basic.API/"]
RUN dotnet restore "MicroService.Basic.API/MicroService.Basic.API.csproj"
COPY . .
WORKDIR "/src/MicroService.Basic.API"
RUN dotnet build "MicroService.Basic.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroService.Basic.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroService.Basic.API.dll"]