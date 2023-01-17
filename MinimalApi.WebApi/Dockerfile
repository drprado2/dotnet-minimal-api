FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MinimalApiStudy/MinimalApiStudy.csproj", "MinimalApiStudy/"]
RUN dotnet restore "MinimalApiStudy/MinimalApiStudy.csproj"
COPY . .
WORKDIR "/src/MinimalApiStudy"
RUN dotnet build "MinimalApiStudy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApiStudy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalApiStudy.dll"]
