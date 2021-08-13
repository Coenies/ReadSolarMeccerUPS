using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadUPS
{
    public class QPIGS
    {
        public double Grid_voltage { get; set; }
        public double Grid_frequency { get; set; }
        public double AC_output_voltage { get; set; }
        public double AC_output_frequency { get; set; }
        public int AC_output_apparent_power { get; set; }
        public int AC_output_active_power { get; set; }
        public int Output_load_percent { get; set; }
        public int BUS_voltage { get; set; }
        public double Battery_voltage { get; set; }
        public int Battery_charging_current { get; set; }
        public int Battery_capacity { get; set; }
        public int Inverter_heat_sink_temperature { get; set; }
        public int PV_Input_current_1 { get; set; }
        public double PV_Input_voltage_1 { get; set; }
        public double Battery_voltage_from_SCC_1 { get; set; }
        public int Battery_discharge_current { get; set; }
        public bool SBU_priority { get; set; }
        public bool configuration_status { get; set; }
        public bool SCC_firmware_version_updated { get; set; }
        public bool Load_status { get; set; }
        public bool battery_voltage_to_steady_while_charging { get; set; }
        public bool Charging_status { get; set; }
        public bool Charging_status_SCC1 { get; set; }
        public bool Charging_status_AC { get; set; }
        public int Battery_voltage_offset_for_fans_on { get; set; }
        public int EEPROM_version { get; set; }
        public int PV_Charging_power_1 { get; set; }
        public bool charging_to_floating_mode { get; set; }
        public bool Switch_On { get; set; }

        /// <summary>
        /// Parse string recieved from the UPS into its constituent fields
        /// </summary>
        /// <param name="data"></param>
        public QPIGS(byte[] data)
        {
            if (data.Length < 105)
                return;
            Grid_voltage = ParseToDouble(data, 1, 5);
            Grid_frequency = ParseToDouble(data, 7, 4);
            AC_output_voltage = ParseToDouble(data, 12, 5);
            AC_output_frequency = ParseToDouble(data, 18, 4);
            AC_output_apparent_power = ParseToInt(data, 23, 4);
            AC_output_active_power = ParseToInt(data, 28, 4);
            Output_load_percent = ParseToInt(data, 33, 3);
            BUS_voltage = ParseToInt(data, 37, 3);
            Battery_voltage = ParseToDouble(data, 41, 5);
            Battery_charging_current = ParseToInt(data, 47, 3);
            Battery_capacity = ParseToInt(data, 51, 3);
            Inverter_heat_sink_temperature = ParseToInt(data, 55, 4);
            PV_Input_current_1 = ParseToInt(data, 60, 4);
            PV_Input_voltage_1 = ParseToDouble(data, 65, 5);
            Battery_voltage_from_SCC_1 = ParseToDouble(data, 71, 5);
            Battery_discharge_current = ParseToInt(data, 77, 5);
            SBU_priority = ParseToBool(data, 83);
            configuration_status = ParseToBool(data, 84);
            SCC_firmware_version_updated = ParseToBool(data, 85);
            Load_status = ParseToBool(data, 86);
            battery_voltage_to_steady_while_charging = ParseToBool(data, 87);
            Charging_status = ParseToBool(data, 88);
            Charging_status_SCC1 = ParseToBool(data, 89);
            Charging_status_AC = ParseToBool(data, 90);
            Battery_voltage_offset_for_fans_on = ParseToInt(data, 92, 2);
            EEPROM_version = ParseToInt(data, 95, 2);
            PV_Charging_power_1 = ParseToInt(data, 98, 5);
            charging_to_floating_mode = ParseToBool(data, 104);
            Switch_On = ParseToBool(data, 105);
        }

        //Return JSON string representing object for MQTT transmission
        public string GetJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public int ParseToInt(byte[] data, int from, int len)
        {
            if (data.Length < from + len)
                return 0;
            var stringval = System.Text.Encoding.ASCII.GetString(data, from, len);
            int outval = 0;
            int.TryParse(stringval, out outval);
            return outval;
        }

        public double ParseToDouble(byte[] data, int from, int len)
        {
            if (data.Length < from + len)
                return 0;
            var stringval = System.Text.Encoding.ASCII.GetString(data, from, len);
            double outval = 0;
            double.TryParse(stringval, out outval);
            return outval;
        }

        public bool ParseToBool(byte[] data, int from)
        {
            if (data.Length < from || data.Length < 1)
                return false;
            if (data[from] == 0x31)
                return true;
            return false;
        }
    }
}
