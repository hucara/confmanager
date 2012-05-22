using System;
using System.Timers;
using System.Diagnostics;
using System.IO;

namespace Util
{
    public class LogDeletion
    {
        private const int STATIC_LINE_LENGTH = 100;

        private System.Timers.Timer MyTimer = null;
        private String ApplicationMainName = "";
        private String LogFileName = "";
        private int iMaxAgeOfLogFiles = 0;
        private bool bCreateLogFiles = false;
        private LogCreation Log = null;
        private String MyExecutable = Process.GetCurrentProcess().MainModule.FileName;
        private String BasePath = "";

        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This is the constructor
        //
        // Two informations needed in this class:
        //
        // 1. The application main name for creation of the name of the config file
        //    If the main application name is "ReceiptCopyManager" then the config file will be:
        //    "..\config\ReceiptCopyManager.conf"
        // 2. The prefix (abbreviation) of the application name
        //    If the main application name is "ReceiptCopyManager" then the abbrviation will be "RCM"
        //    Then the log files will be named "RCM_Log_YYY_MM_DD.txt"
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public LogDeletion(String ApplicatioName, String ApplicationLogFileName, int MaxAgeOfLogs)
        {
            ApplicationMainName = ApplicatioName;
            LogFileName = ApplicationLogFileName;

            BasePath = MyExecutable.Substring(0, MyExecutable.LastIndexOf('\\'));
            BasePath = MyExecutable.Substring(0, BasePath.LastIndexOf('\\'));
            BasePath += "\\";

            iMaxAgeOfLogFiles = MaxAgeOfLogs;

            SetupTimer();
            ReadConfiguration();

            Log = new LogCreation(LogFileName, STATIC_LINE_LENGTH, '#', bCreateLogFiles);

            CheckForLogFilesToDelete();
        }



        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This will set up a timer
        //
        // The timer will elapse exactly once a day.
        //
        // ###################################################################################################################
        // ###################################################################################################################
        private void SetupTimer()
        {
            MyTimer = new System.Timers.Timer();

            // Add an event handler
            MyTimer.Elapsed += new ElapsedEventHandler(TimeToDeleteLogFilesReached);
            // Set the Interval to 'once a day'
            MyTimer.Interval = 1000 * 60 * 60 * 24;
            // Start the timer
            MyTimer.Enabled = true;
        }


        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method will read out the configuration from the configuration file
        //
        // Two things are important:
        //
        // 1. The maximim age of the logfiles
        // 2. The configuration, if logfiles should be created.
        //
        // ###################################################################################################################
        // ###################################################################################################################
        private void ReadConfiguration()
        {
            //IniFile ConfigurationFile       = new IniFile("");

            //String MaxAgeOfLogFiles         = ConfigurationFile.GetIniFileValue( ApplicationMainName + ".conf", 1, "config", "Debug", "MaxAgeOfLogFiles" );
            //String CreateLogFiles           = ConfigurationFile.GetIniFileValue( ApplicationMainName + ".conf", 1, "config", "Debug", "CreateLogFiles"   );

            //String MaxAgeOfLogFiles = "";
            //String CreateLogFiles = "";

            //int.TryParse( MaxAgeOfLogFiles, out iMaxAgeOfLogFiles );

            //if( String.Equals( CreateLogFiles, "yes", System.StringComparison.CurrentCultureIgnoreCase ) ) 
            //    bCreateLogFiles = true;
            //else
            //    bCreateLogFiles = false;

            bCreateLogFiles = Configuration_Manager.Model.getInstance().createLogs;
        }

        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This is the event handler of the timer
        // If the timer has elapsed, we will check the batch of meanwhile created log files for their age
        // and for the need to delete logs that are older than the configured number of days
        //
        // ###################################################################################################################
        // ###################################################################################################################
        private void TimeToDeleteLogFilesReached(object source, ElapsedEventArgs e)
        {
            CheckForLogFilesToDelete();
        }



        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method reads the ages of all logfiles and will delete those, whose age is older than the configured
        // maximum age 
        //
        // This methode will be called
        //
        // 1. On creation of the class
        // 2. On the timeout of the timer (once a day)
        //
        // ###################################################################################################################
        // ###################################################################################################################
        private void CheckForLogFilesToDelete()
        {
            String FileTemplate = LogFileName + "*.txt";
            DirectoryInfo di = new DirectoryInfo(BasePath + "log");
            FileInfo[] rgFiles = di.GetFiles(FileTemplate);
            String LogMessage = "";

            Log.Append(" ");
            Log.AppendDividingLine();
            Log.AppendWithFrame(" ");
            Log.AppendWithFrame("Time reached for automatic log deletion...");
            LogMessage = String.Format("Max age of log files is configured to {0} days", iMaxAgeOfLogFiles);
            Log.AppendWithFrame(LogMessage);

            int iIterator = 0;

            foreach (FileInfo fi in rgFiles)
            {
                String ThisFileName = rgFiles[iIterator].ToString();
                String FileNameLocation = rgFiles[iIterator].DirectoryName + "\\" + ThisFileName;
                DateTime Today = DateTime.Now;
                DateTime DDay = Today.AddDays(-iMaxAgeOfLogFiles);

                if (ThisFileName.Length > 0)
                {
                    DateTime LastWiteTime = rgFiles[iIterator].LastWriteTime;

                    if (LastWiteTime.CompareTo(DDay) < 0)
                    {
                        // We have to delete the file

                        try
                        {
                            System.IO.File.Delete(FileNameLocation);

                            LogMessage = "File deleted: ";
                            LogMessage += ThisFileName;

                            TimeSpan DiffTime = Today.Subtract(LastWiteTime);
                            LogMessage += "   [" + DiffTime.Days + " days] old";
                            Log.AppendWithFrame(LogMessage);
                        }
                        catch
                        {
                            LogMessage = "Unable to delete file: ";
                            LogMessage += FileNameLocation;

                            Log.AppendWithFrame(LogMessage);
                        }
                    }
                }

                iIterator++;
            }

            Log.AppendWithFrame("Ready with log file deletion process...");
            Log.AppendWithFrame(" ");
            Log.AppendDividingLine();
            Log.Append(" ");
        }
    }
}
