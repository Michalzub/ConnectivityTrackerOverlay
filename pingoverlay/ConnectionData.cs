using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace pingtest
{
    public class ConnectionData : INotifyPropertyChanged
    {
        private IPStatus status = IPStatus.Unknown;
        private long rtt = -1;
        private SolidColorBrush scb = new SolidColorBrush(Color.FromRgb(150,150,150));


        public string Ipadd { get; set; }
        public IPStatus Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                    StatusColor = scb;
                }
            }
        }
        public long Rtt
        {
            get => rtt;
            set
            {
                if (rtt != value)
                {
                    rtt = value;
                    OnPropertyChanged(nameof(Rtt));
                    StatusColor = scb;
                }
            }
        }

        public string RttString
        {
            get => rtt.ToString() + " ms";
        }

        public SolidColorBrush StatusColor
        {
            get => scb;

            set
            {
                scb = value;
                scb.Color = GetStatusColor();
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public Color GetStatusColor()
        {
            if (Status == IPStatus.Unknown)
            {
                return Color.FromRgb(150, 150, 150);
            }
            else if (Status != IPStatus.Success)
            {
                return Color.FromRgb(255, 0, 0);
            }
            else
            {
                return (Rtt < 10) ? Color.FromRgb(0, 255, 0) : Color.FromRgb(255, 255, 0);
            }
        }


        public ConnectionData(string ipaddress)
        {
            Ipadd = ipaddress;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Ping()
        {
            try
            {
                Ping ping = new Ping();
                var reply = await ping.SendPingAsync(Ipadd, 1000);
                Status = reply.Status;
                Rtt = reply.RoundtripTime;
            }
            catch
            {
                Status = IPStatus.Unknown;
                Rtt = -1;
            }

        }
    }
}