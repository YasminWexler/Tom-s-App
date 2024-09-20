# שלב 1: השתמשי בדימוי של .NET ליצירת הקוד
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# העתיקי את קבצי הפרויקט והתקיני תלויות
COPY TomWebApi/TomWebApi.csproj ./
RUN dotnet restore

# העתיקי את שאר הקבצים ובני את האפליקציה
COPY . ./
RUN dotnet publish -c Release -o out

# שלב 2: השתמשי בדימוי runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# הגדרת הפקודה שתתבצע בהרצה
ENTRYPOINT ["dotnet", "TomWebApi.dll"]
