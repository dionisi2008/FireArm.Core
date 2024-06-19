using System;
using System.Collections.Generic;

namespace C2000_PP
{
    public enum ModbusFunctionCode
    {
        Чтение_значений_из_нескольких_регистров_флагов = 0x01,
        Чтение_значения_одного_дискретного_входа = 0x02,
        Чтение_значений_из_нескольких_регистров_хранения = 0x03,
        Чтение_значений_из_нескольких_входных_регистров = 0x04,
        Запись_значения_одного_флага = 0x05,
        Запись_значения_в_один_регистр_хранения = 0x06,
        Запись_значений_в_несколько_регистров_флагов = 0x0F,
        Запись_значений_в_несколько_регистров_хранения = 0x10
    }

    public class PacketData
    {
        private byte Address;
        private ModbusFunctionCode Function;
        private byte[] DataGet;
        private byte[] CRC;

        public PacketData(byte addressOfDevices, ModbusFunctionCode functionCode, byte[] data)
        {
            
            Address = addressOfDevices;
            Function = functionCode;
            DataGet = data;
            List<byte> tmp = new List<byte>();
            tmp.Add(Address);
            tmp.Add((byte)Function);
            tmp.AddRange(DataGet);
            CRC = CalculateCRC(tmp.ToArray());
        }

        public byte[] ПолучитьПакетВВидеБайт()
        {
            List<byte> tmp = new List<byte>();
            tmp.Add(Address);
            tmp.Add((byte)Function);
            tmp.AddRange(DataGet);
            tmp.AddRange(CRC);
            return tmp.ToArray();
        }

        private byte[] CalculateCRC(byte[] data)
        {
            ushort crc = 0xFFFF;

            for (int i = 0; i < data.Length; i++)
            {
                crc ^= data[i];

                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            byte[] crcBytes = new byte[2];
            crcBytes[0] = (byte)(crc & 0xFF);       // Младший байт
            crcBytes[1] = (byte)(crc >> 8);         // Старший байт

            return crcBytes;
        }
    }
}