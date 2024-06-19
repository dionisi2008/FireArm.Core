using System;

namespace C2000_PP
{
    public class C2000_PP : C2000_PP_Info
    {
        public Func_Состояния Функции_Запроса_И_Устоновки_Состояния;
        public C2000_PP(string IP_address, int UDP_Port, byte AddressOfDevices, int Speed) : base(IP_address, UDP_Port, AddressOfDevices, Speed)
        {
            Функции_Запроса_И_Устоновки_Состояния = new Func_Состояния(this.IP_address, this.UDP_Port, this.AddressOfDevices);
        }


    }
}