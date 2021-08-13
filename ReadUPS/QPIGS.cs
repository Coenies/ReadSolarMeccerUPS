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

        public QPIGS(byte[] data)
        {
            Grid_voltage = parsetodouble(data, 1, 5);
            Grid_frequency = parsetodouble(data, 7, 4);
            AC_output_voltage = parsetodouble(data, 12, 5);
            AC_output_frequency = parsetodouble(data, 18, 4);
            AC_output_apparent_power = parseToInt(data, 23, 4);
            AC_output_active_power = parseToInt(data, 28, 4);
            Output_load_percent = parseToInt(data, 33, 3);
            BUS_voltage = parseToInt(data, 37, 3);
            Battery_voltage = parsetodouble(data, 41, 5);
            Battery_charging_current = parseToInt(data, 47, 3);
            Battery_capacity = parseToInt(data, 51, 3);
            Inverter_heat_sink_temperature = parseToInt(data, 55, 4);
            PV_Input_current_1 = parseToInt(data, 60, 4);
            PV_Input_voltage_1 = parsetodouble(data, 65, 5);
            Battery_voltage_from_SCC_1 = parsetodouble(data, 71, 5);
            Battery_discharge_current = parseToInt(data, 77, 5);
            SBU_priority = parsetobool(data, 83);
            configuration_status = parsetobool(data, 84);
            SCC_firmware_version_updated = parsetobool(data, 85);
            Load_status = parsetobool(data, 86);
            battery_voltage_to_steady_while_charging = parsetobool(data, 87);
            Charging_status = parsetobool(data, 88);
            Charging_status_SCC1 = parsetobool(data, 89);
            Charging_status_AC = parsetobool(data, 90);
            Battery_voltage_offset_for_fans_on = parseToInt(data, 92, 2);
            EEPROM_version = parseToInt(data, 95, 2);
            PV_Charging_power_1 = parseToInt(data, 98, 5);
            charging_to_floating_mode = parsetobool(data, 104);
            Switch_On = parsetobool(data, 105);



        }

        public string GetJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        private int parseToInt(byte[] data, int from, int len)
        {
            if (data.Length < from + len)
                return 0;
            var stringval = System.Text.Encoding.ASCII.GetString(data, from, len);
            int outval = 0;
            int.TryParse(stringval, out outval);
            return outval;
        }

        private double parsetodouble(byte[] data, int from, int len)
        {
            if (data.Length < from + len)
                return 0;
            var stringval = System.Text.Encoding.ASCII.GetString(data, from, len);
            double outval = 0;
            double.TryParse(stringval, out outval);
            return outval;
        }

        private bool parsetobool(byte[] data, int from)
        {
            if (data.Length < from)
                return false;
            if (data[from] == 0x31)
            {
                return true;
            }
            return false;
        }
    }
}
