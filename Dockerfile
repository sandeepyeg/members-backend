FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["src/EnterpriseMembers.Api/EnterpriseMembers.Api.csproj", "src/EnterpriseMembers.Api/"]
COPY ["src/EnterpriseMembers.Application/EnterpriseMembers.Application.csproj", "src/EnterpriseMembers.Application/"]
COPY ["src/EnterpriseMembers.Infrastructure/EnterpriseMembers.Infrastructure.csproj", "src/EnterpriseMembers.Infrastructure/"]


RUN dotnet restore "src/EnterpriseMembers.Api/EnterpriseMembers.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/EnterpriseMembers.Api"
RUN dotnet build "EnterpriseMembers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnterpriseMembers.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnterpriseMembers.Api.dll"]
