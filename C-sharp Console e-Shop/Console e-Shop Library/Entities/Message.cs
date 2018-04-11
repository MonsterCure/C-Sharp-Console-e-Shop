using System;

using static System.Console;  //C# 6.0 feature: using static

namespace Console_e_Shop_Library.Entities
{
    public static class Message
    {
        public static string textDivider = "======================================================================================================================";

        #region Logo printing method
        public static void Logo()
        {
            Clear();
            string shopName = "                              ___ _  _ ____                ____    ____ _  _ ____ ___ \n                               |  |__| |___                |___ __ [__  |__| |  | |__]\n                               |  |  | |___                |___    ___] |  | |__| |   \n                                                                                      ";
            Write($"{textDivider}\n{shopName}\n{textDivider}\n\n");
        }
        #endregion

        #region Message printing method
        public static void Print(string message, ConsoleColor color)
        {
            ForegroundColor = color;
            WriteLine($"\n{message}\n");
            ResetColor();
            WriteLine(textDivider);
        }
        #endregion
    }
}
