using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Runtime.Tracking;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Globalization;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using System.Workflow.Runtime.Configuration;
using System.Configuration;

namespace Microsoft.Samples.Workflow.Tutorials.Hosting
{
    class Program
    {
        static AutoResetEvent waitHandle = new AutoResetEvent(false);
        static Dictionary<string, object> parameters = new Dictionary<string, object>();
        public static string connectionString;
        static Guid wfinstanceId;

        static void Main(string[] args)
        {
            //从配置文件的工作流节中得到数据库连接字串
            WorkflowRuntimeSection config;
            config = ConfigurationManager.GetSection("HostingWorkflowRuntime") as WorkflowRuntimeSection;
            connectionString = config.CommonParameters["ConnectionString"].Value;

            CreateTrackingProfile();

            WorkflowRuntime workflowRuntime = new WorkflowRuntime("HostingWorkflowRuntime");

            // workflow runtime events
            workflowRuntime.Started += new EventHandler<WorkflowRuntimeEventArgs>(workflowRuntime_Started);
            workflowRuntime.Stopped += new EventHandler<WorkflowRuntimeEventArgs>(workflowRuntime_Stopped);
            workflowRuntime.WorkflowSuspended += new EventHandler<WorkflowSuspendedEventArgs>(workflowRuntime_WorkflowSuspended);
            workflowRuntime.WorkflowResumed += new EventHandler<WorkflowEventArgs>(workflowRuntime_WorkflowResumed);
            workflowRuntime.WorkflowTerminated += new EventHandler<WorkflowTerminatedEventArgs>(workflowRuntime_WorkflowTerminated);

            workflowRuntime.WorkflowLoaded += new EventHandler<WorkflowEventArgs>(workflowRuntime_WorkflowLoaded);
            workflowRuntime.WorkflowIdled += new EventHandler<WorkflowEventArgs>(workflowRuntime_WorkflowIdled);
            workflowRuntime.WorkflowPersisted += new EventHandler<WorkflowEventArgs>(workflowRuntime_WorkflowPersisted);
            workflowRuntime.WorkflowUnloaded += new EventHandler<WorkflowEventArgs>(workflowRuntime_WorkflowUnloaded);

            workflowRuntime.WorkflowCompleted += new EventHandler<WorkflowCompletedEventArgs>(workflowRuntime_WorkflowCompleted);

            workflowRuntime.StartRuntime();

            // retrieve workflow parameters
            do
            {
                try
                {
                    Console.Write("Enter a value for operand1: ");
                    parameters["Operand1"] = Int32.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + Environment.NewLine);
                }
            } while (parameters.ContainsKey("Operand1") == false);

            do
            {
                try
                {
                    Console.Write("Enter a value for operand2: ");
                    parameters["Operand2"] = Int32.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + Environment.NewLine);
                }
            } while (parameters.ContainsKey("Operand2") == false);

            Type type = typeof(HostingWorkflows);
            WorkflowInstance workflowInstance = workflowRuntime.CreateWorkflow(type, parameters);
            wfinstanceId = workflowInstance.InstanceId;
            workflowInstance.Start();

            //workflowInstance.Suspend("Suspending workflow");
            //workflowInstance.Terminate("Terminating workflow");

            waitHandle.WaitOne();
            workflowRuntime.StopRuntime();

            GetInstanceTrackingEvents(workflowInstance.InstanceId);
            GetActivityTrackingEvents(workflowInstance.InstanceId);

