%windir%\microsoft.net\framework\v3.5\msbuild /t:clean ImageAndBase64.sln
@IF %ERRORLEVEL% NEQ 0 PAUSE