@echo off
echo 正在创建数据库 WorkflowTrackingStore...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -i "WorkflowTrackingStore.sql"
echo.
echo 正在创建持久表...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -d WorkflowTrackingStore -i "%windir%\Microsoft.Net\Framework\v3.0\Windows Workflow Foundation\SQL\EN\SqlPersistenceService_Schema.sql"
echo.
echo 正在创建持久存储过程...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -d WorkflowTrackingStore -i "%windir%\Microsoft.Net\Framework\v3.0\Windows Workflow Foundation\SQL\EN\SqlPersistenceService_Logic.sql"
echo.
echo 正在创建跟踪表...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -d WorkflowTrackingStore -i "%windir%\Microsoft.Net\Framework\v3.0\Windows Workflow Foundation\SQL\EN\Tracking_Schema.sql"
echo.
echo 正在创建跟踪存储过程...
sqlcmd -S %COMPUTERNAME%\SQLExpress -E -d WorkflowTrackingStore -i "%windir%\Microsoft.Net\Framework\v3.0\Windows Workflow Foundation\SQL\EN\Tracking_Logic.sql"
echo.
pause