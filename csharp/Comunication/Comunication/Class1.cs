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
            Topeni,
            Zaluzie
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
                switch (Prvek)
                {
                    case PossibleChanges.Okno:
                        Buffer[0] = 15;
                        Buffer[1] = Hodnota;
                        break;
                    case PossibleChanges.Topeni:
                        Buffer[0] = 25;
                        Buffer[1] = Hodnota;
                        break;
                    case PossibleChanges.Ventilace:
                        Buffer[0] = 35;
                        Buffer[1] = Hodnota;
                        break;
                    case PossibleChanges.Zaluzie:
                        Buffer[0] = 100;
                        Buffer[1] = Hodnota;
                        break;
                    default:
                        break;
                }
                stream.Write(Buffer, 0, Buffer.Length);
                stream.Flush();
            }
            else
            {
                Disconected();
            }
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
        private string SetStringType;
        private string SetStringValue;

        public DatabaseCommunication()
        {
            Url = "http://tymc15.jecool.net/www/api/";
            GetString = "vyber?key=t4m15&pocet=1";
            SetStringType = "";
            SetStringValue = "";
        }
        public List<string> GetData()
        {
            List<string> Ret = new List<string>();
            WebRequest Req = WebRequest.Create(Url + GetString);
            WebResponse Res = Req.GetResponse();
            StreamReader SR = new StreamReader(Res.GetResponseStream());
            string Line;
            while ((Line = SR.ReadLine()) != null)
            {
                Ret.Add(Line);
                if (Line[0] == ']')
                    break;
            }
            SR.Close();
            return Ret;
        }
        public void UpdateHouseData(string Prvek, int Hodnota)
        {
            WebRequest Req = WebRequest.Create(Url + SetStringType + Prvek + SetStringValue + Hodnota);
        }
    }
    public class HouseSettings<T>
    {
        private string Path;
        private XmlSerializer Serializer;
        public HouseSettings()
        {
            Path = Environment.ExpandEnvironmentVariables("%appdata%/Team15/Config.Bin");
            Serializer = new XmlSerializer(typeof(T));
        }
        public T LoadSettings()
        {
            if (!File.Exists(Path))
                throw new Exception("Soubor s nastavením nebyl nalezen");
            using (FileStream FS = new FileStream(Path, FileMode.Open))
            {
                return (T)Serializer.Deserialize(FS);
            }
        }
        public void SaveSetting(T ob)
        {
            using (FileStream FS = new FileStream(Path, FileMode.OpenOrCreate))
            {
                Serializer.Serialize(FS, ob);
            }
        }
    }
}
