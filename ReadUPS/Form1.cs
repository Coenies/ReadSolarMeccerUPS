using MQTTnet;
using MQTTnet.Client;

using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace ReadUPS
{
    public partial class Form1 : Form
    {
        
        private System.Threading.Timer timer;

        public Form1()
        {
            InitializeComponent();

             timer = new System.Threading.Timer(Tick, null, 2000, 2000);
        }

        private void Tick(object state)
        {
        
        }

     

      

        
    }
}
