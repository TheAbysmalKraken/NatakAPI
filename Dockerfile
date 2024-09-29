FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Natak
COPY . .
RUN dotnet publish Natak.API/Natak.API.csproj -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /Natak
COPY --from=build /Natak/publish .
ENTRYPOINT ["dotnet", "Natak.API.dll"]
