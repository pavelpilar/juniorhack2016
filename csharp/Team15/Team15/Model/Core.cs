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


        private SerialConnection _serialConnestion;
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
            return _serialConnestion.GetSerialPorts();
        }

        public void StartServer(string port, string url)
        {
            _serialConnestion.StartCommunication(port);
            Thread server = new Thread(() =>
            {
                string s = _databaseCommunication.GetData();
                bool change;
                /*yolo code co s hodně if*/
                if (change)
                {
                    //_serialConnestion.SendCommand();
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
            });
        }

        public void NewSettings(Settings settings)
        {
            ActualSettings = settings;
            _houseSettings.SaveSetting(settings);
        }
    }

    public class Settings
    {
        public int Heating { get; set; }
        public int AirConditioning { get; set; }
        public int Windows { get; set; }
        public int Jalousie { get; set; }

        public Settings(int heating, int airConditioning, int windows, int jalousie)
        {
            Heating = heating;
            AirConditioning = airConditioning;
            Windows = windows;
            Jalousie = jalousie;
        }

        public Settings() { }
    }
}
