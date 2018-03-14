/*****************************************************************************\
*                                                                             *
* ParametersWindow.cs -  Parameters window functions, types, and definitions. *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/6/10.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;

namespace KLine
{
    /// <summary>
    /// 参数设置窗体
    /// </summary>
    public class ParametersWindow : WindowXmlEx
    {
        #region Lord 2016/6/10
        /// <summary>
        /// 创建聊天窗体
        /// </summary>
        /// <param name="native">方法库</param>
        /// <param name="indicator">指标</param>
        public ParametersWindow(INativeBase native, CIndicator indicator)
        {
            m_indicator = indicator;
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\ParametersWindow.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowParameters") as WindowEx;
            GetParameters();
            //注册点击事件
            RegisterEvents(m_window);
        }

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        private OwChart m_chart;

        /// <summary>
        /// 获取或设置行情控件
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private CIndicator m_indicator;

        /// <summary>
        /// 获取或设置指标
        /// </summary>
        public CIndicator Indicator
        {
            get { return m_indicator; }
        }

        private WindowEx m_window;

        /// <summary>
        /// 获取或设置窗体
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name=="btnClose" || name == "btnSubmit" || name == "btnCancel")
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public override void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_window != null)
                {
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParameters()
        {
            if (m_indicator != null)
            {
                m_window.Text = "参数设置(" + m_indicator.Name + ")";
                IndicatorData indicatorData = m_indicator.Tag as IndicatorData;
                String[] strs = indicatorData.m_parameters.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                int strsSize = strs.Length;
                //依此创建控件
                int addHeight = 0;
                for (int i = 0; i < strsSize; i++)
                {
                    String str = strs[i];
                    String[] strs2 = str.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    String name = strs2[0];
                    String value = strs2[3];
                    //创建标签
                    LabelA label = new LabelA();
                    POINT location = new POINT(50, addHeight + 40);
                    label.Location = location;
                    label.Text = name;
                    m_window.AddControl(label);
                    //创建数值控件
                    SpinA spin = new SpinA();
                    location.x = 130;
                    location.y = addHeight + 40;
                    spin.Location = location;
                    spin.Maximum = 10000000;
                    spin.Value = CStrA.ConvertStrToDouble(value);
                    m_window.AddControl(spin);
                    addHeight += 30;
                }
                m_window.Height += addHeight;
                //调整按钮的位置
                ButtonA cancelButton = GetButton("btnCancel");
                ButtonA submitButton = GetButton("btnSubmit");
                cancelButton.Top += addHeight;
                submitButton.Top += addHeight;
            }
        }

        /// <summary>
        /// 数值输入变化方法
        /// </summary>
        /// <param name="spinA">数值控件</param>
        public void OnSpinTextChanged(SpinA spin)
        {
            SetParameters();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);
            ControlEvent spinInputChangedEvent = new ControlEvent(SpinTextChanged);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ButtonA button = controls[i] as ButtonA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                SpinA spin = controls[i] as SpinA;
                if (spin != null)
                {
                    spin.RegisterEvent(spinInputChangedEvent, EVENTID.TEXTCHANGED);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        private void SetParameters()
        {
            List<ControlA> controls = m_window.GetControls();
            List<LabelA> labels = new List<LabelA>();
            List<SpinA> spins = new List<SpinA>();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA control = controls[i];
                LabelA label = control as LabelA;
                SpinA spin = control as SpinA;
                if (label != null)
                {
                    labels.Add(label);
                }
                else if (spin != null)
                {
                    spins.Add(spin);
                }
            }
            int labelsSize = labels.Count;
            String newParameters = "";
            for (int i = 0; i < labelsSize; i++)
            {
                newParameters += labels[i].Text + ",0,0," + spins[i].Value.ToString();
                newParameters += ";";
            }
            labels.Clear();
            spins.Clear();
            m_indicator.Clear();
            m_indicator.SetSourceField(KeyFields.CLOSE, KeyFields.CLOSE_INDEX);
            m_indicator.SetSourceField(KeyFields.HIGH, KeyFields.HIGH_INDEX);
            m_indicator.SetSourceField(KeyFields.LOW, KeyFields.LOW_INDEX);
            m_indicator.SetSourceField(KeyFields.OPEN, KeyFields.OPEN_INDEX);
            m_indicator.SetSourceField(KeyFields.VOL, KeyFields.VOL_INDEX);
            m_indicator.SetSourceField(KeyFields.AMOUNT, KeyFields.AMOUNT_INDEX);
            m_indicator.SetSourceField(KeyFields.CLOSE.Substring(0, 1), KeyFields.CLOSE_INDEX);
            m_indicator.SetSourceField(KeyFields.HIGH.Substring(0, 1), KeyFields.HIGH_INDEX);
            m_indicator.SetSourceField(KeyFields.LOW.Substring(0, 1), KeyFields.LOW_INDEX);
            m_indicator.SetSourceField(KeyFields.OPEN.Substring(0, 1), KeyFields.OPEN_INDEX);
            m_indicator.SetSourceField(KeyFields.VOL.Substring(0, 1), KeyFields.VOL_INDEX);
            m_indicator.SetSourceField(KeyFields.AMOUNT.Substring(0, 1), KeyFields.AMOUNT_INDEX);
            IndicatorData indicatorData = m_indicator.Tag as IndicatorData;
            indicatorData.m_parameters = newParameters;
            String constValue = "";
            if (newParameters != null && newParameters.Length > 0)
            {
                String[] strs = newParameters.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                int strsSize = strs.Length;
                for (int i = 0; i < strsSize; i++)
                {
                    String str = strs[i];
                    String[] strs2 = str.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    constValue += "const " + strs2[0] + ":" + strs2[3] + ";";
                }
            }
            m_indicator.Script = constValue + indicatorData.m_script;
            m_indicator.OnCalculate(0);
            if (m_indicator.AttachVScale == AttachVScale.Left)
            {
                //m_indicator.Div.TitleBar.Text = m_indicator.Title;
            }
            m_chart.Chart.Update();
            m_chart.Chart.Invalidate();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public override void Show()
        {
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(false);
            m_window.Invalidate();
        }

        /// <summary>
        /// 文本框输入值变化
        /// </summary>
        /// <param name="sender">控件</param>
        private void SpinTextChanged(object sender)
        {
            SpinA spin = sender as SpinA;
            OnSpinTextChanged(spin);
        }
        #endregion
    }
}
