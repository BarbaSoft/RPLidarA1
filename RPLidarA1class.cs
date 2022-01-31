using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.IO.Ports;

namespace RPLidarA1
{
    public class RPLidarA1class
    {
        SerialPort seriale; //serialport object
        static string SYNC = "" + (char)0xA5 + (char)0x5A; //Sincronismo
        private string m_ricevuto; //Received string
        private int lbyte;


        public string[] FindSerialPorts() //Find Serial Ports on PC
        {
            string[] porte = SerialPort.GetPortNames(); 
            return porte;
        }

        public bool ConnectSerial (string com) //Open Serial Port com 
        {
            int baudrate = 115200;
            seriale = new SerialPort(com, baudrate, Parity.None, 8, StopBits.One);
            seriale.ReadTimeout = 1000;
            seriale.ReadBufferSize = 10000;

            try
            {
                seriale.Open();

            }
            catch (Exception e)
            {
                throw e;
            }
            seriale.DtrEnable = true; //Stop Motor
            
            return true;
        }

        public void CloseSerial () //Close Serial Port
        {
            if (seriale.IsOpen) seriale.Close();
        }

        public string SerialNum() //Retrive Info from Lidar
        {
            string inviare;
            int pos;
            inviare = "" +(char) 0xA5 + (char)0x50;
            Writeserial(inviare);
            if (!ReadSerial1Shot()) return "";
            pos = m_ricevuto.IndexOf(SYNC);
            if (pos < 0) return "";
            string info = "" + m_ricevuto.Substring(pos + 7);
            return info;
        }

        private bool ReadSerial1Shot()  //Read single answer
        {
            if (seriale.IsOpen == false) return false;
            lbyte = 0;
            m_ricevuto = "";

            for (int x = 0; (x < 700) && (lbyte == 0); x++)
            {
                System.Threading.Thread.Sleep(10);
                try
                {
                    lbyte = seriale.BytesToRead;
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                lbyte = seriale.BytesToRead;
            }
            catch
            {
                return false;
            }
            for (int z = 0; z < lbyte; z++)
            {
                m_ricevuto += (char)seriale.ReadByte();
            }
            return true;
        }
        private bool Writeserial(string s) //Write to serial port
        {
            if (seriale.IsOpen == false) return false;

            byte[] ss = new byte[200];
            for (int x = 0; x < s.Length; x++)
            {
                ss[x] = (byte)s.ElementAt(x);
            }

            try
            {
                seriale.Write(ss, 0, s.Length);
            }
            catch
            {
                return false;
            }
            return true;
        }


    }
}
