using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terraria_Server.LuaObjects
{
    class LuaFunctions
    {
        public void PrintToConsole(string s)
        {
            Console.WriteLine(s);
        }

        public void PrintToChat(string msg, int client = -1)
        {
            NetMessage.SendData(0x19, client, -1, msg);
        }
    }
}
