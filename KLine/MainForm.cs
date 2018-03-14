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

namespace KLine
{
    /// <summary>
    /// ����
    /// </summary>
    public partial class MainForm : Form
    {
        #region Lord 2012/7/4
        /// <summary>
        ///  ����ͼ�οؼ�
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �ؼ�������
        /// </summary>
        private WinHost m_host;

        /// <summary>
        /// �ؼ���
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// XML
        /// </summary>
        private UIXmlEx m_xml;

        /// <summary>
        /// ��ȡ�ͻ��˳ߴ�
        /// </summary>
        /// <returns>�ͻ��˳ߴ�</returns>
        public SIZE GetClientSize()
        {
            return new SIZE(ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="name">����</param>
        public void Load(String name)
        {
            if (name == "MainFrame")
            {
                m_xml = new MainFrame();
            }
            m_xml.CreateNative();
            m_native = m_xml.Native;
            m_native.Paint = new GdiPlusPaintEx();
            m_host = new WinHostEx();
            m_host.Native = m_native;
            m_native.Host = m_host;
            m_host.HWnd = Handle;
            m_native.AllowScaleSize = true;
            m_native.DisplaySize = new SIZE(ClientSize.Width, ClientSize.Height);
            m_xml.ResetScaleSize(GetClientSize());
            m_xml.Script = new GaiaScript(m_xml);
            m_xml.Load(DataCenter.GetAppPath() + "\\config\\"+ name +".html");
            m_native.Update();
            Invalidate();
        }

        /// <summary>
        /// ����ر��¼�
        /// </summary>
        /// <param name="e">�¼�����</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            m_xml.Exit();
            Environment.Exit(0);
            base.OnFormClosing(e);
        }

        /// <summary>
        /// �ߴ�ı䷽��
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (m_host != null)
            {
                m_xml.ResetScaleSize(GetClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (m_host.IsKeyPress(0x11))
            {
                double scaleFactor = m_xml.ScaleFactor;
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
                m_xml.ScaleFactor = scaleFactor;
                m_xml.ResetScaleSize(GetClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {            
            if (m_host != null)
            {
                try
                {
                    if (m_host.OnMessage(ref m) > 0)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    int a = 0;
                }
            }
             base.WndProc(ref m);
        }
        #endregion
    }
}
