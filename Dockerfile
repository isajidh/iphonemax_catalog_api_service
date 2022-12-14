FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5001

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser
USER root

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Ecom.Catalog.Service.csproj", "./"]
RUN dotnet restore "Ecom.Catalog.Service.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Ecom.Catalog.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecom.Catalog.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Disabled to test in the production environment 
# ENTRYPOINT ["dotnet", "Ecom.Catalog.Service.dll"]

EXPOSE 5001
EXPOSE 5000

LABEL org.opencontainers.image.source="https://github.com/isajidh/iphonemax_catalog_api_service"