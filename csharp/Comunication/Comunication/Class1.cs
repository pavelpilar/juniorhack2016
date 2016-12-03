//Created by TEAM 15
//Knihovna pro sériovou komunikaci s Arduinem, které bude kontrolovat prvky v domě (Otvírání oken, termostat, atd...)
//Dále obsahuje třídu pro získávání informací z databáze, které následně předává programu pro vyhodnocení

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace Comunication
{
    public class SerialConnection
    {
        public Action Disconected;

        private const int Speed = 115200;
        private byte[] CheckByte = new byte[] { 1 };
        private SerialPort Port;
        private Stream stream;

        public enum PossibleChanges
        {
            Okno,
            Ventilace,
            Topeni
        }

        public SerialConnection()
        {
            Port = new SerialPort();
            Port.BaudRate = 115200;
            Port.Handshake = Handshake.None;
        }
        public string[] GetSerialPorts()
        {
            return SerialPort.GetPortNames();
        }
        public void StartCommunication(string PortName)
        {
            if (!Port.IsOpen)
            {
                Port.PortName = PortName;
                try
                {
                    Port.Open();
                }
                catch(UnauthorizedAccessException)
                {
                    throw new Exception("Port byl odpojen nebo je právě používaný");
                }
                catch(Exception e)
                {
                    throw e;
                }
                stream = Port.BaseStream;
                stream.ReadTimeout = 250;
            }
            else
            {
                throw new Exception("Port je již otevřen");
            }
        }
        public void StopCommunication()
        {
            if (Port.IsOpen)
                Port.Close();
        }
        public void SendCommand(PossibleChanges Prvek, byte Hodnota)
        {
            if (stream.CanWrite)
            {
                byte[] Buffer = new byte[2];
                Buffer[0] = (byte)Prvek;
                Buffer[1] = Hodnota;
                stream.Write(Buffer, 0, Buffer.Length);
                stream.Flush();
            }
            else
            {
                Disconected();
            }
        }
        public byte[] ReceiveData()
        {
            byte[] buffer = new byte[2];
            stream.Read(buffer, 0, 2);
            return buffer;
        }
        public bool SendCheckByte()
        {
            try
            {
                stream.Write(CheckByte, 0, 1);
                stream.Flush();
                return true;
            }
            catch
            {
                Disconected();
                return false;
            }
        }
    }
    public class DatabaseCommunication
    {
        private string Url;
        private string GetString;
        private string GetSettingString;

        public DatabaseCommunication()
        {
            Url = "http://tymc15.jecool.net/www/api/";
            GetString = "vyber?key=t4m15&pocet=2";
            GetSettingString = "nastaveni?key=t4m15&volba=1&nid=1";
        }
        public List<string> GetData()
        {
            List<string> Ret = new List<string>();
            Console.WriteLine("Creating Request");
            WebRequest Req = WebRequest.Create(Url + GetString);
            Console.WriteLine("Getting Response");
            WebResponse Res = Req.GetResponse();
            Console.WriteLine("Creating SR");
            StreamReader SR = new StreamReader(Res.GetResponseStream());
            string Line;
            Console.WriteLine("Reading Data");
            while ((Line = SR.ReadLine()) != null)
            {
                Ret.Add(Line);
                if (Line[0] == ']')
                    break;
            }
            Console.WriteLine("Closing Stream");
            SR.Close();
            return Ret;
        }
        public List<string> GetSettings()
        {
            List<string> Ret = new List<string>();
            WebRequest Req = WebRequest.Create(Url + GetSettingString);
            WebResponse Res = Req.GetResponse();
            StreamReader SR = new StreamReader(Res.GetResponseStream());
            string Line;
            while((Line = SR.ReadLine()) != null)
            {
                Ret.Add(Line);
                if (Line[0] == ']')
                    break;
            }
            SR.Close();
            return Ret;
        }
        public void UpdateSettings(int TempMin, int TempMax, int WetnMin, int WetnMax, int Windows, int Heating)
        {
            string c = String.Format("{0}nastaveni?key={1}&nid={2}&maxtmp={3}&mintmp={4}&maxvum={5}&minvum={6}&topeni={7}&okna={8}", Url, "t4m15", 1, TempMax, TempMin, WetnMax, WetnMin, Heating, Windows);
            WebRequest WR = WebRequest.Create(c);
            WR.GetResponse();
        }
        public void UpdateData(byte temperature, byte day)
        {
            string UpdateUrl = String.Format("{0}pridat?key=t4m15&sid={1}&tmp={2}&lum={3}", Url, "output", temperature, day);
            WebRequest WR = WebRequest.Create(UpdateUrl);
            WR.GetResponse();
        }
    }
}
