FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Kertu.Solution/Kertu.InteractiveServer/Kertu.InteractiveServer.csproj", "Kertu.Solution/Kertu.InteractiveServer/"]
RUN dotnet restore "Kertu.Solution/Kertu.InteractiveServer/Kertu.InteractiveServer.csproj"
COPY . .
WORKDIR "/src/Kertu.Solution/Kertu.InteractiveServer/"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "Kertu.InteractiveServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Kertu.InteractiveServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Kertu.InteractiveServer.dll"]