            Console.Write("完成。按回车..."); Console.ReadLine();
        }

        static void workflowRuntime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
            Console.WriteLine("Result: {0}", e.OutputParameters["Result"]);
            waitHandle.Set();
        }

        static void workflowRuntime_Started(object sender, WorkflowRuntimeEventArgs e)
        {
            Console.WriteLine("Workflow runtime started");
        }

        static void workflowRuntime_Stopped(object sender, WorkflowRuntimeEventArgs e)
        {
            Console.WriteLine("Workflow runtime stopped");
        }

        static void workflowRuntime_WorkflowSuspended(object sender, WorkflowSuspendedEventArgs e)
        {
            Console.WriteLine("Workflow suspended: {0}", e.Error);
            e.WorkflowInstance.Resume();
        }

        static void workflowRuntime_WorkflowResumed(object sender, WorkflowEventArgs e)
        {
            Console.WriteLine("Workflow resumed");
        }

        static void workflowRuntime_WorkflowTerminated(object sender, WorkflowTerminatedEventArgs e)
        {
            Console.WriteLine("Workflow Terminated : {0}", e.Exception.Message);
            waitHandle.Set();
        }

        static void workflowRuntime_WorkflowLoaded(object sender, WorkflowEventArgs e)
        {
            Console.WriteLine("Workflow {0} loaded", e.WorkflowInstance.InstanceId);
        }

        static void workflowRuntime_WorkflowIdled(object sender, WorkflowEventArgs e)
        {
            Console.WriteLine("Workflow {0} idled", e.WorkflowInstance.InstanceId);
            e.WorkflowInstance.Unload();
        }

        static void workflowRuntime_WorkflowPersisted(object sender, WorkflowEventArgs e)
        {
            Console.WriteLine("Workflow {0} persisted", e.WorkflowInstance.InstanceId);
        }

        static void workflowRuntime_WorkflowUnloaded(object sender, WorkflowEventArgs e)
        {
            Console.WriteLine("Workflow {0} unloaded", e.WorkflowInstance.InstanceId);
            BinInfoToFile();
        }

        private static void BinInfoToFile()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = string.Format("select top 1 state from InstanceState where uidInstanceID='{0}'", wfinstanceId);
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    string filename = string.Format("{0}{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"\data__$001.bin");
                    BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate));
                    System.Data.SqlTypes.SqlBinary binary = dr.GetSqlBinary(0);
                    byte[] bytes = binary.Value;
                    bw.Write(bytes);
                    bw.Flush();
                    ConsoleColor fcc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("已输出二进制数据文件: {0}",filename);
                    Console.ForegroundColor = fcc;
                }
                catch (Exception ex)
                {
                    ConsoleColor fcc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("错误: {0}",ex.Message);
                    Console.ForegroundColor = fcc;
                }
                finally
                {
                    conn.Close();
                }
                
                
            }
        }

        static void UnloadInstance(object sender, WorkflowEventArgs e)
        {
            Console.WriteLine("Workflow {0} unloaded", e.WorkflowInstance.InstanceId);
            e.WorkflowInstance.Unload();
        }

        static void GetInstanceTrackingEvents(Guid instanceId)
        {
            SqlTrackingQuery sqlTrackingQuery = new SqlTrackingQuery(connectionString);

            SqlTrackingWorkflowInstance sqlTrackingWorkflowInstance;
            if (!sqlTrackingQuery.TryGetWorkflow(instanceId, out sqlTrackingWorkflowInstance))
            {
                Console.WriteLine("Could not retrieve SqlTrackingWorkflowInstance");
            }
            else
            {
                Console.WriteLine("\nInstance Level Events:\n");

                foreach (WorkflowTrackingRecord workflowTrackingRecord in sqlTrackingWorkflowInstance.WorkflowEvents)
                {
                    Console.WriteLine("EventDescription : {0}  DateTime : {1}",
                        workflowTrackingRecord.TrackingWorkflowEvent,
                        workflowTrackingRecord.EventDateTime);
                }
            }
        }

        static void GetActivityTrackingEvents(Guid instanceId)
        {
            SqlTrackingQuery sqlTrackingQuery = new SqlTrackingQuery(connectionString);

            SqlTrackingWorkflowInstance sqlTrackingWorkflowInstance;
            if (!sqlTrackingQuery.TryGetWorkflow(instanceId, out sqlTrackingWorkflowInstance))
            {
                Console.WriteLine("Could not retrieve SqlTrackingWorkflowInstance");
            }
            else
            {
                Console.WriteLine("\nActivity Tracking Events:\n");

                foreach (ActivityTrackingRecord activityTrackingRecord in
                    sqlTrackingWorkflowInstance.ActivityEvents)
                {
                    Console.WriteLine(
                        "StatusDescription: {0}  DateTime: {1} Activity Qualified ID: {2}",
                        activityTrackingRecord.ExecutionStatus,
                        activityTrackingRecord.EventDateTime,
                        activityTrackingRecord.QualifiedName);
                }
            }
        }

        static void CreateTrackingProfile()
        {
            TrackingProfile profile = new TrackingProfile();
            ActivityTrackPoint trackPoint = new ActivityTrackPoint();
            profile.Version = new Version("1.0.0.0");

            // track CodeActivity activities only
            ActivityTrackingLocation location = new ActivityTrackingLocation(typeof(CodeActivity));

            // add all activity tracking events
            foreach (ActivityExecutionStatus s in Enum.GetValues(typeof(ActivityExecutionStatus)))
            {
                location.ExecutionStatusEvents.Add(s);
            }

            trackPoint.MatchingLocations.Add(location);
            profile.ActivityTrackPoints.Add(trackPoint);

            WorkflowTrackPoint wtp = new WorkflowTrackPoint();
            WorkflowTrackingLocation wtl = new WorkflowTrackingLocation();

            // add all workflow tracking events
            foreach (TrackingWorkflowEvent s in Enum.GetValues(typeof(TrackingWorkflowEvent)))
            {
                wtl.Events.Add(s);
            }

            wtp.MatchingLocation = wtl;
            profile.WorkflowTrackPoints.Add(wtp);

            // serialize tracking profile and save to SQL
            TrackingProfileSerializer serializer = new TrackingProfileSerializer();
            StringWriter writer = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture);

            serializer.Serialize(writer, profile);

            InsertTrackingProfile(writer.ToString());
        }

        static void InsertTrackingProfile(string profile)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.UpdateTrackingProfile";
            cmd.Connection = new SqlConnection(Program.connectionString);

            try
            {
                SqlParameter typFullName = new SqlParameter();
                typFullName.ParameterName = "@TypeFullName";
                typFullName.SqlDbType = SqlDbType.NVarChar;
                typFullName.SqlValue = typeof(HostingWorkflows).ToString();
                cmd.Parameters.Add(typFullName);

                SqlParameter assemblyFullName = new SqlParameter();
                assemblyFullName.ParameterName = "@AssemblyFullName";
                assemblyFullName.SqlDbType = SqlDbType.NVarChar;
                assemblyFullName.SqlValue = typeof(HostingWorkflows).Assembly.FullName;
                cmd.Parameters.Add(assemblyFullName);

                SqlParameter versionId = new SqlParameter();
                versionId.ParameterName = "@Version";
                versionId.SqlDbType = SqlDbType.VarChar;
                versionId.SqlValue = "1.0.0.0";
                cmd.Parameters.Add(versionId);

                SqlParameter trackingProfile = new SqlParameter();
                trackingProfile.ParameterName = "@TrackingProfileXml";
                trackingProfile.SqlDbType = SqlDbType.NVarChar;
                trackingProfile.SqlValue = profile;
                cmd.Parameters.Add(trackingProfile);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("The Tracking Profile was not inserted. " +
                    "if you want to add a new Tracking Profile, modify the version " +
                    "string in the profile by specifying a higher version number.");
                return;
            }
            finally
            {
                if ((null != cmd) && (null != cmd.Connection) &&
                    (ConnectionState.Closed != cmd.Connection.State))
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                }
            }
        }
    }
}

