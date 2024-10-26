# Pobierz oficjalny obraz runtime dla .NET 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Pobierz oficjalny obraz SDK dla .NET 8.0 do budowania aplikacji
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Skopiuj plik projektowy i przywróć zależności
COPY ["Backend.csproj", "./"]
RUN dotnet restore "Backend.csproj"

# Skopiuj całą zawartość projektu i zbuduj aplikację
COPY . .
WORKDIR "/src"
RUN dotnet build "Backend.csproj" -c Release -o /app/build

# Publikowanie aplikacji
FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish

# Skopiowanie opublikowanej aplikacji do końcowego obrazu
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]
