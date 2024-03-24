#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MongoCRUD/MongoCRUD.csproj", "MongoCRUD/"]
RUN dotnet restore "./MongoCRUD/MongoCRUD.csproj"
COPY . .
WORKDIR "/src/MongoCRUD"
RUN dotnet build "./MongoCRUD.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MongoCRUD.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MongoCRUD.dll"]