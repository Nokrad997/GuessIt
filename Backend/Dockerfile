# Pobierz oficjalny obraz SDK dla .NET 8.0 do finalnego runtime
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Zainstaluj dotnet-ef na obrazie SDK, aby było dostępne w kontenerze uruchomieniowym
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Skopiuj plik projektowy i przywróć zależności
COPY ["Backend.csproj", "./"]
RUN dotnet restore "Backend.csproj"

# Skopiuj całą zawartość projektu i zbuduj aplikację
COPY . .
RUN dotnet build "Backend.csproj" -c Release -o /app/build

# Publikowanie aplikacji
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish

# Ustaw katalog roboczy na opublikowaną aplikację
WORKDIR /app/publish

# Ustawienie entrypointu do uruchomienia aplikacji
ENTRYPOINT ["dotnet", "Backend.dll"]
