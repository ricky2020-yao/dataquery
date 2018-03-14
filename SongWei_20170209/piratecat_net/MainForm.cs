using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using OwLib;

namespace piratecat
{
    /// <summary>
    /// OWCHART示例控件
    /// </summary>
    public partial class MainForm : Form
    {
        #region Lord 2012/7/4
        /// <summary>
        ///  创建图形控件
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            m_chart = new OwChart();
            m_chart.CreateNative();
            m_native = m_chart.Native;
            m_native.Paint = new GdiPlusPaintEx();
            m_host = new WinformControlHostEx();
            m_host.Native = m_native;
            m_native.Host = m_host;
            m_host.HWnd = Handle;
            m_native.AllowScaleSize = true;
            m_native.DisplaySize = new SIZE(ClientSize.Width, ClientSize.Height);
            m_chart.ResetScaleSize(GetClientSize());
            Invalidate();
            m_chart.Load(DataCenter.GetAppPath() + "\\config\\MainFrame.xml");
            m_native.Update();
            //m_chart.ShowLoginWindow();
        }

        /// <summary>
        /// 行情系统
        /// </summary>
        private OwChart m_chart;

        /// <summary>
        /// 控件管理器
        /// </summary>
        private WinformControlHost m_host; 

        /// <summary>
        /// 控件库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 获取客户端尺寸
        /// </summary>
        /// <returns>客户端尺寸</returns>
        public SIZE GetClientSize()
        {
            return new SIZE(ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            m_chart.Exit();
            base.OnFormClosing(e);
        }

        /// <summary>
        /// 尺寸改变方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (m_host != null)
            {
                m_chart.ResetScaleSize(GetClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标滚动方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (m_host.IsKeyPress(0x11))
            {
                double scaleFactor = m_chart.ScaleFactor;
                if (e.Delta > 0)
                {
                    if (scaleFactor > 0.2)
                    {
                        scaleFactor -= 0.1;
                    }
                }
                else if (e.Delta < 0)
                {
                    if (scaleFactor < 10)
                    {
                        scaleFactor += 0.1;
                    }
                }
                m_chart.ScaleFactor = scaleFactor;
                m_chart.ResetScaleSize(GetClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// 消息监听
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x100 || m.Msg == 260)
            {
                if (m_native != null)
                {
                    char key = (char)m.WParam;
                    m_chart.ShowSearchDiv(key);
                }
            }
            if (m_host != null)
            {
                if (m_host.OnMessage(ref m) > 0)
                {
                    return;
                }
            }
            base.WndProc(ref m);
        }
        #endregion
    }
}
