# ��� 1: ������ ������ �� .NET ������ ����
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# ������ �� ���� ������� ������� ������
COPY TomWebApi/TomWebApi.csproj ./
RUN dotnet restore

# ������ �� ��� ������ ���� �� ���������
COPY . ./
RUN dotnet publish -c Release -o out

# ��� 2: ������ ������ runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# ����� ������ ������ �����
ENTRYPOINT ["dotnet", "TomWebApi.dll"]
