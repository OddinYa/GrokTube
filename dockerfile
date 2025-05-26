# Этап сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .  # Копируем ВСЕ файлы репозитория в контейнер

# Восстанавливаем зависимости (важно для работы с NuGet пакетами)
RUN dotnet restore "GrokTube/GrokTube.csproj"

# Собираем релизную версию в папку /app/publish
RUN dotnet publish "GrokTube/GrokTube.csproj" -c Release -o /app/publish

# Этап запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .  # Берём собранные файлы из этапа build
ENTRYPOINT ["dotnet", "GrokTube.dll"]  # Стартовая команда
