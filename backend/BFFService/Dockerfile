FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BFFService.csproj", "./"]
RUN dotnet restore "BFFService.csproj"

COPY . ./BFFService
WORKDIR "/src/BFFService"
RUN dotnet build "BFFService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BFFService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "BFFService.dll"]
