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
        private const int Speed = 115200;
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
                stream = Port.BaseStream;
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
                byte[] Buffer = new byte[3];
                switch (Prvek)
                {
                    case PossibleChanges.Okno:
                        Buffer[0] = 10;
                        Buffer[1] = Hodnota;
                        break;
                    case PossibleChanges.Topeni:
                        Buffer[0] = 20;
                        Buffer[1] = Hodnota;
                        break;
                    case PossibleChanges.Ventilace:
                        Buffer[0] = 30;
                        Buffer[1] = Hodnota;
                        break;
                    case PossibleChanges.Zaluzie:
                        Buffer[0] = 100;
                        Buffer[1] = Hodnota;
                        break;
                    default:
                        break;
                }
                Buffer[2] = 10;
                stream.Write(Buffer, 0, Buffer.Length);
            }
            else
            {
                if (!Port.IsOpen)
                    throw new Exception("Arduino není připojeno");
                else
                    throw new Exception("Nečekaná chyba");
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
            GetString = "?ziskat=5";
            SetStringType = "";
            SetStringValue = "";
        }
        public string GetData()
        {
            List<string> Ret = new List<string>();
            WebRequest Req = WebRequest.Create(Url);
            WebResponse Res = Req.GetResponse();
            StreamReader SR = new StreamReader(Res.GetResponseStream());
            string Line = SR.ReadLine();
            SR.Close();
            for (int i = 0; i < Line.Length - 1; i++)
            {
                if (Line[i] == '<' && Line[i + 1] == '!')
                {
                    return Line.Remove(i, Line.Length - i);
                }
            }
            return Line;
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
