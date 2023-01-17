FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MinimalApi.Worker/MinimalApi.Worker.csproj", "MinimalApi.Worker/"]
RUN dotnet restore "MinimalApi.Worker/MinimalApi.Worker.csproj"
COPY . .
WORKDIR "/src/MinimalApi.Worker"
RUN dotnet build "MinimalApi.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApi.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalApi.Worker.dll"]
