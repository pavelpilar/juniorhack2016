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
                byte[] Buffer = null;
                switch (Prvek)
                {
                    case PossibleChanges.OknoPrvniPatro:
                        Buffer = new byte[4];
                        Buffer[1] = 10;
                        Buffer[2] = Hodnota;
                        break;
                    case PossibleChanges.OknoDruhyPatro:
                        Buffer = new byte[4];
                        Buffer[1] = 20;
                        Buffer[2] = Hodnota;
                        break;
                    case PossibleChanges.OknoStrecha:
                        Buffer = new byte[4];
                        Buffer[1] = 30;
                        Buffer[2] = Hodnota;
                        break;
                    case PossibleChanges.Termostat:
                        Buffer = new byte[4];
                        Buffer[1] = 100;
                        Buffer[2] = Hodnota;
                        break;
                    case PossibleChanges.Klimatizace:
                        Buffer = new byte[4];
                        Buffer[1] = 110;
                        Buffer[2] = Hodnota;
                        break;
                    default:
                        break;
                }
                Buffer[0] = 24;
                Buffer[3] = 255;
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
        private string IP = "";
        public List<string> GetData()
        {
            return null;
        }
        public void UpdateHouseData(string Prvek, int Hodnota)
        {
            WebRequest Req = WebRequest.Create("http://localhost/?5");
        }
    }
    public static class HouseSettings<T>
    {
        private static string Path = Environment.ExpandEnvironmentVariables("%appdata%/Team15/Config.Bin");
        private static XmlSerializer Serializer = new XmlSerializer(typeof(T));
        public static T LoadSettings()
        {
            if (!File.Exists(Path))
                throw new Exception("Soubor s nastavením nebyl nalezen");
            object o = Serializer.Deserialize(new FileStream(Path, FileMode.Open));
        }
    }
}
