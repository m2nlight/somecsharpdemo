using System;  
using System.Security.Permissions;  
using System.Windows.Threading;  
  
public class DispatcherHelper  
{  
    /// <summary>  
    /// Simulate Application.DoEvents function of <see cref=" System.Windows.Forms.Application"/> class.  
    /// </summary>  
    [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]  
    public static void DoEvents()  
    {  
        var frame = new DispatcherFrame();  
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,  
            new DispatcherOperationCallback(ExitFrames), frame);  
  
        try  
        {  
            Dispatcher.PushFrame(frame);  
        }  
        catch (InvalidOperationException)  
        {  
        }  
    }  
  
    private static object ExitFrames(object frame)  
    {  
        ((DispatcherFrame)frame).Continue = false;  
        return null;  
    }  
}  