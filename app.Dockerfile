FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ./TasteTrailIdentity/src/TasteTrailIdentity.Api/*.csproj .TasteTrailIdentity/src/TasteTrailIdentity.Api/
COPY ./TasteTrailIdentity/src/TasteTrailIdentity.Infrastructure/*.csproj .TasteTrailIdentity/src/TasteTrailIdentity.Infrastructure/
COPY ./TasteTrailIdentity/src/TasteTrailIdentity.Core/*.csproj .TasteTrailIdentity/src/TasteTrailIdentity.Core/

COPY . .

RUN dotnet publish TasteTrailIdentity/src/TasteTrailIdentity.Api/TasteTrailIdentity.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "TasteTrailIdentity.Api.dll" ]