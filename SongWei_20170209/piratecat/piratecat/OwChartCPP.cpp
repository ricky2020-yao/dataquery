#include "stdafx.h"
#include "resource.h"
#include "Chart\\UI\\OwChart.h"
#include "Chart\\UI\\WinformControlHostEx.h"
#include "Chart\\UI\\GdiPlusPaintEx.h"
#include "Xml\\UIXml.h"
#include "Chart\\UI\\UIXmlEx.h"
#include "Chart\\Service\\DataCenter.h"
#ifndef ULONG_PTR  
#define ULONG_PTR unsigned long
#endif
#include "gdiplus.h"
#pragma comment(lib, "gdiplus.lib")
using namespace Gdiplus;

OwChart *m_chart = 0;
ControlHost *m_host = 0;
HWND m_hWnd = 0;
NativeBase *m_native = 0;

HINSTANCE hInst;					     //实例句柄
// 此代码模块中包含的函数的前向声明:
ATOM			   MyRegisterClass(HINSTANCE hInstance);                 //注册窗口类
BOOL			   InitInstance(HINSTANCE, int);		         //初始化窗口各种控件
LRESULT CALLBACK   WndProc(HWND, UINT, WPARAM, LPARAM);                          //主窗口回调函数

//程序入口
int APIENTRY _tWinMain(HINSTANCE hInstance,
					   HINSTANCE hPrevInstance,
					   LPTSTR    lpCmdLine,
					   int       nCmdShow)
{
	std::locale::global(std::locale(""));
	MSG msg;
	CoInitialize(0);
	GdiplusStartupInput gdiplusStartupInput;
	ULONG_PTR gdiplusToken;
	GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, 0);
	MyRegisterClass(hInstance);
	// 执行应用程序初始化:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}
	// 主消息循环:
	while (GetMessage(&msg, 0, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, 0, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}
	CoUninitialize();
	GdiplusShutdown(gdiplusToken);
	return 0;
}

//注册窗口类
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(wcex);

	wcex.style			= CS_HREDRAW | CS_VREDRAW | CS_DBLCLKS;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_ICON1));
	wcex.hCursor		= LoadCursor(0, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)GetStockObject(BLACK_BRUSH);
	wcex.lpszMenuName	= 0;
	wcex.lpszClassName  = L"DIANDIAN";
	wcex.hIconSm		= 0;

	return RegisterClassEx(&wcex);
}

SIZE GetClientSize()
{
	RECT rect;
	::GetClientRect(m_hWnd, &rect);
	SIZE size = {rect.right - rect.left, rect.bottom - rect.top};
	return size;
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	HWND hWnd;
	hInst = hInstance; // 将实例句柄存储在全局变量中
	hWnd = CreateWindow(L"DIANDIAN", L"路德金融平台(C++)", WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, 0, 0, hInstance, NULL);
	if (!hWnd)
	{
		return FALSE;
	}
	m_hWnd = hWnd;
	m_chart = new OwChart();
	m_chart->CreateNative();
	m_native = m_chart->GetNative();
	GdiPlusPaintEx *gdiPlusPaint = new GdiPlusPaintEx;
	m_native->SetPaint(gdiPlusPaint);
	DataCenter::StartService();
	WinformControlHost *host = new WinformControlHostEx;
	host->SetNative(m_native);
	m_native->SetHost(host);
	host->SetHWnd(hWnd);
	m_native->SetAllowScaleSize(true);
	m_host = host;
	m_chart->ResetScaleSize(GetClientSize());
	UpdateWindow(m_hWnd);
	string filePath = DataCenter::GetAppPath() + "\\config\\MainFrame.xml";
	String wFilePath;
	CStr::stringTowstring(wFilePath, filePath);
	m_chart->Load(wFilePath);
	ShowWindow(hWnd, SW_MAXIMIZE);
	UpdateWindow(hWnd);
	m_chart->ShowLoginWindow();
	return TRUE;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	if(message == WM_MOUSEWHEEL)
	{
		int delta = (int)wParam;
		if (m_host->IsKeyPress(VK_CONTROL))
		{
			double scaleFactor = m_chart->GetScaleFactor();
			if (delta > 0)
			{
				if (scaleFactor > 0.1)
				{
					scaleFactor -= 0.1;
				}
			}
			else if (delta < 0)
			{
				if (scaleFactor < 10)
				{
					scaleFactor += 0.1;
				}
			}
			m_chart->SetScaleFactor(scaleFactor);
			m_chart->ResetScaleSize(GetClientSize());
			UpdateWindow(m_hWnd);
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
	else if(message == WM_SETCURSOR)
	{
		if((int)lParam == 33554433)
		{
			return 1;
		}
	}
	switch (message)
	{
		//处理关闭窗口的消息
	case WM_CLOSE:
		{
			if(m_chart)
			{
				m_chart->Exit();
			}
			DestroyWindow(hWnd);
			PostQuitMessage(0);
			break;
		}
	case WM_SIZE:
		{    
			if (m_host)
            {
				m_chart->ResetScaleSize(GetClientSize());
				UpdateWindow(m_hWnd);
            }
			break;
		}
	default:
		int handle = 0;
		if(m_chart)
		{
			handle = m_chart->WndProc(hWnd, message, wParam, lParam);
		}
		if(handle == 0)
		{
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		else
		{
			return 1;
		}
	}
	return 0;
}