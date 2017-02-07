using System;
using System.Diagnostics;
using System.Net.Http;

namespace WinHttpHandlerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (HttpClient c = new HttpClient(new WinHttpHandler()
            {
                 MaxConnectionsPerServer = 1,
                 Proxy = null,
                 WindowsProxyUsePolicy = WindowsProxyUsePolicy.DoNotUseProxy,
                  
                 SendTimeout = TimeSpan.FromMilliseconds(1000),
                 ReceiveDataTimeout = TimeSpan.FromMilliseconds(1000),
                 ReceiveHeadersTimeout = TimeSpan.FromMilliseconds(1000),
                      
            }, true))
            {
                c.Timeout = TimeSpan.FromMilliseconds(1000);
                //c.CancelPendingRequests();

                while(true)
                {
                    var t1 = c.GetStringAsync("http://www.google.com");
                    var t2 = c.GetStringAsync("http://www.google.com");

                    Debug.WriteLine("get2: " + t2.Result);
                    Debug.WriteLine("get1: " + t1.Result);

                    Console.ReadLine();
                }
            }
            Console.WriteLine("ok");
            Console.ReadLine();
        }
    }
}
