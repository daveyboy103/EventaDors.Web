FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EventaDors.Data/EventaDors.Data.csproj", "EventaDors.Data/"]
RUN dotnet restore "EventaDors.Data/EventaDors.Data.csproj"
COPY . .
WORKDIR "/src/EventaDors.Data"
RUN dotnet build "EventaDors.Data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventaDors.Data.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventaDors.Data.dll"]
