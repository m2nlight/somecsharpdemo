using System;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;

public class Program
{
	public static void Main()
	{
		var versionInfo=NETVersionChecker.GetLatestDOTNETVersion();
		var text=string.Format("系统中最新的版本：\nFrameworkVersion: {0}\nServeicePack: {1}",versionInfo.FrameworkVersion,versionInfo.ServicePack);
		
		var checkVersionInfo=new NETVersionChecker.DOTNETVersionInfo{FrameworkVersion=3.5, ServicePack=1};
		MessageBoxIcon icon;
		
		if(NETVersionChecker.CheckRequiredDOTNETVersion(checkVersionInfo))
		{
			text+="\n\n检测到系统已经安装 .NET Framework 3.5 SP1";
			icon=MessageBoxIcon.Asterisk;
		}
		else
		{
			text+="\n\n未能检测到 .NET Framework 3.5 SP1";
			icon=MessageBoxIcon.Error;
		}
		
		MessageBox.Show(text,"检查 .NET Framework 3.5 SP1 环境",MessageBoxButtons.OK,icon);
	}
}


/* 摘自：http://www.codeproject.com/Tips/135964/Get-NET-Framework-version.aspx */
public class NETVersionChecker
{
    public struct DOTNETVersionInfo
    {
        public double FrameworkVersion;
        public int ServicePack;
    }
    public static bool CheckRequiredDOTNETVersion(DOTNETVersionInfo required)
    {
        bool reslt = false;
        double tmpFramework = 0;
        int tmpSP = 0;
        try
        {
            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP", false);
            string[] version_names = installed_versions.GetSubKeyNames();
            string tmpBaseVersion;
            //check each installed version
            foreach (string ver in version_names)
            {
                //set default values
                tmpFramework = 0;
                tmpSP = 0;
                tmpBaseVersion = string.Empty;
                try
                {
                    //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                    string tmpFullVersion = ver.Remove(0, 1);
                    //now remove the minor versions 2.0.5725
                    if (tmpFullVersion.Length > 3)
                    {
                        tmpBaseVersion = tmpFullVersion.Remove(tmpFullVersion.IndexOfAny((".").ToCharArray(), 2), tmpFullVersion.Length - 3);
                    }
                    else //its just 3 digit version
                    {
                        tmpBaseVersion = tmpFullVersion;
                    }
                    double basicVersion = 0;
                    if (double.TryParse(tmpBaseVersion, out basicVersion))
                    {
                        tmpFramework = basicVersion;
                    }
                }
                catch
                {
                    tmpFramework = 0;
                }
                //The service pack key might not exist so it might throw an error
                try
                {
                    tmpSP = Convert.ToInt32(installed_versions.OpenSubKey(ver).GetValue("SP", 0));
                }
                catch { }
                if (tmpFramework == required.FrameworkVersion && tmpSP == required.ServicePack)
                {
                    reslt = true;
                    break;
                }
            }
        }
        catch (Exception exp)
        {
            string message = "Error occured:" + exp.Message;
            if (exp is System.Security.SecurityException)
            {
                message += "\n Unable to find .NET Framework version. \n The user does not have the permissions required to access the registry key:\n"
                        + @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP";
            }
            MessageBox.Show(message);
        }
        return reslt;
    }
    public static DOTNETVersionInfo GetLatestDOTNETVersion()
    {
        DOTNETVersionInfo dnVer;
        dnVer.FrameworkVersion = 0;
        dnVer.ServicePack = 0;
        try
        {
            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP", false);
            string[] version_names = installed_versions.GetSubKeyNames();
            //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
            double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
            dnVer.FrameworkVersion = Framework;
            //The service pack key might not exist so it might throw an error
            int SP = 0;
            try
            {
                SP = Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1]).GetValue("SP", 0));
            }
            catch { }
            dnVer.ServicePack = SP;
        }
        catch(Exception exp)
        {
            string message = "Error occured:" + exp.Message;
            if (exp is System.Security.SecurityException)
            {
                message += "\n Unable to find .NET Framework version. \n The user does not have the permissions required to access the registry key:\n"
                        + @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP";
            }
            MessageBox.Show(message);
        }
        return dnVer;
    }
}