using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using C2000_PP;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

           // C2000_PP.C2000_PP u = new C2000_PP.C2000_PP("10.1.3.250", 40005, 1, 9600);
            C2000_PP.ApiServer Dev6 = new ApiServer(8085);



            // C2000_PP.C2000_PP u = new C2000_PP.C2000_PP("10.1.3.250", 40005, 1, 9600);
            // u.Функции_Запроса_И_Устоновки_Состояния.Команда_Устоновки_Состояния_Зоны(6, C2000_PP_Info.States_Zone.Взятие_ШС);
            // Console.WriteLine(u.Функции_Запроса_И_Устоновки_Состояния.ЗапросСостоянияЗоны(6)[0]);
            //System.Console.WriteLine(u.Функции_Запроса_И_Устоновки_Состояния.Запрос_типа_и_версии_прибора());
            // var io = u.Функции_Запроса_И_Устоновки_Состояния.Запрос_расшироенного_Состояния_Зоны_По_Устоновленному_Номеру(6, 8);
            // System.Console.WriteLine();


        }
    }
}