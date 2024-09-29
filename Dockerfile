FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /app

COPY --link Natak/*.csproj .
RUN dotnet restore -a $TARGETARCH

COPY --link Natak/. .
RUN dotnet publish Natak.API -a $TARGETARCH -c Release -o /publish --no-restore --self-contained

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --link --from=build /publish .
ENTRYPOINT ["./Natak.API"]
