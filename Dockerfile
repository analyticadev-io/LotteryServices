FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copiar el archivo csproj al contenedor
COPY LotteryServices/LotteryServices.csproj ./LotteryServices/

# Restaurar las dependencias
RUN dotnet restore ./LotteryServices/LotteryServices.csproj

# Copiar el resto del código al contenedor
COPY . .

# Copiar el archivo ca.pem al contenedor
COPY LotteryServices/Utilitys/ca.pem ./Utilitys/

# Publicar la aplicación
RUN dotnet publish ./LotteryServices/LotteryServices.csproj -c Release -o /app/output

# Etapa final, copiar los archivos publicados y configurar el contenedor
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/output .
COPY --from=build /app/Utilitys ./Utilitys

# Exponer los puertos que la aplicación usará
EXPOSE 80
EXPOSE 5001

# Comando de entrada
ENTRYPOINT ["dotnet", "LotteryServices.dll"]
