FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5173

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["permission-service.csproj", "."]
RUN dotnet restore "./permission-service.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "permission-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "permission-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "permission-service.dll", "--urls", "http://*:5173"]