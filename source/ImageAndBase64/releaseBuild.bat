%windir%\microsoft.net\framework\v3.5\msbuild /property:Configuration=Release ImageAndBase64.sln
@IF %ERRORLEVEL% NEQ 0 PAUSE