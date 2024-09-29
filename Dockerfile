FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish Natak --runtime linux-arm -c Release -o publish --self-contained

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --link --from=build publish .
ENTRYPOINT ["./Natak.API"]
