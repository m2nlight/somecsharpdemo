@echo off
if "%1"=="" goto help
csc /t:winexe /r:system.dll,system.xaml.dll,"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\WPF\windowsbase.dll","%WINDIR%\Microsoft.NET\Framework\v4.0.30319\WPF\presentationcore.dll","%WINDIR%\Microsoft.NET\Framework\v4.0.30319\WPF\presentationframework.dll" %1 %2 %3 %4 %5 %6 %7 %8 %9
if errorlevel 1 pause
goto exit
:help
echo ��ʽ��%0 �����ļ� [��������]
echo ���ӣ�%0 test.cs /unsafe
:exit