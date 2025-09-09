# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# O Render define PORT; vamos escutar nele
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo da solução
COPY . .

# Restaura e publica o projeto web (ajuste o nome se for diferente)
RUN dotnet restore "HubWalks/HubWalks.csproj"
RUN dotnet publish "HubWalks/HubWalks.csproj" -c Release -o /app

# Final
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "HubWalks.dll"]
