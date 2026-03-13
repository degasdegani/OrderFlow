FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/OrderFlow.API/OrderFlow.API.csproj", "src/OrderFlow.API/"]
COPY ["src/OrderFlow.Application/OrderFlow.Application.csproj", "src/OrderFlow.Application/"]
COPY ["src/OrderFlow.Domain/OrderFlow.Domain.csproj", "src/OrderFlow.Domain/"]
COPY ["src/OrderFlow.Infrastructure/OrderFlow.Infrastructure.csproj", "src/OrderFlow.Infrastructure/"]
RUN dotnet restore "src/OrderFlow.API/OrderFlow.API.csproj"
COPY . .
WORKDIR "/src/src/OrderFlow.API"
RUN dotnet build "OrderFlow.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderFlow.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderFlow.API.dll"]