FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./Beauty4u.Jobs ./Beauty4u.Jobs
RUN dotnet restore ./Beauty4u.Jobs
RUN dotnet publish ./Beauty4u.Jobs -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV DOTNET_ENVIRONMENT=Production
ENV ConnectionStrings__B4uConnection=""
ENV Cors__AllowedOrigins=""
ENTRYPOINT ["dotnet", "Beauty4u.Jobs.dll"]
