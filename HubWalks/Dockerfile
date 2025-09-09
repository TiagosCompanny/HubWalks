# Etapa base: runtime ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Etapa de build: SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "HubWalks/HubWalks.csproj"
RUN dotnet publish "HubWalks/HubWalks.csproj" -c Release -o /app

# Etapa final: copia build para runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "HubWalks.dll"]
