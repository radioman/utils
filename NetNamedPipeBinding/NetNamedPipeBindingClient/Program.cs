using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using NetNamedPipeBindingClient.ServiceReference;

namespace NetNamedPipeBindingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                TransferMode = TransferMode.Streamed,
                SendTimeout = TimeSpan.FromSeconds(5),
                ReceiveTimeout = TimeSpan.FromSeconds(5)
            };
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            EndpointAddress addr = new EndpointAddress("net.pipe://localhost/Demo/DoStuff");
            RemoteObjectClient client = null;
            while (true)
            {
                try
                {
                    if (client == null || client.State == CommunicationState.Faulted)
                    {
                        if (client != null)
                        {
                            client.Abort();
                        }
                        client = new RemoteObjectClient(binding, addr);
                    }

                    if (client.State != CommunicationState.Opened)
                    {
                        client.Open();
                    }

                    int size = 1024 * 200;
                    byte[] buffer = client.GetRBytes(size);

                    var stopWatch = Stopwatch.StartNew();
                    int count = 500;
                    for (int i = 0; i < count; i++)
                    {
                        client.ReceiveRBytes(buffer);

                        //Thread.Sleep(200);
                    }
                    stopWatch.Stop();
                    Console.WriteLine("Time {0}, count: {1}, {2}MB/s - buffer: {3}kb",
                        stopWatch.Elapsed,
                        count,
                        ((count * size / 1024 * 1024) / stopWatch.ElapsedMilliseconds) / 1000,
                        size/1024);
                }
                catch
                {
                    Console.WriteLine("offline..." + DateTime.Now);
                }
                Thread.Sleep(1000);
            }
            client.Close();

            Console.ReadLine();
        }
    }
}
