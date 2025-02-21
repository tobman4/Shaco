FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY ./src /src

RUN dotnet build -c Release -o /build Shaco.API

FROM mcr.microsoft.com/dotnet/aspnet:9.0

EXPOSE 8080
VOLUME /var/shaco

WORKDIR /build
COPY --from=build /build /build

CMD [ "dotnet", "Shaco.API.dll" ]
