FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EventaDors.Batch/EventaDors.Batch.csproj", "EventaDors.Batch/"]
RUN dotnet restore "EventaDors.Batch/EventaDors.Batch.csproj"
COPY . .
WORKDIR "/src/EventaDors.Batch"
RUN dotnet build "EventaDors.Batch.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventaDors.Batch.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventaDors.Batch.dll"]
