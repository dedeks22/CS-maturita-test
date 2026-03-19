FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "CS-maturita-test/CS-maturita-test/CS-maturita-test.csproj"
RUN dotnet publish "CS-maturita-test/CS-maturita-test/CS-maturita-test.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "CS-maturita-test.dll"]