


using System;
using System.Net;
using System.Collections;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Threading.Tasks.Dataflow;
using System.Dynamic;
using System.Net.Sockets;

namespace C2000_PP
{
    public class ApiServer
    {
        protected HttpListener WebObjectApi;
        protected C2000_PP Device;
        public ApiServer(int GetPort)
        {
            this.WebObjectApi = new HttpListener();


            WebObjectApi.Prefixes.Add("http://*:" + GetPort + "/");
            WebObjectApi.Start();

            while (WebObjectApi.IsListening)
            {
                var Context = WebObjectApi.GetContextAsync().Result;
                WorkerContext(Context);
                Context.Response.Close();
            }

        }

        public async void WorkerContext(HttpListenerContext GetContext)
        {
            if (GetContext.Request.IsWebSocketRequest == true)
            {
                var tim = await GetContext.AcceptWebSocketAsync("test");
                WorkerWS(tim.WebSocket);
            }
            else
            {
                if (GetContext.Request.HttpMethod == "POST")
                {
                    sortPostDataAndForward(GetContext);
                }
                else
                {
                    if (GetContext.Request.RawUrl.ToString() == "/")
                    {
                        this.DisplaySystemState(GetContext);
                    }
                    else
                    {
                        GetContext.Response.OutputStream.Write(new ReadOnlySpan<byte>(File.ReadAllBytes(@".\WEB\test.html")));
                    }
                }
            }


        }

        public async void WorkerWS(System.Net.WebSockets.WebSocket GetWS)
        {
            ArraySegment<byte> ReadDataByte = new ArraySegment<byte>();
            GetWS.ReceiveAsync(ReadDataByte, CancellationToken.None).Wait();
            List<string> RequestServer = new List<string>(Encoding.UTF8.GetString(ReadDataByte.ToArray()).Split('\n'));
            Console.WriteLine(string.Join('\n', RequestServer.ToArray()));
        }
        public void DisplaySystemState(HttpListenerContext GetContext)
        {
            List<string> outStrings = new List<string>();
            outStrings.Add("State start Server Web API: " + WebObjectApi.IsListening);
            outStrings.Add("State Create Deviec Web API: " + (Device != null));
            byte[] DataWrite = Encoding.UTF8.GetBytes(string.Join("\n\r", outStrings.ToArray()));
            GetContext.Response.OutputStream.Write(new ReadOnlySpan<byte>(DataWrite));
        }

        public void sortPostDataAndForward(HttpListenerContext GetContext)
        {
            List<string> ReadDataUser = new List<string>(new System.IO.StreamReader(GetContext.Request.InputStream).ReadToEnd().Split(' '));
            if (ReadDataUser.Count >= 2)
            {
                switch (ReadDataUser[0])
                {
                    case "initDeviceInteraction":
                        initDeviceInteraction(GetContext, ReadDataUser[1..]);
                        break;
                    case "RequestZoneState":
                        RequestZoneState(GetContext, ReadDataUser[1..]);
                        break;
                    case "SetZoneStateCommand":
                        SetZoneStateCommand(GetContext, ReadDataUser[1..]);
                        break;
                    default:
                        Console.WriteLine(string.Join(',', ReadDataUser));
                        break;
                }
            }

        }

        public void SetZoneStateCommand(HttpListenerContext GetContext, List<string> GetZapros)
        {
            int Zone;
            int state;
            if (int.TryParse(GetZapros[0], out Zone) && int.TryParse(GetZapros[1], out state))
            {
                sendMessage(GetContext, Device.Функции_Запроса_И_Устоновки_Состояния.Команда_Устоновки_Состояния_Зоны(Zone, (C2000_PP_Info.States_Zone)state).ToString());
            }
            else
            {
                string ErrorMessage = "An error occurred in the initDeviceInteraction function! Reason: Incorrect input data format. Expected data: 'Number'. Received data: " + GetZapros[0];
                sendMessage(GetContext, ErrorMessage);
            }

        }

        public void RequestZoneState(HttpListenerContext GetContext, List<string> GetZapros)
        {
            int Zone;
            if (int.TryParse(GetZapros[0], out Zone))
            {
                var states = Device.Функции_Запроса_И_Устоновки_Состояния.ЗапросСостоянияЗоны(Zone);
                List<string> ListStates = new List<string>();
                for (int shag = 0; shag <= states.Length - 1; shag++)
                {
                    ListStates.Add(states[shag].ToString());
                }
                sendMessage(GetContext, string.Join('\n', ListStates.ToArray()));
            }
            else
            {
                string ErrorMessage = "An error occurred in the initDeviceInteraction function! Reason: Incorrect input data format. Expected data: 'Number'. Received data: " + GetZapros[0];
                sendMessage(GetContext, ErrorMessage);
                //Device.Функции_Запроса_И_Устоновки_Состояния.Команда_Устоновки_Состояния_Зоны
            }

        }

        public void initDeviceInteraction(HttpListenerContext GetContext, List<string> Zapros)
        {
            int PortUDP;
            byte DeviceAddress;
            int Speed;
            if (int.TryParse(Zapros[1], out PortUDP) && byte.TryParse(Zapros[2], out DeviceAddress) && int.TryParse(Zapros[3], out Speed))
            {
                Device = new C2000_PP(Zapros[0], PortUDP, DeviceAddress, Speed);
                sendMessage(GetContext, Device.Запрос_типа_и_версии_прибора());
            }
            else
            {
                string ErrorMessage = "An error occurred in the initDeviceInteraction function! Reason: Incorrect input data format. Expected data: 'IP(String) PortUDP(Number) DeviceAddress(Number) Speed(Number)'. Received data: " + string.Join(' ', Zapros);
                sendMessage(GetContext, ErrorMessage);
            }
        }

        public void sendMessage(HttpListenerContext GetContext, string Message)
        {
            byte[] DataWrite = Encoding.UTF8.GetBytes(Message);
            GetContext.Response.OutputStream.Write(new ReadOnlySpan<byte>(DataWrite));
        }

    }

}