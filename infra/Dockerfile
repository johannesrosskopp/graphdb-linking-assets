# syntax=docker/dockerfile:1

# ─── Build stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY graphdb-linking-assets.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish graphdb-linking-assets.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ─── Runtime stage ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Container Apps / ACA expects HTTP on port 80 by default
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "graphdb-linking-assets.dll"]
