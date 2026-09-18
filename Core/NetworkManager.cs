// Feature to fix! If you have problem with fixing it, just wait for next update.

using Cosmos.HAL;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using System;

namespace Citrix.Core
{
    public static class NetworkManager
    {
        private static bool isConfigured = false;

        public static void Initialize()
        {
            try
            {
                Console.WriteLine("Searching for network adapters...");

                if (NetworkDevice.Devices.Count == 0)
                {
                    Console.WriteLine("Network Error: No compatible AMD PCNet II adapter found.");
                    Console.WriteLine("VMware is currently presenting an Intel e1000 adapter.");
                    Console.WriteLine("Change Network Adapter type in VMX or use QEMU/Bochs runner.");
                    return;
                }

                var card = NetworkDevice.Devices[0];
                Console.WriteLine($"Adapter detected: {card.Name} [{card.MACAddress}]");

                var ip = new Address(192, 168, 192, 150);
                var mask = new Address(255, 255, 255, 0);
                var gw = new Address(192, 168, 192, 2);

                Console.WriteLine("Applying network configuration...");
                IPConfig.Enable(card, ip, mask, gw);
                isConfigured = true;

                Console.WriteLine($"Network ready! Assigned IP: {ip} via Gateway {gw}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Network Error: {ex.Message}");
            }
        }

        public static void DisplayInterfaceDetails()
        {
            if (NetworkDevice.Devices.Count == 0)
            {
                Console.WriteLine("No network interfaces available.");
                return;
            }

            for (int i = 0; i < NetworkDevice.Devices.Count; i++)
            {
                var card = NetworkDevice.Devices[i];
                Console.WriteLine($"eth{i}: flags=4163<UP,BROADCAST,RUNNING,MULTICAST>");
                Console.WriteLine($"        hardware: {card.Name}");
                Console.WriteLine($"        ether: {card.MACAddress}");

                if (isConfigured && NetworkConfiguration.CurrentAddress != null)
                {
                    Console.WriteLine($"        inet: {NetworkConfiguration.CurrentAddress}");
                }
                else
                {
                    Console.WriteLine("        inet: disconnected");
                }
                Console.WriteLine();
            }
        }

        public static void PingHost(string destinationIp)
        {
            if (!isConfigured || NetworkConfiguration.CurrentAddress == null)
            {
                Console.WriteLine("Network interface is offline. Run 'net' first.");
                return;
            }

            Address targetAddress;
            try
            {
                targetAddress = Address.Parse(destinationIp);
            }
            catch
            {
                Console.WriteLine("Invalid IP address format.");
                return;
            }

            Console.WriteLine($"PING {destinationIp} 56(84) bytes of data.");

            try
            {
                using (var icmp = new ICMPClient())
                {
                    icmp.Connect(targetAddress);

                    int received = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        icmp.SendEcho();

                        var source = new Cosmos.System.Network.IPv4.EndPoint(Address.Zero, 0);
                        int timeout = 2000;

                        int result = icmp.Receive(ref source, timeout);

                        if (result >= 0 && source.Address != null && source.Address.ToString() != "0.0.0.0")
                        {
                            received++;
                            Console.WriteLine($"64 bytes from {source.Address}: icmp_seq={i + 1} ttl=64");
                        }
                        else
                        {
                            Console.WriteLine($"Request timeout for icmp_seq {i + 1}");
                        }
                    }

                    Console.WriteLine($"--- {destinationIp} ping statistics ---");
                    Console.WriteLine($"4 packets transmitted, {received} received.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ping Exception: {ex.Message}");
            }
        }
    }
}