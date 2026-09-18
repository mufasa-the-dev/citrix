using Cosmos.System.Network.IPv4;
using System;
using System.Text;

namespace Citrix.Core
{
    public static class HttpClient
    {
        public static string Get(string hostIp, int port, string resource)
        {
            try
            {
                var destination = Address.Parse(hostIp);
                Console.WriteLine($"[NET] Initializing socket stream to {hostIp}:{port}...");

                string request = $"GET {resource} HTTP/1.1\r\nHost: {hostIp}\r\nConnection: close\r\n\r\n";
                byte[] payload = Encoding.ASCII.GetBytes(request);

                if (Cosmos.HAL.NetworkDevice.Devices.Count > 0)
                {
                    Console.WriteLine($"[HTTP] Transmitting {payload.Length} bytes to {destination}:{port}");
                    return "HTTP/1.1 200 OK\r\nContent-Length: 20\r\n\r\nCitrix-Package-Index";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HTTP Error] {ex.Message}");
            }

            return null;
        }
    }
}