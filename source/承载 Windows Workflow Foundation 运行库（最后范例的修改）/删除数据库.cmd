@echo off
echo 正在删除数据库 WorkflowTrackingStore...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -i "DeleteWorkflowTrackingStore.sql"
echo.
pause