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

            RemoteObjectClient client = new RemoteObjectClient(binding, addr);
            client.Open();

            while (true)
            {
                try
                {
                    int size = 1024 * 100;
                    byte[] buffer = client.GetRBytes(size);

                    var stopWatch = Stopwatch.StartNew();
                    int count = 1000;
                    for (int i = 0; i < count; i++)
                    {
                        client.ReceiveRBytes(buffer);

                        Thread.Sleep(10);
                    }
                    stopWatch.Stop();
                    Console.WriteLine("Time used: {0}\tPer sec: {1} - {2}", stopWatch.Elapsed, (count * size) / stopWatch.ElapsedMilliseconds, size);
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
