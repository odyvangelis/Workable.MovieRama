FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./src/app/MovieRama.WebApp/MovieRama.WebApp.csproj", "src/app/MovieRama.WebApp/"]
COPY ["./src/lib/MovieRama.Core/MovieRama.Core.csproj", "src/lib/MovieRama.Core/"]
COPY ["./src/lib/MovieRama.Domain/MovieRama.Domain.csproj", "src/lib/MovieRama.Domain/"]
COPY ["./src/lib/MovieRama.Configuration/MovieRama.Configuration.csproj", "src/lib/MovieRama.Configuration/"]
COPY ["./src/lib/MovieRama.Infrastructure/MovieRama.Infrastructure.csproj", "src/lib/MovieRama.Infrastructure/"]
RUN dotnet restore "src/app/MovieRama.WebApp/MovieRama.WebApp.csproj"
COPY . .
WORKDIR "/src/src/app/MovieRama.WebApp"
RUN dotnet build "MovieRama.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MovieRama.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieRama.WebApp.dll"]
