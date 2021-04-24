FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EventaDors.Web/EventaDors.Web.csproj", "EventaDors.Web/"]
RUN dotnet restore "EventaDors.Web/EventaDors.Web.csproj"
COPY . .
WORKDIR "/src/EventaDors.Web"
RUN dotnet build "EventaDors.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventaDors.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventaDors.Web.dll"]
