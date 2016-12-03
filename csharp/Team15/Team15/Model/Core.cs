using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comunication;
using System.Threading;

namespace Team15.Model
{
    class Core
    {
        public Settings ActualSettings { get; set; }
        public Action OnDisconnect;
        public byte Temperature { get; private set; }
        public byte DayTime { get; private set; }

        private SerialConnection _serialConnection;
        private DatabaseCommunication _databaseCommunication;

        public Core()
        {
            _serialConnection = new SerialConnection();
            _databaseCommunication = new DatabaseCommunication();
            SetSettingsFromList(_databaseCommunication.GetSettings());
        }

        public string[] FindPossibleConections()
        {
            return _serialConnection.GetSerialPorts();
        }

        public void StartServer(string port, string url)
        {
            _serialConnection.StartCommunication(port);
            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Topeni, 0x30);
            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Okno, 0x30);
            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Ventilace, 0x30);
            Thread server = new Thread(() =>
            {
                for (;;)
                {
                    try
                    {
                        byte[] updateData = _serialConnection.ReceiveData();
                        Temperature = updateData[0];
                        DayTime = updateData[1];
                        _databaseCommunication.UpdateData(Temperature, DayTime);
                    }
                    catch
                    {

                    }
                    ReceivedParameters RP;
                    try
                    {
                        RP = GetDataFromString(_databaseCommunication.GetData());
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    if (RP == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    if (RP.Heat <= ActualSettings.TemperatureMin)
                    {
                        if (!ActualSettings.Heating)
                        {
                            Console.WriteLine("Too cold: " + RP.Heat);
                            //Teplota moc nízká
                            ActualSettings.Heating = true;
                            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Topeni, 0x31);
                            if (ActualSettings.Windows)
                            {
                                ActualSettings.Windows = false;
                                _serialConnection.SendCommand(SerialConnection.PossibleChanges.Okno, 0x30);
                                ActualSettings.Venting = true;
                                _serialConnection.SendCommand(SerialConnection.PossibleChanges.Ventilace, 0x31);
                            }
                            SendChange();
                        }
                    }
                    else if (RP.Heat >= ActualSettings.TemperatureMax)
                    {
                        if (ActualSettings.Heating)
                        {
                            //Teplota moc vysoká
                            Console.WriteLine("Too hot: " + RP.Heat);
                            ActualSettings.Heating = false;
                            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Topeni, 0x30);
                            if (ActualSettings.Venting)
                            {
                                ActualSettings.Venting = false;
                                _serialConnection.SendCommand(SerialConnection.PossibleChanges.Ventilace, 0x30);
                                ActualSettings.Windows = true;
                                _serialConnection.SendCommand(SerialConnection.PossibleChanges.Okno, 0x31);
                            }
                            SendChange();
                        }
                    }
                    if (RP.AirConditioning <= ActualSettings.AirConditioningMin)
                    {
                        Console.WriteLine("Too dry: " + RP.AirConditioning);
                        if (ActualSettings.Windows)
                        {
                            //Vlhkost vzduchu moc nízká (dlouho otevřené okno)
                            ActualSettings.Windows = false;
                            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Okno, 0x30);
                        }
                        else if (ActualSettings.Venting)
                        {
                            ActualSettings.Venting = false;
                            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Ventilace, 0x30);
                        }
                        SendChange();
                    }
                    else if (RP.AirConditioning >= ActualSettings.AirConditioningMax)
                    {
                        Console.WriteLine("Too wet: " + RP.AirConditioning);
                        Console.WriteLine("");
                        if (!ActualSettings.Windows && !ActualSettings.Heating)
                        {
                            //Vlhkost vzduchu moc vysoká (dlouho zavřené okno)
                            ActualSettings.Windows = true;
                            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Okno, 0x31);
                        }
                        else if (!ActualSettings.Venting)
                        {
                            ActualSettings.Venting = true;
                            _serialConnection.SendCommand(SerialConnection.PossibleChanges.Ventilace, 0x31);
                        }
                        SendChange();
                    }
                    else
                    {
                        if (!_serialConnection.SendCheckByte())
                        {
                            if (OnDisconnect != null)
                            {
                                OnDisconnect();
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
            server.Start();
        }

        public void NewSettings(Settings settings)
        {
            ActualSettings = settings;
            _databaseCommunication.UpdateSettings(ActualSettings.TemperatureMin, ActualSettings.TemperatureMax, ActualSettings.AirConditioningMin, ActualSettings.AirConditioningMax, ActualSettings.Windows ? 1 : 0, ActualSettings.Heating ? 1 : 0);
        }

        public void SendChange()
        {
            _databaseCommunication.UpdateSettings(ActualSettings.TemperatureMin, ActualSettings.TemperatureMax, ActualSettings.AirConditioningMin, ActualSettings.AirConditioningMax, ActualSettings.Windows ? 1 : 0, ActualSettings.Heating ? 1 : 0);
        }


        private ReceivedParameters GetDataFromString(List<string> Data)
        {
            int pointer = 2;
            string[] Split = Data[pointer].Split('"');
            if (Split[3].StartsWith("TeplotaPokoj"))
                Split = Data[pointer + 1].Split('"');
            else
            {
                pointer = 8;
                Split = Data[pointer].Split('"');
                if (Split[3].StartsWith("TeplotaPokoj"))
                    Split = Data[pointer + 1].Split('"');
                else
                {
                    pointer = 14;
                    Split = Data[pointer].Split('"');
                    if (Split[3].StartsWith("TeplotaPokoj"))
                        Split = Data[pointer + 1].Split('"');
                    else return null;
                }
            }
            int Temperature = int.Parse(Split[3].Split('.')[0]);
            Split = Data[pointer + 2].Split('"');
            int Conditioning = int.Parse(Split[3].Split('.')[0]);
            return new ReceivedParameters(Temperature, Conditioning);
        }

        private void SetSettingsFromList(List<string> Data)
        {
            ActualSettings = new Settings(FindNumber(Data[2]), FindNumber(Data[3]), FindNumber(Data[4]), FindNumber(Data[5]), FindNumber(Data[6]) == 1, FindNumber(Data[7]) == 1, false);
        }
        private int FindNumber(string s)
        {
            string Number = "";
            bool Found = false;
            for (int i = s.Length - 1; i > 0; i--)
            {
                if (s[i] > 47 && s[i] < 58)
                {
                    Found = true;
                    Number = s[i] + Number;
                }
                else if (Found)
                    break;
            }
            return int.Parse(Number);
        }
    }

    public class Settings
    {
        public int TemperatureMin { get; set; }
        public int TemperatureMax { get; set; }
        public int AirConditioningMin { get; set; }
        public int AirConditioningMax { get; set; }
        public bool Windows { get; set; }
        public bool Heating { get; set; }
        public bool Venting { get; set; }

        public Settings(int temperatureMax, int temperatureMin, int airConditioningMax, int airConditioningMin, bool windows, bool heating, bool venting)
        {
            TemperatureMin = temperatureMin;
            TemperatureMax = temperatureMax;
            AirConditioningMin = airConditioningMin;
            AirConditioningMax = airConditioningMax;
            Windows = windows;
            Heating = heating;
            Venting = venting;
        }

        public Settings() { }
    }

    public class ReceivedParameters
    {
        public int Heat { get; set; }
        public int AirConditioning { get; set; }

        public ReceivedParameters(int heat, int airConditioning)
        {
            Heat = heat;
            AirConditioning = airConditioning;
        }

        public ReceivedParameters()
        {

        }
    }
}
