using System;
using System.Collections.Generic;
using System.Text;

namespace KLine
{
    public class MacroService : BaseService
    {
        public const int FUNCTIONID_MACRO_ADDMACROS = 0;
        public const int FUNCTIONID_MACRO_DELETEMACROS = 1;
        public const int FUNCTIONID_MACRO_EXECUTEMACROS = 2;
        public const int FUNCTIONID_MACRO_GETMACROS = 3;
        public const int FUNCTIONID_MACRO_UPDATEMACROS = 4;
        public const int FUNCTIONID_MACRO_UPDATEORDERNUM = 5;
        private int m_socketID = 0;
        public const int SERVICEID_MACRO = 9;

        public MacroService()
        {
            base.ServiceID = SERVICEID_MACRO;
        }

        public int AddMacro(int requestID, Macro macro)
        {
            List<Macro> macros = new List<Macro>();
            macros.Add(macro);
            int num = this.Send(0, requestID, macros);
            macros.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int DeleteMacro(int requestID, Macro macro)
        {
            List<Macro> macros = new List<Macro>();
            macros.Add(macro);
            int num = this.Send(1, requestID, macros);
            macros.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int ExecuteMacro(int requestID, Macro macro)
        {
            List<Macro> macros = new List<Macro>();
            macros.Add(macro);
            int num = this.Send(2, requestID, macros);
            macros.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int GetMacros(int requestID, Macro macro)
        {
            List<Macro> macros = new List<Macro>();
            macros.Add(macro);
            int num = this.Send(3, requestID, macros);
            macros.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public static int GetMacros(List<Macro> macros, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            int num = binary.ReadInt();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    Macro item = new Macro();
                    item.m_macroID = binary.ReadString();
                    item.m_userID = binary.ReadInt();
                    item.m_name = binary.ReadString();
                    item.m_description = binary.ReadString();
                    item.m_icon = binary.ReadString();
                    item.m_script = binary.ReadString();
                    item.m_type = binary.ReadInt();
                    item.m_interval = binary.ReadInt();
                    item.m_cd = binary.ReadInt();
                    item.m_orderNum = binary.ReadInt();
                    macros.Add(item);
                }
            }
            binary.Close();
            return 1;
        }

        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            this.SendToListener(message);
        }

        public int Send(int functionID, int requestID, List<Macro> macros)
        {
            Binary binary = new Binary();
            int count = macros.Count;
            binary.WriteInt(count);
            for (int i = 0; i < count; i++)
            {
                Macro macro = macros[i];
                binary.WriteString(macro.m_macroID);
                binary.WriteInt(macro.m_userID);
                binary.WriteString(macro.m_name);
                binary.WriteString(macro.m_description);
                binary.WriteString(macro.m_icon);
                binary.WriteString(macro.m_script);
                binary.WriteInt(macro.m_type);
                binary.WriteInt(macro.m_interval);
                binary.WriteInt(macro.m_cd);
                binary.WriteInt(macro.m_orderNum);
            }
            byte[] bytes = binary.GetBytes();
            int num3 = this.Send(new CMessage(base.GroupID, base.ServiceID, functionID, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return num3;
        }

        public int UpdateMacro(int requestID, Macro macro)
        {
            List<Macro> macros = new List<Macro>();
            macros.Add(macro);
            int num = this.Send(4, requestID, macros);
            macros.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int UpdateOrderNum(int requestID, int userID, List<string> ids)
        {
            string str = "";
            int count = ids.Count;
            for (int i = 0; i < count; i++)
            {
                str = str + ids[i];
                if (i != (count - 1))
                {
                    str = str + ",";
                }
            }
            Macro macro = new Macro();
            macro.m_macroID = str;
            macro.m_userID = userID;
            List<Macro> macros = new List<Macro>();
            macros.Add(macro);
            int num3 = this.Send(5, requestID, macros);
            macros.Clear();
            return ((num3 > 0) ? 1 : 0);
        }

        public int SocketID
        {
            get
            {
                return this.m_socketID;
            }
            set
            {
                this.m_socketID = value;
            }
        }
    }
}
