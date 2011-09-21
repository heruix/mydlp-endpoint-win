﻿//    Copyright (C) 2011 Huseyin Ozgur Batur <ozgur@medra.com.tr>
//
//--------------------------------------------------------------------------
//    This file is part of MyDLP.
//
//    MyDLP is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    MyDLP is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with MyDLP.  If not, see <http://www.gnu.org/licenses/>.
//--------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace MyDLP.EndPoint.Core
{
    public class Configuration
    {
        static String appPath;
        static String seapServer;
        static int seapPort;
        static Logger.LogLevel logLevel;
        static String minifilterPath;
        static String pyBackendPath;
        static String erlangPath;
        static String pythonBinPaths;
        static String erlangBinPaths;
        static String pythonPath;

        public static String AppPath
        {
            get
            {
                return appPath;
            }
        }

        public static String SeapServer
        {
            get
            {
                return seapServer;
            }
        }

        public static int SeapPort
        {
            get
            {
                return seapPort;
            }
        }

        public static Logger.LogLevel LogLevel
        {
            get
            {
                return logLevel;
            }
        }

        public static String MinifilterPath
        {
            get
            {
                return minifilterPath;
            }
        }

        public static String PyBackendPath
        {
            get
            {
                return pyBackendPath;
            }
        }

        public static String ErlangPath
        {
            get
            {
                return erlangPath;
            }
        }

        public static String ErlangBinPaths
        {
            get
            {
                return erlangBinPaths;
            }
        }

        public static String PythonBinPaths
        {
            get
            {
                return pythonBinPaths;
            }
        }

        public static String PythonPath
        {
            get
            {
                return pythonPath;
            }
        }

        public static bool GetRegistryConf()
        {
            if (System.Environment.UserInteractive)
            {
                //Use development conf
                minifilterPath = "C:\\workspace\\mydlp-endpoint-win\\EndPoint\\MiniFilter\\src\\objchk_wxp_x86\\i386\\MyDLPMF.sys";
                pyBackendPath = @"C:\workspace\mydlp-endpoint-win\EndPoint\Engine\mydlp\src\backend\py\";
                erlangPath = @"C:\workspace\mydlp-endpoint-win\EndPoint\Engine\mydlp\src\mydlp\";
                erlangBinPaths = @"C:\workspace\mydlp-deployment-env\erl5.7.4\bin;C:\workspace\mydlp-deployment-env\erts-5.7.4\bin";
                pythonBinPaths = @"C:\workspace\mydlp-deployment-env\Python26";
                pythonPath = @"C:\workspace\mydlp-endpoint-win\EndPoint\Engine\mydlp\src\thrift\gen-py";
                appPath = @"C:\workspace\mydlp-endpoint-win\EndPoint\Engine\mydlp\src\mydlp\";
                return true;
            }
            else
            {
                //Use normal conf
                try
                {
                    RegistryKey mydlpKey = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("MyDLP");

                    //Get path
                    try
                    {
                        appPath = mydlpKey.GetValue("AppPath").ToString();
                        minifilterPath = appPath + "MyDLPMF.sys";
                        pyBackendPath = appPath + "engine\\py\\";
                        erlangPath = appPath + "engine\\erl\\";
                        erlangBinPaths = appPath + @"erl5.7.4\bin;" + appPath + @"erl5.7.4\erts-5.7.4\bin";
                        pythonPath = appPath + "engine\\py\\";
                        pythonBinPaths = appPath + "Python26";

                    }
                    catch (Exception e)
                    {
                        Logger.GetInstance().Error("Unable to get registry value  HKLM/Software/MyDLP:AppPath "
                            + e.Message + " " + e.StackTrace);
                        return false;
                    }

                    //Get loglevel
                    try
                    {
                        logLevel = (Logger.LogLevel)mydlpKey.GetValue("LogLevel");
                        if (logLevel > Logger.LogLevel.DEBUG) logLevel = Logger.LogLevel.DEBUG;
                    }
                    catch (Exception e)
                    {
                        Logger.GetInstance().Error("Unable to get registry value  HKLM/Software/MyDLP:LogLevel "
                             + e.Message + " " + e.StackTrace);
                        return false;
                    }

                    //Get seapServer
                    try
                    {
                        seapServer = mydlpKey.GetValue("SeapServer").ToString();
                    }
                    catch (Exception e)
                    {
                        Logger.GetInstance().Error("Unable to get registry value  HKLM/Software/MyDLP:SeapServer "
                             + e.Message + " " + e.StackTrace);
                        return false;
                    }

                    //Get seapPort
                    try
                    {
                        seapPort = (int)mydlpKey.GetValue("SeapPort");
                    }
                    catch (Exception e)
                    {
                        Logger.GetInstance().Error("Unable to get registry value  HKLM/Software/MyDLP:SeapPort "
                             + e.Message + " " + e.StackTrace);
                        return false;
                    }

                    Logger.GetInstance().Info("MyDLP Path: " + appPath);
                    Logger.GetInstance().Info("MyDLP LogLevel: " + logLevel.ToString());
                    Logger.GetInstance().Info("MyDLP SeapServer: " + seapServer + ":" + seapPort);

                    return true;
                }
                catch (Exception e)
                {
                    Logger.GetInstance().Error("Unable to open registry key HKLM/Software/MyDLP "
                        + e.Message + " " + e.StackTrace);
                    return false;
                }
            }
        }
    }
}
