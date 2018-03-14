using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Threading;

namespace KLine
{
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
                //indicator.AddFunction(new CFunctionEx(indicator, 1000, "ALERT", m_mainFrame-));
                //indicator.AddFunction(new CFunctionEx(indicator, 1001, "SETCYCLE", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1002, "SETLAYOUT", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1003, "SETCODE", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1004, "SHOWWINDOW", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1005, "SLEEP", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1006, "SCROLLLEFT", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1007, "SCROLLRIGHT", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1008, "ZOOMIN", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1009, "ZOOMOUT", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1010, "WIN_MOUSEEVENT", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1011, "WIN_SETTEXT", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1012, "WIN_EXECUTE", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1013, "WIN_SENDKEY", m_chart));
                //indicator.AddFunction(new CFunctionEx(indicator, 1014, "WIN_GETVALUE", m_chart));
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
