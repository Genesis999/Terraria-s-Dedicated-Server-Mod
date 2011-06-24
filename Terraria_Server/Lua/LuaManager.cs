using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;
using System.Reflection;
using Terraria_Server.Commands;

namespace Terraria_Server.LuaObjects
{
    class LuaManager
    {
        private static Lua _manager = null;
        public static Lua GetManager()
        {
            return _manager;
        }

        private static LuaFunctions _functions = null;
        public static LuaFunctions GetFunctions()
        {
            return _functions;
        }

        private static bool _initialised = false;
        public static bool IsInitialised()
        {
            return _initialised;
        }

        private static string _filename;

        public static void Initialise(string script_name)
        {
            try
            {
                Console.WriteLine("Initializing lua...");

                _manager = new Lua();
                _functions = new LuaFunctions();

                try
                {
                    _manager.RegisterFunction("PrintToConsole", _functions, _functions.GetType().GetMethod("PrintToConsole"));
                    _manager.RegisterFunction("PrintToChat", _functions, _functions.GetType().GetMethod("PrintToChat"));
                }
                catch
                {
                    Console.WriteLine("Oof. Registering functions shouldnt have failed.");
                }

                try
                {
                    _manager.DoFile(script_name);
                }
                catch
                {
                    Console.WriteLine("Error opening the script.");
                }
                _initialised = true;
                _filename = script_name;
            }
            catch
            {
                Console.WriteLine("Error initialising lua");
                _initialised = false;
            }
        }

        public static void ReloadScript(Sender sender)
        {
            if (sender is Player)
            {
                if (sender.isOp())
                {
                    try
                    {
                        _manager.DoFile(_filename);
                        sender.sendMessage("Reloaded current lua script.");
                        _initialised = true;
                    }
                    catch
                    {
                        sender.sendMessage("Failed to reload current lua script.");
                        _initialised = false;
                    }
                }
            }
            else
            {
                sender.sendMessage("You Cannot Perform That Action.", 255, 238f, 130f, 238f);
            }
        }

        public static void LoadScript(Sender sender, string file)
        {
            if (sender is Player)
            {
                if (sender.isOp())
                {
                    if (file.Length > 1 && file != null && file.Trim().Length > 0)
                    {
                        try
                        {
                            _manager.DoFile(file);
                            sender.sendMessage("Lua script loaded.");
                            _initialised = true;
                        }
                        catch
                        {
                            sender.sendMessage("Loading lua script failed.");
                            _initialised = false;
                        }
                    }
                    else
                    {
                        sender.sendMessage("Lua script not found");
                        _initialised = false;
                    }
                }
                else
                {
                    sender.sendMessage("You Cannot Perform That Action.", 255, 238f, 130f, 238f);
                }
            }
        }

        public static object[] RunLuaFunction(string func, object param = null)
        {
            try
            {
                if (param != null)
                {
                    object[] return_result = _manager.GetFunction(func).Call(param);
                    if ((return_result != null) && (return_result.Length > 0))
                    {
                        if ((string)return_result[0] == "#failed")
                        {
                            Console.WriteLine("Calling lua function: " + func + " failed.");
                            return null;
                        }
                        else
                        {
                            return return_result;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Call to function: " + func + " failed.");
                _initialised = false;
                return null;
            }
        }
    }
}
