%windir%\microsoft.net\framework\v3.5\msbuild /property:Configuration=Release RegexMatch.csproj
@IF %ERRORLEVEL% NEQ 0 PAUSE