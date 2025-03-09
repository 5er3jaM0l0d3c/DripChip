# Используем официальный образ .NET 8 SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл решения и восстанавливаем зависимости
COPY *.sln .
COPY DripChip/DripChipAPI.csproj DripChip/
COPY Entities/Entities.csproj Entities/
COPY Services/Services.csproj Services/
RUN dotnet restore

# Копируем все файлы проекта
COPY . .

# Собираем и публикуем проект
WORKDIR /src/DripChip
RUN dotnet publish -c Release -o /app

# Используем официальный образ .NET 8 Runtime для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Копируем собранное приложение из предыдущего этапа
COPY --from=build /app .

# Открываем порт, который использует приложение
EXPOSE 81

# Запускаем приложение
ENTRYPOINT ["dotnet", "DripChipAPI.dll"]