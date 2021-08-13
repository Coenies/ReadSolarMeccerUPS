using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using ReadUPS;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ReadUPSComms
{
    public class UPSReader : BackgroundService
    {
        private const string _comPort = "COM1";
        private const int _baudRate = 2400;
        private readonly ILogger<UPSReader> _logger;
        private int _st = 0;
        private const string _BROKER_URL = "192.168.15.157";
        private const string _clientId = "UPSStats";
        private IMqttClient _mqttClient;
        private string _buffer = "";
        private DateTime _lastread = DateTime.Now;
        private SerialPort _comOut { get; set; }

        public UPSReader(ILogger<UPSReader> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Worker();
                await Task.Delay(5000, stoppingToken);
            }
        }

        /// <summary>
        /// Main service loop
        /// </summary>
        public void Worker()
        {
            if (_comOut != null && _comOut.IsOpen && DateTime.Now.Subtract(_lastread).Seconds < 10)
            {
                if (_st == 0)
                {
                    var cmdbytes = new byte[] { 0x51, 0x50, 0x49, 0x47, 0x53, 0xB7, 0xA9, 0x0D };
                    _comOut.Write(cmdbytes, 0, cmdbytes.Length);
                }
                else if (_st == 1)
                {
                    var cmdbytes2 = new byte[] { 0x51, 0x50, 0x49, 0x47, 0x53, 0x32, 0x68, 0x2D, 0x0D };
                    _comOut.Write(cmdbytes2, 0, cmdbytes2.Length);
                }
            }
            else
            {
                try
                {
                    Connect();
                }
                catch (Exception)
                {

                }
            }
            if (_mqttClient == null || !_mqttClient.IsConnected)
            {
                try
                {
                    var factory = new MqttFactory();
                    _mqttClient = factory.CreateMqttClient();
                    var options = new MqttClientOptionsBuilder()
                        .WithClientId(_clientId)
                        .WithTcpServer(_BROKER_URL)
                        .Build();
                    _mqttClient.ConnectAsync(options);
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Open com port function. Exceptions are expected and quite normal with .net
        /// </summary>
        private void Connect()
        {
            if (_comOut != null)
                try
                {
                    _comOut.Close();
                }
                catch (Exception)
                {

                }
            _comOut = new SerialPort();
            _comOut.Parity = Parity.None;
            _comOut.PortName = _comPort;
            _comOut.StopBits = StopBits.One;
            _comOut.DataBits = 8;
            _comOut.BaudRate = _baudRate;
            _comOut.Open();
            _comOut.DataReceived += ComOut_DataReceived;

        }

        /// <summary>
        /// Callback function from comport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComOut_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _lastread = DateTime.Now;
            if (_comOut.BytesToRead > 106)
            {

                var stringvalue = "";
                int dataLength = _comOut.BytesToRead;
                byte[] data = new byte[dataLength];
                int nbrDataRead = _comOut.Read(data, 0, dataLength);
                if (nbrDataRead != 0)
                {
                    try
                    {
                        stringvalue = System.Text.Encoding.ASCII.GetString(data);
                        _buffer += stringvalue;
                        var terminatedpart = _buffer.IndexOf((char)(0x0d));
                        if (terminatedpart > 0)
                        {
                            stringvalue = _buffer.Substring(0, terminatedpart);
                            terminatedpart++;
                            if (_buffer.Length > terminatedpart && _buffer.Length - terminatedpart > 7)
                                _buffer = _buffer.Substring(terminatedpart, _buffer.Length - terminatedpart);
                            // buffer = "";
                            else
                                _buffer = "";

                            if (_buffer.Length > 300)
                                _buffer = "";

                            _logger.LogInformation(stringvalue);
                            if (stringvalue.Length > 100)
                            {
                                var res = new QPIGS(System.Text.Encoding.ASCII.GetBytes(stringvalue));
                                var json = res.GetJSON();
                                _mqttClient.PublishAsync("UPSData", json); ;
                                _logger.LogInformation(json);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }
        }
    }
}
