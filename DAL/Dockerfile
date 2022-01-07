FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DAL/DAL.csproj", "DAL/"]
RUN dotnet restore "DAL/DAL.csproj"
COPY . .
WORKDIR "/src/DAL"
RUN dotnet build "DAL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DAL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DAL.dll"]
