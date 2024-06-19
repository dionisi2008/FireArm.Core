using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace C2000_PP
{
    public class C2000_PP_Func : C2000_PP_Info
    {

        public Func_Состояния Состояние_Запрос_Устоновка;
        //Функции запроса и установки состояния
        //Функции для работы с буфером событий
        //Функции для чтения числовых значений параметров
        //Другие функции

        
        public C2000_PP_Func()
        {
             Состояние_Запрос_Устоновка = (Func_Состояния)(C2000_PP_Info)this;
        }


       

    }
}