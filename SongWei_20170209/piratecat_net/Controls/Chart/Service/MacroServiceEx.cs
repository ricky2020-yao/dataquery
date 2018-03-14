/*******************************************************************************************\
*                                                                                           *
* MacroServiceEx.cs -  Macro service functions, types, and definitions.                     *
*                                                                                           *
*               Version 1.00  ★★★                                                        *
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
    /// 提示方法
    /// </summary>
    public class CFunctionEx : CFunction
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="chart">行情控件</param>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="withParameters">是否有参数</param>
        public CFunctionEx(OwChart chart, CIndicator indicator, int id, String name)
        {
            m_chart = chart;
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
        }

        /// <summary>
        /// 行情控件
        /// </summary>
        private OwChart m_chart;

        /// <summary>
        /// 指标
        /// </summary>
        private CIndicator m_indicator;

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
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
        /// 是否最新数据
        /// </summary>
        /// <returns></returns>
        private bool IsNewData()
        {
            CTable dataSource = m_indicator.DataSource;
            return dataSource.RowsCount == m_indicator.Index + 1;
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
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
        /// 左滚
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public double ScrollLeft(CVariable var)
        {
            m_chart.Chart.ScrollLeft(1);
            m_chart.Chart.Update();
            m_chart.Native.Invalidate();
            return 1;
        }

        /// <summary>
        /// 右滚
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public double ScrollRight(CVariable var)
        {
            m_chart.Chart.ScrollRight(1);
            m_chart.Chart.Update();
            m_chart.Native.Invalidate();
            return 1;
        }

        /// <summary>
        /// 设置代码
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
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
        /// 切换周期
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public double SetCycle(CVariable var)
        {
            m_chart.ChangeCycle((int)m_indicator.GetValue(var.m_parameters[0]));
            return 1;
        }

        /// <summary>
        /// 设置布局
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
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
        /// 打开窗体
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
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
        /// 睡眠
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public double Sleep(CVariable var)
        {
            Thread.Sleep((int)m_indicator.GetValue(var.m_parameters[0]));
            return 1;
        }

        /// <summary>
        /// Windows下执行程序
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
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
        /// Windows下获取控件数值
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>数值</returns>
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
        /// Windows下的键盘事件
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
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
        /// Windows下的鼠标事件
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
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
        /// Windows下设置文字
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
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
        /// 缩小
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public double ZoomIn(CVariable var)
        {
            m_chart.Chart.ZoomIn();
            m_chart.Chart.Update();
            m_chart.Native.Invalidate();
            return 1;
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public double ZoomOut(CVariable var)
        {
            m_chart.Chart.ZoomOut();
            m_chart.Chart.Update();
            m_chart.Native.Invalidate();
            return 1;
        }
    }

    /// <summary>
    /// 宏定义服务扩展
    /// </summary>
    public class MacroServiceEx : MacroService
    {
        #region Lord 2016/10/17
        /// <summary>
        /// 创建宏定义服务扩展
        /// </summary>
        public MacroServiceEx()
        {
        }

        /// <summary>
        /// 宏集合
        /// </summary>
        private List<Macro> m_macros = new List<Macro>();

        /// <summary>
        /// 正在运行中的宏
        /// </summary>
        private Dictionary<String, String> m_runningMacros = new Dictionary<String, String>();

        private OwChart m_chart;

        /// <summary>
        /// 获取或设置股票控件
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
        /// 获取列表的请求ID
        /// </summary>
        public int GetListRequestID
        {
            get { return m_getListRequestID; }
        }

        private bool m_loaded;

        /// <summary>
        /// 获取或设置是否已加载
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        private int m_operatorRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 获取操作请求ID
        /// </summary>
        public int OperatorRequestID
        {
            get { return m_operatorRequestID; }
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void ChartInvoke(object sender, object args)
        {
            Macro macro = args as Macro;
            if (macro != null)
            {
                Run(macro);
            }
        }

        /// <summary>
        /// 销毁对象
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
        /// 根据ID获取宏
        /// </summary>
        /// <param name="layoutID">宏ID</param>
        /// <param name="layout">宏</param>
        /// <returns>状态</returns>
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
        /// 获取所有的宏
        /// </summary>
        /// <param name="macros">宏集合</param>
        /// <returns>状态</returns>
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
        /// 宏运行结束
        /// </summary>
        /// <param name="macro">宏</param>
        /// <returns>状态</returns>
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
        /// 宏运行中
        /// </summary>
        /// <param name="macro">宏</param>
        /// <returns>状态</returns>
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
        /// 宏开始运行
        /// </summary>
        /// <param name="macro">宏</param>
        /// <returns>状态</returns>
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
        /// 接收消息方法
        /// </summary>
        /// <param name="message">消息</param>
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
        /// 运行宏
        /// </summary>
        /// <param name="macro">宏</param>
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
        /// 运行宏
        /// </summary>
        /// <param name="args">参数</param>
        private void RunMacro(object args)
        {
            Macro macro = args as Macro;
            OnMacroRunning(macro);
        }
        #endregion
    }
}
