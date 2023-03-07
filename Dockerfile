FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SW.Auth.Web/SW.Auth.Web.csproj", "SW.Auth.Web/"]
COPY ["SW.Auth.Web/SW.Auth.Sdk.csproj", "SW.Auth.Sdk/"]

RUN dotnet restore "SW.Auth.Web/SW.Auth.Web.csproj"
COPY . .
WORKDIR "/src/SW.Auth.Web"
RUN dotnet build "SW.Auth.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SW.Auth.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "SW.Auth.Web.dll"]
