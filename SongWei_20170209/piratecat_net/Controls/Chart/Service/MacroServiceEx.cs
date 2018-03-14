/*******************************************************************************************\
*                                                                                           *
* MacroServiceEx.cs -  Macro service functions, types, and definitions.                     *
*                                                                                           *
*               Version 1.00  ����                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.                    *
*               Created by Lord 2016/10/17.                                                  *
*                                                                                           *
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ��ʾ����
    /// </summary>
    public class CFunctionEx : CFunction
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="chart">����ؼ�</param>
        /// <param name="indicator">ָ��</param>
        /// <param name="id">ID</param>
        /// <param name="name">����</param>
        /// <param name="withParameters">�Ƿ��в���</param>
        public CFunctionEx(OwChart chart, CIndicator indicator, int id, String name)
        {
            m_chart = chart;
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
        }

        /// <summary>
        /// ����ؼ�
        /// </summary>
        private OwChart m_chart;

        /// <summary>
        /// ָ��
        /// </summary>
        private CIndicator m_indicator;

        /// <summary>
        /// ��ʾ
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public double Alert(CVariable var)
        {
            BarrageDiv barrageDiv = m_chart.BarrageDiv;
            Barrage barrage = new Barrage();
            barrage.Text = m_indicator.GetText(var.m_parameters[0]);
            barrage.Mode = 1;
            barrageDiv.AddBarrage(barrage);
            return 1;
        }

        /// <summary>
        /// �Ƿ���������
        /// </summary>
        /// <returns></returns>
        private bool IsNewData()
        {
            CTable dataSource = m_indicator.DataSource;
            return dataSource.RowsCount == m_indicator.Index + 1;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public override double OnCalculate(CVariable var)
        {
            switch (var.m_functionID)
            {
                case 1000:
                    return Alert(var);
                case 1001:
                    return SetCycle(var);
                case 1002:
                    return SetLayout(var);
                case 1003:
                    return SetCode(var);
                case 1004:
                    return ShowWindow(var);
                case 1005:
                    return Sleep(var);
                case 1006:
                    return ScrollLeft(var);
                case 1007:
                    return ScrollRight(var);
                case 1008:
                    return ZoomIn(var);
                case 1009:
                    return ZoomOut(var);
                case 1010:
                    return WinMouseEvent(var);
                case 1011:
                    return WinSetText(var);
                case 1012:
                    return WinExecute(var);
                case 1013:
                    return WinSendKey(var);
                case 1014:
                    return WinGetValue(var);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public double ScrollLeft(CVariable var)
        {
            m_chart.Chart.ScrollLeft(1);
            m_chart.Chart.Update();
            m_chart.Native.Invalidate();
            return 1;
        }

        /// <summary>
        /// �ҹ�
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public double ScrollRight(CVariable var)
        {
            m_chart.Chart.ScrollRight(1);
            m_chart.Chart.Update();
            m_chart.Native.Invalidate();
            return 1;
        }

        /// <summary>
        /// ���ô���
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public double SetCode(CVariable var)
        {
            Security security = new Security();
            if (DataCenter.SecurityService.GetSecurityByCode(m_indicator.GetText(var.m_parameters[0]), ref security))
            {
                m_chart.SearchSecurity(security);
            }
            return 1;
        }

        /// <summary>
        /// �л�����
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public double SetCycle(CVariable var)
        {
            m_chart.ChangeCycle((int)m_indicator.GetValue(var.m_parameters[0]));
            return 1;
        }

        /// <summary>
        /// ���ò���
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public double SetLayout(CVariable var)
        {
            IndicatorLayout indicatorLayout = new IndicatorLayout();
            if (DataCenter.IndicatorLayoutService.GetLayoutByName(m_indicator.GetText(var.m_parameters[0]), ref indicatorLayout))
            {
                m_chart.SetIndicatorLayout(indicatorLayout.m_layoutID, indicatorLayout.m_text);
            }
            return 1;
        }

        /// <summary>
        /// �򿪴���
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public double ShowWindow(CVariable var)
        {
            String window = m_indicator.GetText(var.m_parameters[0]);
            if (window == "CHATWINDOW")
            {
                m_chart.ShowChatWindow();
            }
            else if (window == "INDICATORLAYOUTLIST")
            {
                m_chart.ShowIndicatorLayoutList();
            }
            else if (window == "INDICATORLIST")
            {
                m_chart.ShowIndicatorList();
            }
            else if (window == "SANDBOXWINDOW")
            {
                m_chart.ShowSandBoxWindow();
            }
            else if (window == "SECURITYFILTERLIST")
            {
                m_chart.ShowSecurityFilterList();
            }
            else if (window == "USERSECURITYLIST")
            {
                m_chart.ShowUserSecurityList();
            }
            return 1;
        }

        /// <summary>
        /// ˯��
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        public double Sleep(CVariable var)
        {
            Thread.Sleep((int)m_indicator.GetValue(var.m_parameters[0]));
            return 1;
        }

        /// <summary>
        /// Windows��ִ�г���
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        public double WinExecute(CVariable var)
        {
            INativeBase native = m_chart.Native;
            WinformControlHostEx host = native.Host as WinformControlHostEx;
            if (host != null)
            {
                host.Execute(m_indicator.GetText(var.m_parameters[0]));
            }
            return 1;
        }

        /// <summary>
        /// Windows�»�ȡ�ؼ���ֵ
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>��ֵ</returns>
        public double WinGetValue(CVariable var)
        {
            double value = 0;
            INativeBase native = m_chart.Native;
            WinformControlHostEx host = native.Host as WinformControlHostEx;
            if (host != null)
            {
                String text = host.GetText();
                value = CStrA.ConvertStrToDouble(text);
            }
            return value;
        }

        /// <summary>
        /// Windows�µļ����¼�
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        public double WinSendKey(CVariable var)
        {
            INativeBase native = m_chart.Native;
            WinformControlHostEx host = native.Host as WinformControlHostEx;
            if (host != null)
            {
                SIZE displaySize = native.DisplaySize;
                int vlen = var.m_parameters.Length;
                String key = "";
                if (vlen >= 1)
                {
                    key = m_indicator.GetText(var.m_parameters[0]);
                }
                host.SendKey(key);
            }
            return 1;
        }

        /// <summary>
        /// Windows�µ�����¼�
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        public double WinMouseEvent(CVariable var)
        {
            INativeBase native = m_chart.Native;
            WinformControlHostEx host = native.Host as WinformControlHostEx;
            if (host != null)
            {
                SIZE displaySize = native.DisplaySize;
                int dx = 0, dy = 0, data = 0;
                int vlen = var.m_parameters.Length;
                String eventID = "";
                if (vlen >= 1)
                {
                    eventID = m_indicator.GetText(var.m_parameters[0]);
                }
                if (vlen >= 2)
                {
                    dx = (int)m_indicator.GetValue(var.m_parameters[1]);
                }
                if (vlen >= 3)
                {
                    dy = (int)m_indicator.GetValue(var.m_parameters[2]);
                }
                if (vlen >= 4)
                {
                    data = (int)m_indicator.GetValue(var.m_parameters[3]);
                }
                host.MouseEvent(eventID, dx, dy, data);
            }
            return 1;
        }

        /// <summary>
        /// Windows����������
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        public double WinSetText(CVariable var)
        {
            INativeBase native = m_chart.Native;
            WinformControlHostEx host = native.Host as WinformControlHostEx;
            if (host != null)
            {
                host.SetText(m_indicator.GetText(var.m_parameters[0]));
            }
            return 1;
        }

        /// <summary>
        /// ��С
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        public double ZoomIn(CVariable var)
        {
            m_chart.Chart.ZoomIn();
            m_chart.Chart.Update();
            m_chart.Native.Invalidate();
            return 1;
        }

        /// <summary>
        /// �Ŵ�
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        public double ZoomOut(CVariable var)
        {
            m_chart.Chart.ZoomOut();
            m_chart.Chart.Update();
            m_chart.Native.Invalidate();
            return 1;
        }
    }

    /// <summary>
    /// �궨�������չ
    /// </summary>
    public class MacroServiceEx : MacroService
    {
        #region Lord 2016/10/17
        /// <summary>
        /// �����궨�������չ
        /// </summary>
        public MacroServiceEx()
        {
        }

        /// <summary>
        /// �꼯��
        /// </summary>
        private List<Macro> m_macros = new List<Macro>();

        /// <summary>
        /// ���������еĺ�
        /// </summary>
        private Dictionary<String, String> m_runningMacros = new Dictionary<String, String>();

        private OwChart m_chart;

        /// <summary>
        /// ��ȡ�����ù�Ʊ�ؼ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set 
            {
                m_chart = value;
                m_chart.Chart.RegisterEvent(new ControlInvokeEvent(ChartInvoke), EVENTID.INVOKE);
            }
        }

        private int m_getListRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ȡ�б������ID
        /// </summary>
        public int GetListRequestID
        {
            get { return m_getListRequestID; }
        }

        private bool m_loaded;

        /// <summary>
        /// ��ȡ�������Ƿ��Ѽ���
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        private int m_operatorRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ȡ��������ID
        /// </summary>
        public int OperatorRequestID
        {
            get { return m_operatorRequestID; }
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="args">����</param>
        private void ChartInvoke(object sender, object args)
        {
            Macro macro = args as Macro;
            if (macro != null)
            {
                Run(macro);
            }
        }

        /// <summary>
        /// ���ٶ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_macros.Clear();
                lock (m_runningMacros)
                {
                    m_runningMacros.Clear();
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// ����ID��ȡ��
        /// </summary>
        /// <param name="layoutID">��ID</param>
        /// <param name="layout">��</param>
        /// <returns>״̬</returns>
        public bool GetMacroByID(String macroID, ref Macro macro)
        {
            int macrosSize = m_macros.Count;
            for (int i = 0; i < macrosSize; i++)
            {
                if (m_macros[i].m_macroID == macroID)
                {
                    macro = m_macros[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ȡ���еĺ�
        /// </summary>
        /// <param name="macros">�꼯��</param>
        /// <returns>״̬</returns>
        public bool GetMacros(List<Macro> macros)
        {
            int macrosSize = m_macros.Count;
            for (int i = 0; i < macrosSize; i++)
            {
                macros.Add(m_macros[i]);
            }
            return true;
        }

        /// <summary>
        /// �����н���
        /// </summary>
        /// <param name="macro">��</param>
        /// <returns>״̬</returns>
        public virtual int OnMacroEnd(Macro macro)
        {
            lock (m_runningMacros)
            {
                if (macro.m_macroID != null && macro.m_macroID.Length > 0)
                {
                    if (m_runningMacros.ContainsKey(macro.m_macroID))
                    {
                        m_runningMacros.Remove(macro.m_macroID);
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="macro">��</param>
        /// <returns>״̬</returns>
        public virtual int OnMacroRunning(Macro macro)
        {
            if (OnMacroStart(macro) == 0)
            {
                return 0;
            }
            if (macro.m_script.Length > 0)
            {
                int endIndex = m_chart.Index;
                ChartA ct = m_chart.Chart;
                CTable dataSource = ct.DataSource;
                CTable newDataSource = SecurityDataHelper.CreateDataSource(ct);
                int interval = macro.m_interval;
                CIndicator indicator = SecurityDataHelper.CreateIndicator(ct, newDataSource, "", "");
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1000, "ALERT"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1001, "SETCYCLE"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1002, "SETLAYOUT"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1003, "SETCODE"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1004, "SHOWWINDOW"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1005, "SLEEP"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1006, "SCROLLLEFT"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1007, "SCROLLRIGHT"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1008, "ZOOMIN"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1009, "ZOOMOUT"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1010, "WIN_MOUSEEVENT"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1011, "WIN_SETTEXT"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1012, "WIN_EXECUTE"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1013, "WIN_SENDKEY"));
                indicator.AddFunction(new CFunctionEx(m_chart, indicator, 1014, "WIN_GETVALUE"));
                indicator.Script = macro.m_script;
                int[] fields = new int[] { KeyFields.CLOSE_INDEX, KeyFields.HIGH_INDEX, KeyFields.LOW_INDEX, KeyFields.OPEN_INDEX, KeyFields.VOL_INDEX, KeyFields.AMOUNT_INDEX };
                int fieldsSize = fields.Length;
                int startIndex = endIndex - (interval - 1);
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                if (startIndex > endIndex)
                {
                    startIndex = endIndex;
                }
                if (macro.m_type == 0 || macro.m_type == 2 || endIndex == -1)
                {
                    double date = 0;
                    newDataSource.Set(date, KeyFields.VOL_INDEX, 0);
                    int index = newDataSource.GetRowIndex(date);
                    for (int j = 0; j < fieldsSize; j++)
                    {
                        if (fields[j] != KeyFields.VOL_INDEX)
                        {
                            newDataSource.Set2(index, fields[j], 0);
                        }
                    }
                }
                else
                {
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        double date = dataSource.GetXValue(i);
                        newDataSource.Set(date, KeyFields.VOL_INDEX, dataSource.Get2(i, KeyFields.VOL_INDEX));
                        int index = newDataSource.GetRowIndex(date);
                        for (int j = 0; j < fieldsSize; j++)
                        {
                            if (fields[j] != KeyFields.VOL_INDEX)
                            {
                                newDataSource.Set2(index, fields[j], dataSource.Get2(i, fields[j]));
                            }
                        }
                    }
                }
                if (macro.m_type == 1 || macro.m_type == 3)
                {
                    indicator.OnCalculate(0);
                }
                else
                {
                    for (int i = 0; i < interval; i++)
                    {
                        indicator.OnCalculate(0);
                    }
                }
                indicator.Clear();
                indicator.Dispose();
                newDataSource.Dispose();
            }
            OnMacroEnd(macro);
            return 1;
        }

        /// <summary>
        /// �꿪ʼ����
        /// </summary>
        /// <param name="macro">��</param>
        /// <returns>״̬</returns>
        public virtual int OnMacroStart(Macro macro)
        {
            int state = 1;
            lock (m_runningMacros)
            {
                if (macro.m_macroID != null && macro.m_macroID.Length > 0)
                {
                    if (m_runningMacros.ContainsKey(macro.m_macroID))
                    {
                        state = 0;
                    }
                    else
                    {
                        m_runningMacros[macro.m_macroID] = "";
                    }
                }
            }
            return state;
        }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            if (message.m_functionID != FUNCTIONID_MACRO_GETMACROS)
            {
                message.m_requestID = m_operatorRequestID;
            }
            List<Macro> macros = new List<Macro>();
            GetMacros(macros, message.m_body, message.m_bodyLength);
            int macrosSize = macros.Count;
            switch (message.m_functionID)
            {
                case FUNCTIONID_MACRO_GETMACROS:
                    {
                        m_macros = macros;
                        m_loaded = true;
                        break;
                    }
                case FUNCTIONID_MACRO_ADDMACROS:
                    {
                        bool add = false;
                        for (int i = 0; i < macrosSize; i++)
                        {
                             Macro macro = null;
                             if (!GetMacroByID(macros[i].m_macroID, ref macro))
                             {
                                 m_macros.Add(macros[i]);
                                 add = true;
                             }
                        }
                        if (!add)
                        {
                            return;
                        }
                        break;
                    }
                case FUNCTIONID_MACRO_DELETEMACROS:
                    {
                        for (int i = 0; i < macrosSize; i++)
                        {
                            Macro macro = null;
                            if (GetMacroByID(macros[i].m_macroID, ref macro))
                            {
                                m_macros.Remove(macro);
                            }
                        }
                        break;
                    }
                case FUNCTIONID_MACRO_EXECUTEMACROS:
                    {
                        for (int i = 0; i < macrosSize; i++)
                        {
                            Macro macro = null;
                            if (GetMacroByID(macros[i].m_macroID, ref macro))
                            {
                                m_chart.Chart.BeginInvoke(macro);
                            }
                            else
                            {
                                m_chart.Chart.BeginInvoke(macros[i]);
                            }
                        }
                        break;
                    }
                case FUNCTIONID_MACRO_UPDATEMACROS:
                    {
                        for (int i = 0; i < macrosSize; i++)
                        {
                            Macro updateMacro = macros[i];
                            int curMacrosSize = m_macros.Count;
                            for (int j = 0; j < curMacrosSize; j++)
                            {
                                Macro macro = m_macros[j];
                                if (macro.m_macroID == updateMacro.m_macroID)
                                {
                                    m_macros[j] = updateMacro;
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            base.OnReceive(message);
        }

        /// <summary>
        /// ���к�
        /// </summary>
        /// <param name="macro">��</param>
        public void Run(Macro macro)
        {
            if (macro.m_type > 2)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(RunMacro));
                thread.IsBackground = true;
                thread.Start(macro);
            }
            else
            {
                RunMacro(macro);
            }
        }

        /// <summary>
        /// ���к�
        /// </summary>
        /// <param name="args">����</param>
        private void RunMacro(object args)
        {
            Macro macro = args as Macro;
            OnMacroRunning(macro);
        }
        #endregion
    }
}
