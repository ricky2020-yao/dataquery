using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace KLine
{
    internal class CMathLib
    {
        [SuppressUnmanagedCodeSecurity, DllImport("owmath.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern double M008(double[] list, int length);
        [SuppressUnmanagedCodeSecurity, DllImport("owmath.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern double M009(double[] list, int length);
        [SuppressUnmanagedCodeSecurity, DllImport("owmath.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int M012(double min, double max, int yLen, int maxSpan, int minSpan, int defCount, ref double step, ref int digit);
        [SuppressUnmanagedCodeSecurity, DllImport("owmath.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void M105(int x1, int y1, int x2, int y2, ref int x, ref int y, ref int w, ref int h);
        [SuppressUnmanagedCodeSecurity, DllImport("owmath.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern double M129(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec);
        [SuppressUnmanagedCodeSecurity, DllImport("owmath.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void M130(double num, ref int tm_year, ref int tm_mon, ref int tm_mday, ref int tm_hour, ref int tm_min, ref int tm_sec, ref int tm_msec);
        [SuppressUnmanagedCodeSecurity, DllImport("owmath.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void Regist();
    }
}
