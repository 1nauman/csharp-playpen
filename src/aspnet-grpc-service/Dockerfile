FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/aspnet-grpc-service/aspnet-grpc-service.csproj", "src/aspnet-grpc-service/"]
RUN dotnet restore "src/aspnet-grpc-service/aspnet-grpc-service.csproj"
COPY . .
WORKDIR "/src/src/aspnet-grpc-service"
RUN dotnet build "aspnet-grpc-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "aspnet-grpc-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aspnet-grpc-service.dll"]
