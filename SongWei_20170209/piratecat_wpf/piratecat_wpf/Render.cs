using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using OwLib;
using piratecat;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Interop;

namespace owchart_wpf
{
    /// <summary>
    /// 辅助呈现控件
    /// </summary>
    public class Render : ContentControl
    {
       #region Lord 2016/11/26
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="native">方法库</param>
        public Render(INativeBase native)
        {
            m_native = native;
            m_wpfPaint = native.Paint as WPFPaint;
        }

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 绘图库
        /// </summary>
        private WPFPaint m_wpfPaint;

        private OwChart m_chart;

        /// <summary>
        /// 获取或设置行情控件
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="drawingContext">参数</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (m_native != null)
            {
                m_wpfPaint.Context = drawingContext;
                RECT rect = new RECT(0, 0, (int)ActualWidth, (int)ActualHeight);
                m_wpfPaint.BeginPaint(IntPtr.Zero, rect, rect);
                m_native.OnPaint(rect);
                m_wpfPaint.EndPaint();
            }
            base.OnRender(drawingContext);
        }

        /// <summary>
        /// 尺寸改变方法
        /// </summary>
        /// <param name="sizeInfo">参数</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (m_native != null)
            {
                m_native.OnResize();
            }
        }
        #endregion
    }
}
