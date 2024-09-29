FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETRUNTIME
WORKDIR /app
COPY . .
RUN dotnet publish Natak/Natak.API/Natak.API.csproj -c Release -o publish --runtime $TARGETRUNTIME --self-contained

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["./Natak.API"]
