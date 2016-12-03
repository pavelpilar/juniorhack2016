﻿using System;
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
            Thread server = new Thread(() =>
            {
                for (;;)
                {
                    try
                    {
                        byte[] updateData = _serialConnection.ReceiveData();
                        if (updateData[0] == 1)
                        {
                            Temperature = updateData[1];
                        }
                        else if (updateData[0] == 2)
                        {
                            DayTime = updateData[2];
                        }
                        if (Temperature != 0)
                        {
                            _databaseCommunication.UpdateData(Temperature, DayTime);
                        }
                    }
                    catch
                    {

                    }
                    ReceivedParameters RP;
                    try
                    {
                        RP = GetDataFromString(_databaseCommunication.GetData());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Thread.Sleep(1000);
                        continue;
                    }
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
                        if (!_serialConnection.SendCheckByte())
                        {
                            if (OnDisconnect != null)
                            {
                                OnDisconnect();
                            }
                        }
                    }
                    _databaseCommunication.UpdateData(10, 40);
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

        private ReceivedParameters GetDataFromString(List<string> Data)
        {
            int pointer = 2;
            string[] Split = Data[pointer].Split('"');
            if (Split[3].StartsWith("Teplota"))
                Split = Data[pointer + 1].Split('"');
            else
            {
                pointer = 8;
                Split = Data[pointer].Split('"');
                if (Split[3].StartsWith("Teplota"))
                    Split = Data[pointer + 1].Split('"');
                else
                    throw new Exception("Nenalezeny výsledky z pokoje");
            }
            int Temperature = int.Parse(Split[3].Split('.')[0]);
            Split = Data[pointer + 2].Split('"');
            int Conditioning = int.Parse(Split[3].Split('.')[0]);
            return new ReceivedParameters(Temperature, Conditioning);
        }

        private void SetSettingsFromList(List<string> Data)
        {
            Console.WriteLine(Data.Count);
            string[] Split = Data[2].Split(':');
            int TemperatureMax = int.Parse(Split[1]);
            Split = Data[3].Split(':');
            int TemperatureMin = int.Parse(Split[1]);
            Split = Data[4].Split(':');
            int AirConditioningMax = int.Parse(Split[1]);
            Split = Data[5].Split(':');
            int AirConditioningMin = int.Parse(Split[1]);
            Split = Data[6].Split(':');
            bool Windows = int.Parse(Split[1]) == 1;
            Split = Data[7].Split(':');
            bool Heating = int.Parse(Split[1]) == 1;
            ActualSettings = new Settings(TemperatureMin, TemperatureMax, AirConditioningMin, AirConditioningMax, Windows, Heating);
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
