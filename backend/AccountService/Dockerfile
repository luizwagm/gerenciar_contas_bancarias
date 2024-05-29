FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AccountService.csproj", "./"]
RUN dotnet restore "AccountService.csproj"

COPY . ./AccountService
WORKDIR "/src/AccountService"
RUN dotnet build "AccountService.csproj" -c Release -o /app/build

RUN dotnet publish "AccountService.csproj" -c Release -o /app/publish

# Instalar dotnet-ef
RUN dotnet tool install --global dotnet-ef

# Usa a mesma imagem para build e runtime para garantir que o SDK esteja disponível
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

# Adicionar a ferramenta dotnet-ef ao PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "AccountService.dll"]
