using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OwLib;
using piratecat;
using System.Diagnostics;
using System.Windows.Interop;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace owchart_wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Lord 2016/11/26
        /// <summary>
        /// 创建窗体
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Background = Brushes.Black;
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        /// <summary>
        /// 行情控件
        /// </summary>
        private OwChart m_chart;

        /// <summary>
        /// 鼠标点击次数
        /// </summary>
        private int m_clickCount = 0;

        /// <summary>
        /// 控件管理器
        /// </summary>
        private WPFHost m_host;

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 绘图库
        /// </summary>
        private WPFPaint m_wpfPaint;

        /// <summary>
        /// 获取鼠标按钮
        /// </summary>
        /// <param name="e">鼠标参数</param>
        /// <returns>鼠标按钮</returns>
        private OwLib.MouseButtonsA GetMouseButton(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                return OwLib.MouseButtonsA.Left;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                return OwLib.MouseButtonsA.Right;
            }
            else
            {
                return OwLib.MouseButtonsA.None;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //创建管理器
            m_host = new WPFHost();
            //注册消息
            WindowInteropHelper helper = new WindowInteropHelper(this);
            m_host.WindowHwnd = helper.Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(m_host.WindowHwnd);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
            //启动服务
            DataCenter.StartService();
            //创建方法库
            m_chart = new OwChart();
            m_chart.CreateNative();
            m_native = m_chart.Native;
            m_wpfPaint = new WPFPaint();
            m_native.Paint = m_wpfPaint;
            m_native.Host = m_host;
            m_host.Render = new Render(m_native);
            m_native.DisplaySize = new SIZE(525, 350);
            AddChild(m_host.Render);
            m_host.Container = m_host.Render;
            m_host.Native = m_native;
            m_chart.Load(WPFHost.GetAppPath() + "\\config\\MainFrame.xml");
            m_host.Render.Chart = m_chart;
            m_chart.ShowLoginWindow();
        }

        /// <summary>
        /// 窗体关闭方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (m_host.AllowOperate)
            {
                m_native.OnKeyDown((char)m_host.GetKeyCode(e.Key));
            }
        }

        /// <summary>
        /// 键盘抬起方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (m_host.AllowOperate)
            {
                m_native.OnKeyUp((char)m_host.GetKeyCode(e.Key));
            }
        }

        /// <summary>
        /// 鼠标双击方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            m_clickCount = 2;
            if (m_host.AllowOperate)
            {
                m_native.OnMouseDown(GetMouseButton(e), m_clickCount, 0);
            }
        }

        /// <summary>
        /// 鼠标按下方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            m_clickCount = 1;
            if (m_host.AllowOperate)
            {
                m_native.OnMouseDown(GetMouseButton(e), m_clickCount, 0);
            }
        }

        /// <summary>
        /// 鼠标移动方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (m_host.AllowOperate)
            {
                m_native.OnMouseMove(GetMouseButton(e), m_clickCount, 0);
            }
        }

        /// <summary>
        /// 鼠标抬起方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (m_host.AllowOperate)
            {
                if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                {
                    m_native.OnMouseUp(OwLib.MouseButtonsA.Left, m_clickCount, 0);
                }
                else if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
                {
                    m_native.OnMouseUp(OwLib.MouseButtonsA.Right, m_clickCount, 0);
                }
                else
                {
                    m_native.OnMouseUp(GetMouseButton(e), m_clickCount, 0);
                }
            }
            m_clickCount = 0;
        }
        
        /// <summary>
        /// 鼠标滚动方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (m_host.AllowOperate)
            {
                m_native.OnMouseWheel(GetMouseButton(e), m_clickCount, e.Delta);
            }
        }

        /// <summary>
        /// 大小改变方法
        /// </summary>
        /// <param name="sizeInfo">参数</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Size newSize = sizeInfo.NewSize;
            ResetLayout(new SIZE((int)newSize.Width, (int)newSize.Height));
        }

        /// <summary>
        /// 重置大小
        /// </summary>
        private void ResetLayout(SIZE size)
        {
            if (m_host!=null && m_host.Render != null)
            {
                double horizontalBorderHeight = SystemParameters.ResizeFrameHorizontalBorderHeight;
                double verticalBorderWidth = SystemParameters.ResizeFrameVerticalBorderWidth;
                double captionHeight = SystemParameters.CaptionHeight;
                m_host.Render.Width = size.cx - verticalBorderWidth * 2;
                m_host.Render.Height = size.cy - horizontalBorderHeight * 2 - captionHeight;
            }
        }

        /// <summary>
        /// 系统消息
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wParam">数据</param>
        /// <param name="lParam">数据</param>
        /// <param name="handled">是否处理</param>
        /// <returns>句柄</returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (m_host != null)
            {
                if (msg == 0x100 || msg == 260)
                {
                    char key = (char)(((int)((long)wParam)));
                    m_chart.ShowSearchDiv(key);
                }
                //输入消息
                if (msg == 0x0102)
                {
                    char key = (char)(wParam);
                    if (m_native.OnChar(key))
                    {
                        handled = true;
                    }
                }
                //秒表消息
                else if (msg == 0x0113)
                {
                    if (m_host.WindowHwnd == hwnd)
                    {
                        int timerID = (int)wParam;
                        m_native.OnTimer(timerID);
                    }
                }
                //调用控件线程方法
                else if (msg == 0x0401)
                {
                    if (m_host != null)
                    {
                        m_host.OnInvoke((int)wParam);
                    }
                }
                //重绘方法
                else if (msg == 0x0402)
                {
                    if (m_host != null)
                    {
                        m_host.Invalidate();
                    }
                }
            }
            return IntPtr.Zero;
        }
        #endregion
    }
}
