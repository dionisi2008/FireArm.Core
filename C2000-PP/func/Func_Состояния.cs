using System;
using System.Collections;
using C2000_PP;

public class Func_Состояния : C2000_PP_Info
{
    public Func_Состояния(string GetIpadd, int GetPort, byte GetAdres)
    {
        this.AddressOfDevices = GetAdres;
        this.IP_address = GetIpadd;
        this.UDP_Port = GetPort;
    }


    public bool[] Запрос_состояния_группы_реле(int Start_Index_Releay, byte Count_Releay)
    {
        var writeData = new List<byte>(BitConverter.GetBytes((ushort)(10000 + (Start_Index_Releay - 1))));
        writeData.Add(0);
        writeData.Add(Count_Releay);
        byte[] response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_флагов, writeData.ToArray()))[3..^2];
        List<bool> tmp2 = new List<bool>();
        for (int shag = 0; shag <= response.Length - 1; shag++)
        {
            string tmp3 = Convert.ToString(response[shag]).PadLeft(8, '0');
            for (int shag2 = 0; shag2 <= 8; shag2++)
            {
                if (tmp3[shag2] == '1')
                {
                    tmp2.Add(true);
                }
                else
                {
                    tmp2.Add(false);
                }
            }
        }

        return tmp2.ToArray();
    }

    public void Команда_управления_группой_реле(int Start_Index, byte Count_Relay, bool[] Get_State)
    {
        var writeData = new List<byte>(BitConverter.GetBytes((ushort)(10000 + (Start_Index - 1))));
        writeData.Add(Count_Relay);
        writeData.Add(0);
        byte[] ByteStateRelay = ConvertBoolArrayToByteArray(Get_State);
        writeData.Add((byte)ByteStateRelay.Length);
        writeData.AddRange(ByteStateRelay);
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значений_в_несколько_регистров_флагов, writeData.ToArray()));

    }
    public void Команда_управления_Одним_реле(int Index_releay, bool State_Releay)
    {
        var writeData = new List<byte>(BitConverter.GetBytes((ushort)(10000 + (Index_releay - 1))));
        if (State_Releay)
        {
            writeData.AddRange(new byte[] { 255, 255 });
        }
        else
        {
            writeData.AddRange(new byte[] { 0, 0 });
        }
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_одного_флага, writeData.ToArray()));

    }

    public States[] ЗапросСостоянияЗоны(int Numbe_Zone)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(40000 + (Numbe_Zone - 1)));
        writeData.AddRange(new byte[] { 0, 1 });
        
        var response = base.SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
        
        return new States[]{(States)response[3], (States)response[4]};
    }
    public bool Команда_Устоновки_Состояния_Зоны(int Numbe_Zone, States_Zone Get_State_Zone)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(40000 + (Numbe_Zone - 1)));
        writeData.AddRange(new byte[] { 0, (byte)Get_State_Zone });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        if (!(response[2..^2].ToArray() == writeData.ToArray()))
        {
            byte[] kk = response[2..^2];
            //Console.WriteLine(BitConverter.ToString(response));
        }
        Console.WriteLine("Debug 34");
        Console.WriteLine(string.Join('-', response[2..^2]));
        Console.WriteLine(string.Join('-', writeData.ToArray()));
        for (int i = 0; i < 3; i++)
        {
            if (response[2..^2][i] != writeData[i])
            {
                return false;
            }
        }
        return true;
    }
    private bool Устоновка_Номера_Зоны_Для_Запроса_Расширенного_Состояния_Зоны(int Numbe_Zone)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46176));
        writeData.AddRange(ConvertInt_Byte_Reverse(Numbe_Zone));
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return response[2..^2] == writeData.ToArray();
    }

    public States[] Запрос_расшироенного_Состояния_Зоны_По_Устоновленному_Номеру(int Numbe_Zone, byte Size_Satetes)
    {
        States[] States_Out = null;
        if (Устоновка_Номера_Зоны_Для_Запроса_Расширенного_Состояния_Зоны(Numbe_Zone))
        {
            var writeData = new List<byte>(ConvertInt_Byte_Reverse(46192));
            writeData.Add(0);
            writeData.Add((byte)(Math.Round((double)Numbe_Zone / 2) + 1));
            var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
            ushort GetNumbeZone = BitConverter.ToUInt16(response[3..5]);
            States_Out = new States[response[(int)response[5]]];

            for (int shag = 0; shag <= States_Out.Length - 1; shag++)
            {
                States_Out[shag] = (States)States_Out[shag];
            }
        }

        return States_Out;
    }
    public States Запрос_Состояния_Раздела(int Numbe_Razdel)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(44096 + (Numbe_Razdel - 1)));
        writeData.AddRange(new byte[] { 0, 1 });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
        return (States)BitConverter.ToUInt16(response[3..^2]);
    }

    public bool Команда_Устоновки_Состояния_Раздела(int Numbe_Razdel, States_Zone Get_State_Razdel)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(44096 + (Numbe_Razdel - 1)));
        writeData.AddRange(new byte[] { 0, (byte)Get_State_Razdel });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return response[2..^2] == writeData.ToArray();
    }

    private bool Устоновка_Номера_Раздела_Для_Запроса_Расширенного_Состояния_Раздела(int Numbe_Razdel)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46177));
        writeData.AddRange(ConvertInt_Byte_Reverse(Numbe_Razdel));
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return response[2..^2] == writeData.ToArray();
    }

    public States[] Запрос_расшироенного_Состояния_Раздела_По_Устоновленному_Номеру(int Numbe_Razdel, byte Size_Satetes)
    {
        States[] States_Out = null;
        if (Устоновка_Номера_Раздела_Для_Запроса_Расширенного_Состояния_Раздела(Numbe_Razdel))
        {
            var writeData = new List<byte>(ConvertInt_Byte_Reverse(46200));
            writeData.Add(0);
            writeData.Add((byte)(Math.Round((double)Numbe_Razdel / 2) + 1));
            var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
            ushort GetNumbeZone = BitConverter.ToUInt16(response[3..5]);
            States_Out = new States[response[(int)response[5]]];

            for (int shag = 0; shag <= States_Out.Length - 1; shag++)
            {
                States_Out[shag] = (States)States_Out[shag];
            }
        }

        return States_Out;
    }

}



