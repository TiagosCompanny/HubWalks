# ====== Runtime ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# O Render injeta a variável PORT. Escute nela:
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# ====== Build ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie primeiro a sln e os csproj para aproveitar cache
COPY HubWalks.sln ./
COPY HubWalks/HubWalks.csproj HubWalks/
COPY HubWalks.Data/HubWalks.Data.csproj HubWalks.Data/
COPY HubWalks.Bussines/HubWalks.Bussines.csproj HubWalks.Bussines/

# Restaure a solução (respeita os ProjectReferences)
RUN dotnet restore HubWalks.sln

# Agora copie o resto do código
COPY . .

# Publique só o projeto web
RUN dotnet publish HubWalks/HubWalks.csproj -c Release -o /app

# ====== Final ======
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "HubWalks.dll"]
