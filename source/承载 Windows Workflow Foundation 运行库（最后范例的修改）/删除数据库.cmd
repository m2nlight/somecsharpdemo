@echo off
echo ����ɾ�����ݿ� WorkflowTrackingStore...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -i "DeleteWorkflowTrackingStore.sql"
echo.
pause