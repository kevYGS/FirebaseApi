# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia solo el archivo de proyecto para restaurar dependencias primero
COPY *.csproj ./

# Restaurar dependencias antes de copiar el c�digo completo
RUN dotnet restore

# Copia el resto del c�digo fuente
COPY . ./

# Publicar en modo Release al directorio out
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa build
COPY --from=build /app/out ./

EXPOSE 80

# Ejecutar la aplicaci�n
ENTRYPOINT ["dotnet", "FirebaseApi.dll"]
