/*****************************************************************************\
*                                                                             *
* CGdiPlus.cs -   Gdi plus functions                                          *
*                                                                             *
*               Version 4.00 ★★★★★                                        *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's OwChart. All rights reserved. *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using piratecat;

namespace OwLib
{
    /// <summary>
    /// Gdi+绘图类
    /// </summary>
    public class WPFPaint : CPaint
    {
        #region Lord 2016/6/12
        /// <summary>
        /// 创建绘图类
        /// </summary>
        public WPFPaint()
        {
        }

        /// <summary>
        /// 是否裁剪
        /// </summary>
        private bool m_hasClip = false;

        /// <summary>
        /// 横向偏移
        /// </summary>
        private int m_offsetX;

        /// <summary>
        /// 纵向偏移
        /// </summary>
        private int m_offsetY;

        /// <summary>
        /// 透明度
        /// </summary>
        private float m_opacity = 1;

        /// <summary>
        /// 资源路径
        /// </summary>
        private String m_resourcePath;

        /// <summary>
        /// 横向缩放因子
        /// </summary>
        private float m_scaleFactorX = 1;

        /// <summary>
        /// 纵向缩放因子
        /// </summary>
        private float m_scaleFactorY = 1;

        private DrawingContext m_context;

        /// <summary>
        /// 获取或设置绘图上下文
        /// </summary>
        public DrawingContext Context
        {
            get { return m_context; }
            set { m_context = value; }
        }

        /// <summary>
        /// 缩放因子生效
        /// </summary>
        /// <param name="rect">矩形</param>
        private void AffectScaleFactor(ref Rect rect)
        {
            if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
            {
                rect.X = (int)(m_scaleFactorX * rect.X);
                rect.Y = (int)(m_scaleFactorY * rect.Y);
                rect.Width = (int)(m_scaleFactorX * rect.Width);
                rect.Height = (int)(m_scaleFactorY * rect.Height);
            }
        }

        /// <summary>
        /// 开始导出
        /// </summary>
        /// <param name="exportPath">路径</param>
        /// <param name="rect">区域</param>
        public virtual void BeginExport(String exportPath, RECT rect)
        {
        }

        /// <summary>
        /// 开始绘图
        /// </summary>
        /// <param name="hdc">HDC</param>
        /// <param name="wRect">窗体区域</param>
        /// <param name="pRect">刷新区域</param>
        public virtual void BeginPaint(IntPtr hdc, RECT wRect, RECT pRect)
        {
            m_hasClip = false;
            m_opacity = 1;
            m_resourcePath = null;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public virtual void ClearCaches()
        {
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        public virtual void DrawEllipse(long dwPenColor, float width, int style, RECT rect)
        {
            if (dwPenColor == COLOR.EMPTY) return;
            double rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            double rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            int centerX = (int)(rect.left + rw / 2 + m_offsetX);
            int centerY = (int)(rect.top + rw / 2 + m_offsetY);
            if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
            {
                centerX = (int)(m_scaleFactorX * centerX);
                centerY = (int)(m_scaleFactorY * centerY);
                rw = (int)(m_scaleFactorX * rw);
                rh = (int)(m_scaleFactorY * rh);
            }
            Geometry geoEllipse = new EllipseGeometry(new Point(centerX, centerY), rw / 2, rh / 2);
            m_context.DrawGeometry(null, GetPen(dwPenColor, width, style), geoEllipse);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public virtual void DrawEllipse(long dwPenColor, float width, int style, int left, int top, int right, int bottom)
        {
            RECT rect = new RECT(left, top, right, bottom);
            DrawEllipse(dwPenColor, width, style, rect);
        }

        /// <summary>
        /// 绘制渐变椭圆
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="angle">角度</param>
        public virtual void DrawGradientEllipse(long dwFirst, long dwSecond, RECT rect, int angle)
        {
            double rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            double rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            LinearGradientBrush lgb = new LinearGradientBrush(GetWPFColor(dwFirst), GetWPFColor(dwSecond), angle);
            int centerX = (int)(rect.left + rw / 2 + m_offsetX);
            int centerY = (int)(rect.top + rw / 2 + m_offsetY);
            if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
            {
                centerX = (int)(m_scaleFactorX * centerX);
                centerY = (int)(m_scaleFactorY * centerY);
                rw = (int)(m_scaleFactorX * rw);
                rh = (int)(m_scaleFactorY * rh);
            }
            Geometry geoEllipse = new EllipseGeometry(new Point(centerX, centerY), rw / 2, rh / 2);
            m_context.DrawGeometry(lgb, null, geoEllipse);
        }

        /// <summary>
        /// 绘制渐变矩形
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="angle">圆角大小</param>
        /// <param name="angle">角度</param>
        public virtual void DrawGradientRect(long dwFirst, long dwSecond, RECT rect, int cornerRadius, int angle)
        {
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            LinearGradientBrush lgb = new LinearGradientBrush(GetWPFColor(dwFirst), GetWPFColor(dwSecond), angle);
            Rect wpfRect = new Rect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            AffectScaleFactor(ref wpfRect);
            Geometry geoRect = new RectangleGeometry(wpfRect);
            m_context.DrawGeometry(lgb, null, geoRect);
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="cornerRadius">圆角角度</param>
        public void DrawRoundRect(long dwPenColor, float width, int style, RECT rect, int cornerRadius)
        {
            DrawRect(dwPenColor, width, style, rect);
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="rect">绘制区域</param>
        public virtual void DrawImage(String imagePath, RECT rect)
        {
        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        public virtual void DrawLine(long dwPenColor, float width, int style, int x1, int y1, int x2, int y2)
        {
            if (dwPenColor == COLOR.EMPTY) return;
            int lx1 = x1 + m_offsetX;
            int ly1 = y1 + m_offsetY;
            int lx2 = x2 + m_offsetX;
            int ly2 = y2 + m_offsetY;
            if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
            {
                lx1 = (int)(m_scaleFactorX * lx1);
                ly1 = (int)(m_scaleFactorY * ly1);
                lx2 = (int)(m_scaleFactorX * lx2);
                ly2 = (int)(m_scaleFactorY * ly2);
            }
            m_context.DrawLine(GetPen(dwPenColor, width, style),new Point(lx1, ly1), new Point(lx2, ly2));
        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="x">第一个点的坐标</param>
        /// <param name="y">第二个点的坐标</param>
        public virtual void DrawLine(long dwPenColor, float width, int style, POINT x, POINT y)
        {
            DrawLine(dwPenColor, width, style, x.x, x.y, y.x, y.y);
        }

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">点的数组</param>
        public virtual void DrawPolygon(long dwPenColor, float width, int style, POINT[] points)
        {
            if (dwPenColor == COLOR.EMPTY) return;
            int pointSize = points.Length;
            List<Point> wpfPoints = new List<Point>();
            for (int i = 0; i < pointSize; i++)
            {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
                {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                wpfPoints.Add(new Point(x, y));
            }
            PathGeometry pathGeo = new PathGeometry();
            PolyLineSegment polyline = new PolyLineSegment(wpfPoints, true);
            List<PolyLineSegment> segments = new List<PolyLineSegment>();
            segments.Add(polyline);
            PathFigure pathFigure = new PathFigure(wpfPoints[0], segments, true);
            pathGeo.Figures.Add(pathFigure);
            m_context.DrawGeometry(null, GetPen(dwPenColor, width, style), pathGeo);
        }

        /// <summary>
        /// 绘制大量直线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">点集</param>
        public virtual void DrawPolyline(long dwPenColor, float width, int style, POINT[] points)
        {
            if (dwPenColor == COLOR.EMPTY) return;
            int pointSize = points.Length;
            List<Point> wpfPoints = new List<Point>();
            for (int i = 0; i < pointSize; i++)
            {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
                {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                wpfPoints.Add(new Point(x, y));
            }
            PathGeometry pathGeo = new PathGeometry();
            PolyLineSegment polyline = new PolyLineSegment(wpfPoints, true);
            List<PolyLineSegment> segments = new List<PolyLineSegment>();
            segments.Add(polyline);
            PathFigure pathFigure = new PathFigure(wpfPoints[0], segments, false);
            pathGeo.Figures.Add(pathFigure);
            m_context.DrawGeometry(null, GetPen(dwPenColor, width, style), pathGeo);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        public virtual void DrawRect(long dwPenColor, float width, int style, RECT rect)
        {
            if (dwPenColor == COLOR.EMPTY) return;
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rect wpfRect = new Rect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            AffectScaleFactor(ref wpfRect);
            Geometry geoRect = new RectangleGeometry(wpfRect);
            m_context.DrawGeometry(null, GetPen(dwPenColor, width, style),geoRect);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public virtual void DrawRect(long dwPenColor, float width, int style, int left, int top, int right, int bottom)
        {
            RECT rect = new RECT(left, top, right, bottom);
            DrawRect(dwPenColor, width, style, rect);
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        public virtual void DrawText(String text, long dwPenColor, FONT font, RECT rect)
        {
            int strX = rect.left + m_offsetX;
            int strY = rect.top + m_offsetY;
            FormattedText ft = null;
            if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
            {
                float fontSize = font.m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2;
                FONT newFont = new FONT(font.m_fontFamily, fontSize, font.m_bold, font.m_underline, font.m_italic);
                ft = GetFont(text, newFont, dwPenColor);
            }
            else
            {
                ft = GetFont(text, font, dwPenColor);
            }
            m_context.DrawText(ft, new Point(strX, strY));
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        public virtual void DrawText(String text, long dwPenColor, FONT font, RECTF rect)
        {
            int strX = (int)(rect.left + m_offsetX);
            int strY = (int)(rect.top + m_offsetY);
            FormattedText ft = null;
            if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
            {
                float fontSize = font.m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2;
                FONT newFont = new FONT(font.m_fontFamily, fontSize, font.m_bold, font.m_underline, font.m_italic);
                ft = GetFont(text, newFont, dwPenColor);
            }
            else
            {
                ft = GetFont(text, font, dwPenColor);
            }
            m_context.DrawText(ft, new Point(strX, strY));
        }

        /// <summary>
        /// 绘制自动省略结尾的文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        public virtual void DrawTextAutoEllipsis(String text, long dwPenColor, FONT font, RECT rect)
        {
            DrawText(text, dwPenColor, font, rect);
        }

        /// <summary>
        /// 结束导出
        /// </summary>
        public virtual void EndExport()
        {
        }

        /// <summary>
        /// 结束绘图
        /// </summary>
        public virtual void EndPaint()
        {
            if (m_hasClip)
            {
                m_hasClip = false;
                m_context.Pop();
            }
            m_offsetX = 0;
            m_offsetY = 0;
            m_opacity = 1;
            m_resourcePath = null;
        }

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        public virtual void FillEllipse(long dwPenColor, RECT rect)
        {
            FillEllipse(dwPenColor, rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public virtual void FillEllipse(long dwPenColor, int left, int top, int right, int bottom)
        {
            if (dwPenColor == COLOR.EMPTY) return;
            double rw = right - left - 1;
            if (rw < 1) rw = 1;
            double rh = bottom - top - 1;
            if (rh < 1) rh = 1;
            int centerX = (int)(left + rw / 2 + m_offsetX);
            int centerY = (int)(top + rw / 2 + m_offsetY);
            if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
            {
                centerX = (int)(m_scaleFactorX * centerX);
                centerY = (int)(m_scaleFactorY * centerY);
                rw = (int)(m_scaleFactorX * rw);
                rh = (int)(m_scaleFactorY * rh);
            }
            Geometry geoEllipse = new EllipseGeometry(new Point(centerX, centerY), rw / 2, rh / 2);
            m_context.DrawGeometry(GetBrush(dwPenColor), null, geoEllipse);
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="points">点的数组</param>
        public virtual void FillPolygon(long dwPenColor, POINT[] points)
        {
            if (dwPenColor == COLOR.EMPTY) return;
            int pointSize = points.Length;
            List<Point> wpfPoints = new List<Point>();
            for (int i = 0; i < pointSize; i++)
            {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != -1 || m_scaleFactorY != -1)
                {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                wpfPoints.Add(new Point(x, y));
            }
            PathGeometry pathGeo = new PathGeometry();
            PolyLineSegment polyline = new PolyLineSegment(wpfPoints, true);
            List<PolyLineSegment> segments = new List<PolyLineSegment>();
            segments.Add(polyline);
            PathFigure pathFigure = new PathFigure(wpfPoints[0], segments, true);
            pathGeo.Figures.Add(pathFigure);
            m_context.DrawGeometry(GetBrush(dwPenColor), null, pathGeo);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        public virtual void FillRect(long dwPenColor, RECT rect)
        {
            FillRect(dwPenColor, rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public virtual void FillRect(long dwPenColor, int left, int top, int right, int bottom)
        {
            if (dwPenColor == COLOR.EMPTY) return;
            Rect wpfRect = new Rect(left + m_offsetX, top + m_offsetY, right - left, bottom - top);
            AffectScaleFactor(ref wpfRect);
            Geometry geoRect = new RectangleGeometry(wpfRect);
            m_context.DrawGeometry(GetBrush(dwPenColor), null, geoRect);
        }

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="cornerRadius">圆角角度</param>
        public void FillRoundRect(long dwPenColor, RECT rect, int cornerRadius)
        {
            FillRect(dwPenColor, rect);
        }

        /// <summary>
        /// 获取画刷
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <returns>画刷</returns>
        private SolidColorBrush GetBrush(long dwPenColor)
        {
            SolidColorBrush brush = new SolidColorBrush(GetWPFColor(dwPenColor));
            return brush;
        }

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        public virtual long GetColor(long dwPenColor)
        {
            if (dwPenColor <= COLOR.EMPTY)
            {
                dwPenColor = CDraw.GetBlackColor(dwPenColor);
            }
            return dwPenColor;
        }

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        public virtual long GetPaintColor(long dwPenColor)
        {
            return GetColor(dwPenColor);
        }

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">整型颜色</param>
        /// <returns>Gdi颜色</returns>
        private Color GetWPFColor(long dwPenColor)
        {
            dwPenColor = GetPaintColor(dwPenColor);
            int a = 0, r = 0, g = 0, b = 0;
            COLOR.ToARGB(this, dwPenColor, ref a, ref r, ref g, ref b);
            Color wpfColor = Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
            if (m_opacity < 1)
            {
                Color opacityColor = Color.FromArgb((byte)(wpfColor.A * m_opacity), (byte)r, (byte)g, (byte)b);
                return opacityColor;
            }
            else
            {
                return wpfColor;
            }
        }

        /// <summary>
        /// 获取格式化字体
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <param name="font">颜色</param>
        /// <returns>格式化字体</returns>
        private FormattedText GetFont(String text, FONT font, long dwPenColor)
        {
            FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(font.m_fontFamily), font.m_fontSize, GetBrush(dwPenColor));
            if (font.m_bold)
            {
                ft.SetFontWeight(FontWeights.Bold);
            }
            if (font.m_italic)
            {
                ft.SetFontStyle(FontStyles.Italic);
            }
            if (font.m_underline)
            {
                ft.SetTextDecorations(TextDecorations.Underline);
            }
            return ft;
        }

        /// <summary>
        /// 获取偏移坐标
        /// </summary>
        /// <returns>坐标</returns>
        public virtual POINT GetOffset()
        {
            return new POINT(m_offsetX, m_offsetY);
        }

        /// <summary>
        /// 获取画笔
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <returns>画笔</returns>
        private Pen GetPen(long dwPenColor, float width, int style)
        {
            Color wpfColor = GetWPFColor(dwPenColor);
            Brush brush = new SolidColorBrush(wpfColor);
            Pen pen = new Pen(brush, width);
            if (style == 0)
            {
                pen.DashStyle = DashStyles.Solid;
            }
            else if (style == 1)
            {
                pen.DashStyle = DashStyles.Dash;
            }
            else if (style == 2)
            {
                pen.DashStyle = DashStyles.Dot;
            }
            return pen;
        }

        /// <summary>
        /// 设置裁剪区域
        /// </summary>
        /// <param name="rect">区域</param>
        public virtual void SetClip(RECT rect)
        {
            if (m_hasClip)
            {
                m_context.Pop();
            }
            Rect wpfRect = new Rect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            AffectScaleFactor(ref wpfRect);
            RectangleGeometry rectGeo = new RectangleGeometry(wpfRect);
            m_context.PushClip(rectGeo);
            m_hasClip = true;
        }

        /// <summary>
        /// 设置偏移
        /// </summary>
        /// <param name="offset">偏移坐标</param>
        public virtual void SetOffset(POINT offset)
        {
            m_offsetX = offset.x;
            m_offsetY = offset.y;
        }

        /// <summary>
        /// 设置透明度
        /// </summary>
        /// <param name="opacity">透明度</param>
        public virtual void SetOpacity(float opacity)
        {
            m_opacity = opacity;
        }

        /// <summary>
        /// 设置资源的路径
        /// </summary>
        /// <param name="resourcePath">资源的路径</param>
        public virtual void SetResourcePath(String resourcePath)
        {
            m_resourcePath = resourcePath;
        }

        /// <summary>
        /// 设置旋转角度
        /// </summary>
        /// <param name="rotateAngle">旋转角度</param>
        public virtual void SetRotateAngle(int rotateAngle)
        {
        }

        /// <summary>
        /// 设置缩放因子
        /// </summary>
        /// <param name="scaleFactorX">横向缩放因子</param>
        /// <param name="scaleFactorY">纵向缩放因子</param>
        public virtual void SetScaleFactor(double scaleFactorX, double scaleFactorY)
        {
        }

        /// <summary>
        /// 设置是否支持透明色
        /// </summary>
        /// <returns>是否支持</returns>
        public virtual bool SupportTransparent()
        {
            return true;
        }

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        public virtual SIZE TextSize(String text, FONT font)
        {
            FormattedText ft = GetFont(text, font, 0);
            return new SIZE((int)ft.Width, (int)ft.Height);
        }

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        public virtual SIZEF TextSizeF(String text, FONT font)
        {
            FormattedText ft = GetFont(text, font, 0);
            return new SIZEF((int)ft.Width, (int)ft.Height);
        }
        #endregion
    }
}