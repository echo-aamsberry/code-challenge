FROM echogl-microsoft-docker-external-remote.jfrog.io/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY src src
COPY test test
COPY Arbitr.sln .
RUN dotnet build Arbitr.sln -c Debug

FROM build AS publish
WORKDIR /app/src/Arbitr.Worker.ClutchTranslator
RUN dotnet publish --no-restore -c Release -o /app

FROM echogl-microsoft-docker-external-remote.jfrog.io/dotnet/aspnet:3.1 AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Arbitr.Worker.ClutchTranslator.dll"]