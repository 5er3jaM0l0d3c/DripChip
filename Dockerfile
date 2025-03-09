# ���������� ����������� ����� .NET 8 SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# �������� ���� ������� � ��������������� �����������
COPY *.sln .
COPY DripChip/DripChipAPI.csproj DripChip/
COPY Entities/Entities.csproj Entities/
COPY Services/Services.csproj Services/
RUN dotnet restore

# �������� ��� ����� �������
COPY . .

# �������� � ��������� ������
WORKDIR /src/DripChip
RUN dotnet publish -c Release -o /app

# ���������� ����������� ����� .NET 8 Runtime ��� �������
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# �������� ��������� ���������� �� ����������� �����
COPY --from=build /app .

# ��������� ����, ������� ���������� ����������
EXPOSE 81

# ��������� ����������
ENTRYPOINT ["dotnet", "DripChipAPI.dll"]