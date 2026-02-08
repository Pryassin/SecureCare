# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the solution file and project files first to leverage Docker cache for restore
# We need to copy distinct project files because they are in different folders
COPY ["src/backend/CMS.Api/CMS.Api.csproj", "src/backend/CMS.Api/"]
COPY ["src/backend/CMS.Application/CMS.Application.csproj", "src/backend/CMS.Application/"]
COPY ["src/backend/CMS.Domain/CMS.Domain.csproj", "src/backend/CMS.Domain/"]
COPY ["src/backend/CMS.Infrastructure/CMS.Infrastructure.csproj", "src/backend/CMS.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/backend/CMS.Api/CMS.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/src/backend/CMS.Api"
RUN dotnet build "CMS.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "CMS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# innovative runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMS.Api.dll"]
