%windir%\microsoft.net\framework\v3.5\msbuild /property:Configuration=Release ListViewPictures.sln
@IF %ERRORLEVEL% NEQ 0 PAUSE