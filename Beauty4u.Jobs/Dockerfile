# -------------------------
# Stage 1: Build
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy the solution file
COPY New_Beauty4U.sln ./

# Copy project files for restore
COPY BusinessLayer/Beauty4u.Business/Beauty4u.Business.csproj BusinessLayer/Beauty4u.Business/
COPY BusinessLayer/Beauty4u.Common/Beauty4u.Common.csproj BusinessLayer/Beauty4u.Common/

COPY DataLayer/Beauty4u.ApiAccess/Beauty4u.ApiAccess.csproj DataLayer/Beauty4u.ApiAccess/
COPY DataLayer/Beauty4u.Data/Beauty4u.Data.csproj DataLayer/Beauty4u.Data/
COPY DataLayer/Beauty4u.DataAccess/Beauty4u.DataAccess.csproj DataLayer/Beauty4u.DataAccess/

COPY Models/Beauty4u.Interfaces/Beauty4u.Interfaces.csproj Models/Beauty4u.Interfaces/
COPY Models/Beauty4u.Models/Beauty4u.Models.csproj Models/Beauty4u.Models/

COPY Web/Beauty4u.Jobs/Beauty4u.Jobs.csproj Web/Beauty4u.Jobs/
COPY Web/Beauty4u.WebApi/Beauty4u.WebApi.csproj Web/Beauty4u.WebApi/

# Restore all NuGet packages
RUN dotnet restore New_Beauty4U.sln

# Copy all source code
COPY . .

# Build and publish the Jobs project
RUN dotnet publish Web/Beauty4u.Jobs/Beauty4u.Jobs.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# -------------------------
# Stage 2: Runtime
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Expose API port
EXPOSE 5001

# Run the API
ENTRYPOINT ["dotnet", "Beauty4u.Jobs.dll"]
