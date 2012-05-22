using System;
using System.IO;
using System.Diagnostics;


namespace Util
{
    public class LogCreation
    {
        private bool bCreateLogFiles = false;
        private String LogFileName = "";
        private int iStaticLineLength = 0;
        private char FrameCharacter = ' ';
        private String MyExecutable = Process.GetCurrentProcess().MainModule.FileName;
        private String LogFilePath = "";


        // ###################################################################################################################
        // ###################################################################################################################
        //
        // Constructor
        // 
        // Three things as parameters
        //
        // 1. The prefix of the logfile that is nothing but the abbreviation of the application name
        //    If the main application name is "ReceiptCopyManager" then the abbrviation will be "RCM"
        //    Then the log files will be named "RCM_Log_YYY_MM_DD.txt"
        // 2. The character(s) for the frame. This is only interesting for the case that each log line
        //    should have a static length and should be "framed" with a certain character.
        //    Example:
        //    "10:24:57.838 : # This is an example log line                       #"
        //    "10:24:57.915 : # All lines have the same length...                 #"
        //    "10:24:57.938 : # ...regardless how long the message itself will be #"
        //    These loglines can be generated with the help of the public method "AppendWithFrame"
        // 3. A bool variable defining, if logs should be generated at all.
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public LogCreation(String LogFileBaseName, int iLineLength, char FrameChar, bool bCreate)
        {
            bCreateLogFiles = bCreate;
            LogFileName = LogFileBaseName;
            iStaticLineLength = iLineLength;
            FrameCharacter = FrameChar;

            LogFilePath = MyExecutable.Substring(0, MyExecutable.LastIndexOf('\\'));
            LogFilePath = MyExecutable.Substring(0, LogFilePath.LastIndexOf('\\'));
            LogFilePath += "\\log";

            // Check whether a 'log' directory exists and ...
            if (System.IO.Directory.Exists(LogFilePath) == false)
            {
                // ...create it if not
                System.IO.Directory.CreateDirectory(LogFilePath);
            }
        }


        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This is the main method for writing lines to the log file with a constant line length
        // 
        // The log file line can have a static length with a frame string at the beginning and the end of the line
        // Each line starts with a time stamp
        //
        // Example for a log lines:
        //
        // "10:24:57.838 : # This is an example log line                       #"
        // "10:24:57.915 : # All lines have the same length...                 #"
        // "10:24:57.938 : # ...regardless how long the message itself will be #"
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendWithFrame(String Message)
        {
            AppendWithFrame(FrameCharacter, iStaticLineLength, Message);
        }

        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method is like the method "AppendWithFrame" but with the possibility to define an own separation character
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendWithFrame(char FrameCharacter, String Message)
        {
            AppendWithFrame(FrameCharacter, iStaticLineLength, Message);
        }

        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method is like the method "AppendWithFrame" but with the possibility to define an own separation character
        // and with the possibility to define the total length of the line
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendWithFrame(char FrameCharacter, int iStaticLineLength, String Message)
        {
            String CompleteMessage = "";

            if (bCreateLogFiles)
            {
                CompleteMessage = FrameCharacter.ToString();
                CompleteMessage += " ";
                CompleteMessage += Message;
                CompleteMessage = CompleteMessage.PadRight(iStaticLineLength, ' ');
                CompleteMessage += FrameCharacter.ToString();

                Write(CompleteMessage);
            }

        }


        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method is like the method "AppendWithFrame" but the given string will be centered
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendCenteredWithFrame(String Message)
        {
            AppendCenteredWithFrame(FrameCharacter, iStaticLineLength, Message);
        }

        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method is like the method "AppendWithFrame" but the given string will be centered
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendCenteredWithFrame(char FrameCharacter, String Message)
        {
            AppendCenteredWithFrame(FrameCharacter, iStaticLineLength, Message);
        }



        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method is like the method "AppendWithFrame" but the given string will be centered
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendCenteredWithFrame(char FrameCharacter, int iStaticLineLength, String Message)
        {
            String CompleteMessage = "";
            int iNumberOfSpacesLeft = 0;
            int iNumberOfSpacesRight = 0;

            if (bCreateLogFiles)
            {
                iNumberOfSpacesLeft = (iStaticLineLength - 2 - Message.Length) / 2;
                iNumberOfSpacesRight = iNumberOfSpacesLeft;

                if (((iStaticLineLength - Message.Length) % 2) > 0)
                    iNumberOfSpacesRight++;


                CompleteMessage = FrameCharacter.ToString();
                CompleteMessage += " ";
                CompleteMessage += "".PadRight(iNumberOfSpacesLeft, ' ');
                CompleteMessage += Message;

                CompleteMessage += "".PadRight(iNumberOfSpacesRight, ' ');

                //CompleteMessage                 = CompleteMessage.PadRight( iNumberOfSpacesRight, ' ' );
                CompleteMessage += FrameCharacter.ToString();

                Write(CompleteMessage);
            }

        }




        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This writes a dividing line to the log with the pre-defined character 
        // 
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendDividingLine()
        {
            AppendDividingLine(FrameCharacter, iStaticLineLength);
        }





        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This is like AppendDividingLine() but with a free-defined dividing character
        // 
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendDividingLine(char FrameCharacter)
        {
            AppendDividingLine(FrameCharacter, iStaticLineLength);
        }





        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This is like AppendDividingLine( char ) but with a free-defined length
        // 
        // ###################################################################################################################
        // ###################################################################################################################
        public void AppendDividingLine(char FrameCharacter, int iStaticLineLength)
        {
            String DividingLine = "";

            if (bCreateLogFiles)
            {
                DividingLine = DividingLine.PadRight(iStaticLineLength + 1, FrameCharacter);

                Write(DividingLine);
            }

        }



        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This is the main method for writing lines to the log file 
        // 
        // ###################################################################################################################
        // ###################################################################################################################
        public void Append(String Message)
        {
            if (bCreateLogFiles)
            {
                Write(Message);
            }
        }




        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This is the main method for writing lines to the log file
        // 
        // ###################################################################################################################
        // ###################################################################################################################
        private void Write(String Message)
        {
            if (bCreateLogFiles)
            {
                DateTime Today = DateTime.Now;
                String TodayLogFileName = GetLogFileName();
                String CompleteMessage = "";

                CompleteMessage = GetTimeStamp();
                CompleteMessage += Message;
                CompleteMessage += "\r\n";

                StreamWriter myFile = new StreamWriter(TodayLogFileName, true);
                myFile.Write(CompleteMessage);
                myFile.Close();
            }
        }



        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method generates the time stamp that is written at the begin of each log line
        // 
        // The time stamp has the following structure:
        //
        // "HH:MM:SS.XXX : "
        // 
        // Where
        // HH:      The current hours
        // MM:      The current minutes
        // SS:      The current seconds
        // XXX:     The current milli-seconds
        // 
        // ###################################################################################################################
        // ###################################################################################################################
        private String GetTimeStamp()
        {
            String TimeStampString;

            DateTime Today = DateTime.Now;

            TimeStampString = Today.Hour.ToString().PadLeft(2, '0') + ":";
            TimeStampString += Today.Minute.ToString().PadLeft(2, '0') + ":";
            TimeStampString += Today.Second.ToString().PadLeft(2, '0') + ".";
            TimeStampString += Today.Millisecond.ToString().PadLeft(3, '0') + " : ";

            return TimeStampString;
        }




        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method generates the log file name
        // 
        // The log file name has the following structure:
        //
        // AAA_Log_YYYY_MM_DD.txt
        // 
        // Where
        // AAA:     The Header name, e.g. "RCM" for "Receipt Copy Manager"
        // YYYY:    The current year
        // MM:      The current months
        // DD:      The current day
        // 
        // ###################################################################################################################
        // ###################################################################################################################
        private String GetLogFileName()
        {
            String TodayLogFileName = LogFilePath + "\\" + LogFileName;
            DateTime Today = DateTime.Now;

            TodayLogFileName += "_Log_" + Today.Year + "_";
            TodayLogFileName += Today.Month.ToString().PadLeft(2, '0') + "_";
            TodayLogFileName += Today.Day.ToString().PadLeft(2, '0') + ".txt";

            return TodayLogFileName;
        }






        // ###################################################################################################################
        // ###################################################################################################################
        public bool CreateLogFiles
        {
            get
            {
                return bCreateLogFiles;
            }
        }


        // ###################################################################################################################
        // ###################################################################################################################
        public String CurrentLogFileName
        {
            get
            {
                return LogFileName;
            }
        }


        // ###################################################################################################################
        // ###################################################################################################################
        public String CurrentLogFilePath
        {
            get
            {
                return LogFilePath;
            }
        }

    }
}
