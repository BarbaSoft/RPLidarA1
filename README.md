# RPLidarA1
RPLidar A1 scan protocol 8000 sample/sec
Dowload LidarExemple for exemple on use NuGet package

Quickstart guide:

//Create object and search serial ports avaiable
rplidar = new RPLidarA1class();
string[] porte=rplidar.FindSerialPorts(); //Find serial ports

//Open Serial Port
bool result=rplidar.ConnectSerial("COM1"); //Open Serial Port COM1

//Retrieve RPLidar Serial Number and Version
string snum = rplidar.SerialNum(); //Get RPLidar Info

//Start Scan 8000 sample/second
if (!rplidar.BoostScan()) //Start BoostScan
   {
        rplidar.CloseSerial(); //On scan error close serial port
        return;
   }

//Create a timer and every ~500ms read rplidar.Measure_List.
//Mesure_List contains at most 100000 sample (auto clear oldest).
//Mesure_List has 4 member (float angle -degrees, int distance -millimeters,double X -position,double Y -position)

private void Timer_Tick(object sender, EventArgs e)  //Refresh mesure/second and measure list
{
      List<Measure> measures = new List<Measure>(); //Measures list
      string sample_second = rplidar.measure_second.ToString();

      lock (rplidar.Measure_List)
         {
             foreach (Measure m in rplidar.Measure_List) //Copy Measure List
              {
                  measures.Add(m);
              }
              rplidar.Measure_List.Clear(); //Clear original List
          }
  //.... use the measures list
}
  
////Stop Scan when you want close comunication !
rplidar.Stop_Scan(); //Stop Scan Thread
rplidar.CloseSerial(); //Close serial port
