FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Hosts/ClassifiedsApi.Api/ClassifiedsApi.Api.csproj", "src/Hosts/ClassifiedsApi.Api/"]
COPY ["src/Contracts/ClassifiedsApi.Contracts/ClassifiedsApi.Contracts.csproj", "src/Contracts/ClassifiedsApi.Contracts/"]
COPY ["src/Infrastructure/ClassifiedsApi.ComponentRegistrar/ClassifiedsApi.ComponentRegistrar.csproj", "src/Infrastructure/ClassifiedsApi.ComponentRegistrar/"]
COPY ["src/Infrastructure/ClassifiedsApi.DataAccess/ClassifiedsApi.DataAccess.csproj", "src/Infrastructure/ClassifiedsApi.DataAccess/"]
COPY ["src/Domain/ClassifiedsApi.Domain/ClassifiedsApi.Domain.csproj", "src/Domain/ClassifiedsApi.Domain/"]
COPY ["src/Infrastructure/ClassifiedsApi.Infrastructure/ClassifiedsApi.Infrastructure.csproj", "src/Infrastructure/ClassifiedsApi.Infrastructure/"]
COPY ["src/Application/ClassifiedsApi.AppServices/ClassifiedsApi.AppServices.csproj", "src/Application/ClassifiedsApi.AppServices/"]
RUN dotnet restore "src/Hosts/ClassifiedsApi.Api/ClassifiedsApi.Api.csproj"
COPY . .
WORKDIR "/src/src/Hosts/ClassifiedsApi.Api"
RUN dotnet build "ClassifiedsApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ClassifiedsApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClassifiedsApi.Api.dll"]
