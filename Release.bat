@ECHO off

DEL /q build
dotnet clean source -c Release

dotnet test source -c Release -r build -l trx

IF %ERRORLEVEL% GTR 0 GOTO :end

dotnet pack source -c Release -o build /p:LOCAL_VERSION=true

:end