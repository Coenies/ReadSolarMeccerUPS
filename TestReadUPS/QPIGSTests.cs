using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadUPS;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReadUPS.Tests
{
    [TestClass()]
    public class QPIGSTests
    {

        byte[] _testData = System.Text.ASCIIEncoding.ASCII.GetBytes(" 236.8 49.9 236.8 49.9 1065 0945 023 403 49.90 001 077 0050 0002 001.5 09.03 00009 01010000 01 01 00009 010");
        QPIGS _qPIGS;

        [TestInitialize]
        public void StartAll()
        {
            _qPIGS = new QPIGS(_testData);
        }
        [TestMethod]
        public void TestQPIGSVoltage()
        {
            Assert.IsTrue(_qPIGS.Grid_voltage == 236.8);
            Assert.IsTrue(_qPIGS.AC_output_active_power == 945);
            Assert.IsTrue(_qPIGS.Output_load_percent == 23);
            Assert.IsTrue(_qPIGS.BUS_voltage == 403);
            Assert.IsTrue(_qPIGS.Battery_voltage == 49.90);
            Assert.IsTrue(_qPIGS.Battery_charging_current == 1);
            Assert.IsTrue(_qPIGS.Battery_capacity == 77);
            Assert.IsTrue(_qPIGS.Inverter_heat_sink_temperature == 50);
            Assert.IsTrue(_qPIGS.PV_Input_current_1 == 2);
            Assert.IsTrue(_qPIGS.PV_Input_voltage_1 == 1.5);
            Assert.IsTrue(_qPIGS.Battery_voltage_from_SCC_1 == 09.03);
            Assert.IsTrue(_qPIGS.Battery_discharge_current == 9);
            Assert.IsTrue(_qPIGS.SBU_priority == false);
            Assert.IsTrue(_qPIGS.configuration_status == true);
            Assert.IsTrue(_qPIGS.SCC_firmware_version_updated == false);
            Assert.IsTrue(_qPIGS.Load_status == true);
            Assert.IsTrue(_qPIGS.battery_voltage_to_steady_while_charging == false);
            Assert.IsTrue(_qPIGS.Charging_status == false);
            Assert.IsTrue(_qPIGS.Charging_status_SCC1 == false);
            Assert.IsTrue(_qPIGS.Charging_status_AC == false);
            Assert.IsTrue(_qPIGS.Battery_voltage_offset_for_fans_on == 1);
            Assert.IsTrue(_qPIGS.EEPROM_version == 1);
            Assert.IsTrue(_qPIGS.PV_Charging_power_1 == 9);
            Assert.IsTrue(_qPIGS.charging_to_floating_mode == false);
            Assert.IsTrue(_qPIGS.Switch_On == true);
        }
        [TestMethod]
        public void TestQPIGSFreq()
        {
            Assert.IsTrue(_qPIGS.Grid_frequency == 49.9);
        }

        [TestMethod]
        public void TestQPIGSOutVolt()
        {
            Assert.IsTrue(_qPIGS.AC_output_voltage == 236.8);
        }

        [TestMethod]
        public void TestQPIGSOutFreq()
        {
            Assert.IsTrue(_qPIGS.AC_output_frequency == 49.9);
        }

        [TestMethod]
        public void TestQPIGSOutAppPow()
        {
            Assert.IsTrue(_qPIGS.AC_output_apparent_power == 1065);
        }
        
       

        [TestMethod()]
        public void ParseToBoolTestFalse()
        {
            byte[] testbyte = new byte[1];
            QPIGS qPIGS = new QPIGS(_testData);
            byte[] data = new byte[] { 0x30 };
            Assert.IsFalse(qPIGS.ParseToBool(data, 0));
        }

        [TestMethod()]
        public void ParseToBoolNoData()
        {
            byte[] testbyte = new byte[1];
            QPIGS qPIGS = new QPIGS(_testData);
            byte[] data = new byte[] { };
            Assert.IsFalse(qPIGS.ParseToBool(data, 0));
        }

        [TestMethod()]
        public void ParseToDoubleTest()
        {
            byte[] testbyte = new byte[1];
            QPIGS qPIGS = new QPIGS(_testData);
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes("99.99");
            Assert.IsTrue(qPIGS.ParseToDouble(data, 0, 5) == 99.99);
        }

        [TestMethod()]
        public void ParseToDoubleNoData()
        {
            byte[] testbyte = new byte[1];
            QPIGS qPIGS = new QPIGS(_testData);
            byte[] data = new byte[] { };
            Assert.IsTrue(qPIGS.ParseToDouble(data, 0, 1) == 0);
        }

        [TestMethod()]
        public void ParseToIntTest()
        {
            byte[] testbyte = new byte[1];
            QPIGS qPIGS = new QPIGS(_testData);
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes("99");
            Assert.IsTrue(qPIGS.ParseToDouble(data, 0, 2) == 99);
        }

        [TestMethod()]
        public void ParseToIntNoData()
        {
            byte[] testbyte = new byte[1];
            QPIGS qPIGS = new QPIGS(_testData);
            byte[] data = new byte[] { };
            Assert.IsTrue(qPIGS.ParseToDouble(data, 0, 1) == 0);
        }
    }
}