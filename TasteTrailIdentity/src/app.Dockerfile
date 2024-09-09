FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ./TasteTrailIdentity.Api/*.csproj ./TasteTrailIdentity.Api/
COPY ./TasteTrailIdentity.Infrastructure/*.csproj ./TasteTrailIdentity.Infrastructure/
COPY ./TasteTrailIdentity.Core/*.csproj ./TasteTrailIdentity.Core/

COPY . .

RUN dotnet publish TasteTrailIdentity.Api/*.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "TasteTrailIdentity.Api.dll" ]