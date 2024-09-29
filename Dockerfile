FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish Natak/Natak.API/Natak.API.csproj -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build publish .
ENTRYPOINT ["dotnet", "Natak.API.dll"]
