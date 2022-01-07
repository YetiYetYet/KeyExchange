FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KeyExchangeApplication/KeyExchangeApplication.csproj", "KeyExchangeApplication/"]
RUN dotnet restore "KeyExchangeApplication/KeyExchangeApplication.csproj"
COPY . .
WORKDIR "/src/KeyExchangeApplication"
RUN dotnet build "KeyExchangeApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KeyExchangeApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KeyExchangeApplication.dll"]
