Use Master
Go
IF EXISTS (SELECT * 
	   FROM   master..sysdatabases 
	   WHERE  name = N'WorkflowTrackingStore')
	DROP DATABASE WorkflowTrackingStore
GO
CREATE DATABASE WorkflowTrackingStore
GO
