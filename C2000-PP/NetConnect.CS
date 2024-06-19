using System.Net;
using System.Net.Sockets;


namespace C2000_PP
{
    public class NetConnect
    {
       
        protected string IP_address;
        protected int UDP_Port;
        protected byte AddressOfDevices;
        protected int Speed;
        protected UdpClient udpClient;

        public NetConnect(string IP_address, int UDP_Port, byte AddressOfDevices, int Speed)
        {

            this.IP_address = IP_address;
            this.UDP_Port = UDP_Port;
            this.AddressOfDevices = AddressOfDevices;
            this.Speed = Speed;
        }

        public NetConnect()
        {

        }

        protected byte[] SendData(PacketData data)
        {
            try
            {
                using (var udpClient = new UdpClient())
                {
                    udpClient.Connect(IP_address, UDP_Port);
                    udpClient.Send(data.ПолучитьПакетВВидеБайт(), data.ПолучитьПакетВВидеБайт().Length);

                    // Ждем 500 мс
                    System.Threading.Thread.Sleep(500);

                    // Проверяем наличие ответа
                    if (udpClient.Available > 0)
                    {
                        IPEndPoint remoteEndPoint = null;
                        byte[] response = udpClient.Receive(ref remoteEndPoint);
                        return response;
                    }
                    else
                    {
                        Console.WriteLine("No response received.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending data via UDP: " + ex.Message);
            }

            return null;
        }
    }
}