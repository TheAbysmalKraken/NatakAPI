FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /app
COPY . .
RUN dotnet restore Natak -a $TARGETARCH
RUN dotnet publish Natak/Natak.API -a $TARGETARCH -c Release -o publish --self-contained --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["./Natak.API"]
