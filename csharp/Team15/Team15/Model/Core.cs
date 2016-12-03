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


        private SerialConnection _serialConnection;
        private DatabaseCommunication _databaseCommunication;
        private HouseSettings<Settings> _houseSettings;

        public Core()
        {
            try
            {
                ActualSettings = _houseSettings.LoadSettings();
            }
            catch {}
        }

        public string[] FindPossibleConections()
        {
            return _serialConnection.GetSerialPorts();
        }

        public void StartServer(string port, string url)
        {
            _serialConnection.StartCommunication(port);
            Thread server = new Thread(() =>
            {
                ReceivedParameters RP = GetDataFromString(_databaseCommunication.GetData());
                if (RP.Heat <= ActualSettings.TemperatureMin)
                {
                    if (!ActualSettings.Heating)
                        //Teplota moc nízká
                        _serialConnection.SendCommand(SerialConnection.PossibleChanges.Topeni, 1);
                }
                else if (RP.Heat >= ActualSettings.TemperatureMax)
                {
                    if (ActualSettings.Heating)
                        //Teplota moc vysoká
                        _serialConnection.SendCommand(SerialConnection.PossibleChanges.Topeni, 0);
                }
                if (RP.AirConditioning <= ActualSettings.AirConditioningMin)
                {
                    if (ActualSettings.Windows)
                        //Vlhkost vzduchu moc nízká (dlouho otevřené okno)
                        _serialConnection.SendCommand(SerialConnection.PossibleChanges.Okno, 0);
                }
                else if (RP.AirConditioning >= ActualSettings.AirConditioningMax)
                {
                    if (!ActualSettings.Windows)
                        //Vlhkost vzduchu moc vysoká (dlouho zavřené okno)
                        _serialConnection.SendCommand(SerialConnection.PossibleChanges.Okno, 1);
                }
                else
                {
                    /*if (!_serialConnestion.SendCheckByte())
                    {
                        if(OnDisconnect != null)
                        {
                            OnDisconnect();
                        }
                    }*/
                }
                Thread.Sleep(1000);
            });
        }

        public void NewSettings(Settings settings)
        {
            ActualSettings = settings;
            _houseSettings.SaveSetting(settings);
        }

        private ReceivedParameters GetDataFromString(List<string> Data)
        {
            string[] Split = Data[3].Split('"');
            float Temperature = float.Parse(Split[3].Replace('.', ','));
            Split = Data[4].Split('"');
            float Conditioning = float.Parse(Split[3].Replace('.', ','));
            return new ReceivedParameters();
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

        public Settings(int temperatureMin, int temperatureMax, int airConditioningMin, int airConditioningMax, bool windows, bool heating)
        {
            TemperatureMin = temperatureMin;
            TemperatureMax = temperatureMax;
            AirConditioningMin = airConditioningMin;
            AirConditioningMax = airConditioningMax;
            Windows = windows;
            Heating = heating;
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
