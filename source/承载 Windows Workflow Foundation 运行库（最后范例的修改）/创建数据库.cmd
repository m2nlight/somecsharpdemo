@echo off
echo ���ڴ������ݿ� WorkflowTrackingStore...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -i "WorkflowTrackingStore.sql"
echo.
echo ���ڴ����־ñ�...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -d WorkflowTrackingStore -i "%windir%\Microsoft.Net\Framework\v3.0\Windows Workflow Foundation\SQL\EN\SqlPersistenceService_Schema.sql"
echo.
echo ���ڴ����־ô洢����...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -d WorkflowTrackingStore -i "%windir%\Microsoft.Net\Framework\v3.0\Windows Workflow Foundation\SQL\EN\SqlPersistenceService_Logic.sql"
echo.
echo ���ڴ������ٱ�...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -d WorkflowTrackingStore -i "%windir%\Microsoft.Net\Framework\v3.0\Windows Workflow Foundation\SQL\EN\Tracking_Schema.sql"
echo.
echo ���ڴ������ٴ洢����...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -d WorkflowTrackingStore -i "%windir%\Microsoft.Net\Framework\v3.0\Windows Workflow Foundation\SQL\EN\Tracking_Logic.sql"
echo.
pause