/* 运行结果：
Workflow runtime started
Enter a value for operand1: 123
Enter a value for operand2: 45
In codeActivity1_ExecuteCode. Adding operands
Workflow 9121c788-fc22-4234-98df-62bfa111b947 idled
Workflow 9121c788-fc22-4234-98df-62bfa111b947 persisted
Workflow 9121c788-fc22-4234-98df-62bfa111b947 unloaded
已输出二进制数据文件: C:\Documents and Settings\bt36\桌面\data__$001.bin
Workflow 9121c788-fc22-4234-98df-62bfa111b947 loaded
In codeActivity2_ExecuteCode
Workflow 9121c788-fc22-4234-98df-62bfa111b947 persisted
Result: 168
Workflow runtime stopped

Instance Level Events:

EventDescription : Created  DateTime : 2008-11-6 4:41:52
EventDescription : Started  DateTime : 2008-11-6 4:41:52
EventDescription : Idle  DateTime : 2008-11-6 4:41:52
EventDescription : Unloaded  DateTime : 2008-11-6 4:41:52
EventDescription : Persisted  DateTime : 2008-11-6 4:41:52
EventDescription : Loaded  DateTime : 2008-11-6 4:42:00
EventDescription : Completed  DateTime : 2008-11-6 4:42:00
EventDescription : Persisted  DateTime : 2008-11-6 4:42:00

Activity Tracking Events:

StatusDescription: Executing  DateTime: 2008-11-6 4:41:52 Activity Qualified ID:
 codeActivity1
StatusDescription: Closed  DateTime: 2008-11-6 4:41:52 Activity Qualified ID: co
deActivity1
StatusDescription: Executing  DateTime: 2008-11-6 4:42:00 Activity Qualified ID:
 codeActivity2
StatusDescription: Closed  DateTime: 2008-11-6 4:42:00 Activity Qualified ID: co
deActivity2
完成。按回车...
*/