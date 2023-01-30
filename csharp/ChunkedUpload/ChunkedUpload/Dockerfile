FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ChunkedUpload/ChunkedUpload.csproj", "ChunkedUpload/"]
RUN dotnet restore "ChunkedUpload/ChunkedUpload.csproj"
COPY . .
WORKDIR "/src/ChunkedUpload"
RUN dotnet build "ChunkedUpload.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChunkedUpload.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChunkedUpload.dll"]
