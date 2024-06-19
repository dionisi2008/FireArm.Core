using System;
using System.Collections;
using C2000_PP;

public class Func_Буфер : C2000_PP_Info
{
    public Func_Буфер()
    {

    }

    public ushort Запрос_Номера_Самого_Нового_события()
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46160));
        writeData.AddRange(new byte[] { 0, 1 });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
        return ConvertByte_ushort_Reverse(response[3..5]);
    }
    public ushort Запрос_Номера_Самого_Старого_события()
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46161));
        writeData.AddRange(new byte[] { 0, 1 });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
        return ConvertByte_ushort_Reverse(response[3..5]);
    }

    public ushort Запрос_Количества_Непрочитоных_Собщений()
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46162));
        writeData.AddRange(new byte[] { 0, 1 });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
        return ConvertByte_ushort_Reverse(response[3..5]);
    }
    public bool Устоновка_признака_Собтие_Прочитано(int Numbe_Event)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46163));
        writeData.AddRange(ConvertInt_Byte_Reverse(Numbe_Event));
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return writeData.ToArray() == response[2..^2];
    }
    public bool Очитска_Буфера_Событий()
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46164));
        writeData.AddRange(new byte[] { 0, 0 });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return writeData.ToArray() == response[2..^2];
    }
    public Event_System Запрос_События()
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46264));
        writeData.Add(0);
        writeData.Add(14);
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return new Event_System(response[3..^2]);
    }
    public bool Устоновка_Номера_События_Для_Запроса_По_Номеру(ushort GetNumbeEvent)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46178));
        writeData.AddRange(ConvertInt_Byte_Reverse((int)GetNumbeEvent));
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return response[2..^2] == writeData.ToArray();
    }

    public Event_System Запрос_События_По_Номеру()
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46296));
        writeData.Add(0);
        writeData.Add(14);
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
        return new Event_System(response[3..^2]);
    }

}