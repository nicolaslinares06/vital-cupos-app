FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CUPOS_FRONT/CUPOS_FRONT.csproj", "CUPOS_FRONT/"]
RUN dotnet restore "CUPOS_FRONT/CUPOS_FRONT.csproj"
COPY . .
WORKDIR "/src/CUPOS_FRONT"
RUN dotnet build "CUPOS_FRONT.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CUPOS_FRONT.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CUPOS_FRONT.dll"]