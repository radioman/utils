using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using NetNamedPipeBindingServiceContract;

namespace NetNamedPipeBindingServiceHost
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class RemoteObject : IRemoteObject
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(RemoteObject));
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                host.Description.Behaviors.Add(smb);

                //provide metadata
                Binding mex = MetadataExchangeBindings.CreateMexNamedPipeBinding();
                host.AddServiceEndpoint(typeof(IMetadataExchange), mex, "net.pipe://localhost/Demo/Meta");

                NetNamedPipeBinding pipe = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
                {
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue,
                    MaxBufferPoolSize = int.MaxValue,
                    TransferMode = TransferMode.Streamed
                };
                pipe.ReaderQuotas.MaxArrayLength = int.MaxValue;
                pipe.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                pipe.ReaderQuotas.MaxStringContentLength = int.MaxValue;

                host.AddServiceEndpoint(typeof(IRemoteObject), pipe, "net.pipe://localhost/Demo/DoStuff");
            }
            host.Open();
            Console.WriteLine("Server Started");
            Console.ReadLine();
            host.Close();
        }

        readonly Guid Id = Guid.NewGuid();

        byte[] IRemoteObject.GetRBytes(int numBytes)
        {
            Console.WriteLine($"GetRBytes({numBytes}): {Id}");

            return new byte[numBytes];
        }

        bool IRemoteObject.ReceiveRBytes(byte[] bytes)
        {
            Console.WriteLine($"ReceiveRBytes({bytes.Length}): {Id}");

            return true;
        }
    }
}
