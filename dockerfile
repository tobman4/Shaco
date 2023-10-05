FROM mcr.microsoft.com/dotnet/sdk:7.0 as build

WORKDIR /build

COPY ./src .

RUN dotnet restore
RUN dotnet build Shaco.Api -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
RUN mkdir /var/shaco

COPY --from=build /app .

CMD [ "dotnet", "Shaco.Api.dll" ]
