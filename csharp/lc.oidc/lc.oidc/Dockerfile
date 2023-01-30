FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["lc.oidc/lc.oidc.csproj", "lc.oidc/"]
RUN dotnet restore "lc.oidc/lc.oidc.csproj"
COPY . .
WORKDIR "/src/lc.oidc"
RUN dotnet build "lc.oidc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lc.oidc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lc.oidc.dll"]
