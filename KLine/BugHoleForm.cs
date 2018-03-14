using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OwLib;
using System.Runtime.InteropServices;

namespace KLine
{
    /// <summary>
    /// �涴����
    /// </summary>
    public partial class BugHoleForm : Form
    {
        /// <summary>
        /// �����涴����
        /// </summary>
        public BugHoleForm()
        {
            InitializeComponent();
        }

        private WinHostEx m_host;

        /// <summary>
        /// ��ȡ�������豸������
        /// </summary>
        public WinHostEx Host
        {
            get { return m_host; }
            set { m_host = value; }
        }

        private INativeBase m_native;

        /// <summary>
        /// ��ȡ�����÷�����
        /// </summary>
        public INativeBase Native
        {
            get { return m_native; }
            set { m_native = value; }
        }

        private double m_scaleFactor = 1;

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        public double ScaleFactor
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        private WindowXmlEx m_window;

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WindowXmlEx Window
        {
            get { return m_window; }
            set { m_window = value; }
        }

        /// <summary>
        /// ��Ӿ���
        /// </summary>
        /// <param name="native">��</param>
        /// <param name="target">Ŀ��</param>
        public void AddBugHole(INativeBase native, ControlA target)
        {
            if (m_native == null)
            {
                m_native = NativeHandler.CreateNative();
                m_native.MirrorMode = MirrorMode.BugHole;
                m_native.Paint = new GdiPlusPaintEx();
                m_native.Host = new WinHostEx();
                m_native.Host.Native = m_native;
                m_native.ResourcePath = WinHostEx.GetAppPath() + "\\config";
                m_host = m_native.Host as WinHostEx;
                m_host.HWnd = Handle;
                //���óߴ�
                m_native.AllowScaleSize = true;
                m_native.DisplaySize = new SIZE(ClientSize.Width, ClientSize.Height);
                ResetScaleSize(GetClientSize());
            }
            m_native.AddMirror(native, target);
            m_native.Update();
            m_native.Invalidate();
        }

        /// <summary>
        /// ��ȡ�ͻ��˳ߴ�
        /// </summary>
        /// <returns>�ͻ��˳ߴ�</returns>
        public SIZE GetClientSize()
        {
            return new SIZE(ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// ����ر��¼�
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            List<ControlA> controls = m_native.GetControls();
            List<ControlA> removeControls = new List<ControlA>();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                removeControls.Add(controls[i]);
            }
            for (int i = 0; i < controlsSize; i++)
            {
                m_native.RemoveMirror(removeControls[i]);
            }
            removeControls.Clear();
            if (m_native != null)
            {
                m_native.Dispose();
                m_native = null;
            }
            if (m_window != null)
            {
                WindowXmlEx window = m_window;
                m_window = null;
                window.Close();
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
                double scaleFactor = ScaleFactor;
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
                ScaleFactor = scaleFactor;
                ResetScaleSize(GetClientSize());
                Invalidate();
            }
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
                ResetScaleSize(GetClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// �������ųߴ�
        /// </summary>
        /// <param name="clientSize">�ͻ��˴�С</param>
        public void ResetScaleSize(SIZE clientSize)
        {
            INativeBase native = Native;
            if (native != null)
            {
                native.ScaleSize = new SIZE((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                native.Update();
            }
        }

        /// <summary>
        /// ��Ϣ
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m_host != null)
            {
                if (m_host.OnMessage(ref m) > 0)
                {
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}