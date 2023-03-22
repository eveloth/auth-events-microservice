FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/AuthEvents/AuthEvents.csproj", "src/AuthEvents/"]
RUN dotnet restore "src/AuthEvents/AuthEvents.csproj"
COPY . .
WORKDIR "/src/src/AuthEvents"
RUN dotnet build "AuthEvents.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthEvents.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthEvents.dll"]
