# Use the official .NET 8 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["PersonService.csproj", "./"]
RUN dotnet restore "PersonService.csproj"

# Copy everything else and build
COPY . ./PersonService
WORKDIR "/src/PersonService"
RUN dotnet build "PersonService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonService.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PersonService.dll"]
