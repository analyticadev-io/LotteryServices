FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR LotteryServices

EXPOSE 80
EXPOSE 5001

#Archivos del proyecto
COPY /LotteryServices.csproj ./
RUN dotnet restore

#COPY
COPY . .
RUN dotnet publish -c Release -o Output

#
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /LotteryServices
COPY --from=build /LotteryServices/Output .
ENTRYPOINT ["dotnet","LotteryServices.dll"]
