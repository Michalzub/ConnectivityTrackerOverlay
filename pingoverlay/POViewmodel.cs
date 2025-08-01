using pingtest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pingoverlay
{
    public class POViewmodel
    {
        public ObservableCollection<ConnectionData> Connections { get; } = new ObservableCollection<ConnectionData>();
        public int Delay { get; set; } = 10;
        private static CancellationTokenSource Cts = new CancellationTokenSource();
        private Task? _pingTask;
        private bool _isPinging = false;

        public POViewmodel() 
        {
        }

        public void DeleteConnection(ConnectionData connection)
        {
            Connections.Remove(connection);
        }

        public void AddConnection(string connectionString)
        {
            Connections.Add(new ConnectionData(connectionString));
        }

        async Task PingLoopWork()
        {
            CancellationToken token = Cts.Token;
            while (!token.IsCancellationRequested)
            {
                Trace.WriteLine("Pinging");
                foreach (ConnectionData data in Connections)
                {
                    _ = data.Ping();
                }
                try
                {
                    await Task.Delay(Delay * 1000,token);
                } catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        public async void PingConnections()
        {
            if(_isPinging)
            {
                Cts.Cancel();
                _isPinging = false;

                try { await _pingTask; } catch (OperationCanceledException) { }

            } else
            {
                Cts = new CancellationTokenSource();
                _isPinging = true;
                _pingTask = PingLoopWork();
            }

        }

        internal void ReadFromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            Connections.Clear();

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    Connections.Add(new ConnectionData(line));
                }
            }
        }

        internal void SaveToFile(string fileName)
        {
            var lines = Connections.Select(c => c.Ipadd);
            File.WriteAllLines(fileName, lines);
        }
    }
}
