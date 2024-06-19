using System;
using System.Collections;
using C2000_PP;

public class Func_Числа_Параметры : C2000_PP_Info
{
    public Func_Числа_Параметры()
    {

    }

    public bool Устоновка_Номера_Зоны_Для_Запроса_Температуры_Или_Влажности(ushort Get_Zone)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46179));
        writeData.AddRange(ConvertInt_Byte_Reverse((int)Get_Zone));
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return writeData.ToArray() == response[2..^2];
    }

    public short Запрос_Числового_Значения_Температуры_Или_Влажности(ushort Get_Zone)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46238));
        writeData.AddRange(new byte[] { 0, 1 });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
        return ConvertByte_short_Reverse(response[3..^2]);
    }
    public bool Устоновка_Номера_Зоны_Для_Запроса_Счётчика(ushort Get_Zone)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46180));
        writeData.AddRange(ConvertInt_Byte_Reverse((int)Get_Zone));
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Запись_значения_в_один_регистр_хранения, writeData.ToArray()));
        return writeData.ToArray() == response[2..^2];
    }

    public short Запрос_Числа(ushort Get_Zone)
    {
        var writeData = new List<byte>(ConvertInt_Byte_Reverse(46238));
        writeData.AddRange(new byte[] { 0, 1 });
        var response = SendData(new PacketData(AddressOfDevices, ModbusFunctionCode.Чтение_значений_из_нескольких_регистров_хранения, writeData.ToArray()));
        return ConvertByte_short_Reverse(response[3..^2]);
    }

}