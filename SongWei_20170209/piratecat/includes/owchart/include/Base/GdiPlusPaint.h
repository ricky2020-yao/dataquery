/*****************************************************************************\
*                                                                             *
* GdiPlusPaint.h -  GDI plus functions, types, and definitions                *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __GDIPLUSPAINT_H__
#define __GDIPLUSPAINT_H__
#pragma once
#include "..\\..\\stdafx.h"
#include <comdef.h>
#include "gdiplus.h"
#pragma comment(lib, "gdiplus.lib")
using namespace Gdiplus;
#include "CPaint.h"

namespace OwLib
{
	class GdiPlusPaint : public CPaint 
	{
	protected:
		Bitmap *m_bitmap;
		SolidBrush *m_brush;
		_int64 m_brushColor;
		StringFormat *m_emptyStringFormat;
		String m_exportPath;
		Graphics *m_g;
		HDC m_hDC;
		map<String, Bitmap*> m_images;
		int m_offsetX;
		int m_offsetY;
		float m_opacity;
		Pen *m_pen;
		RECT m_pRect;
		_int64 m_penColor;
		float m_penWidth;
		int m_penStyle;
		String m_resourcePath;
		int m_rotateAngle;
		double m_scaleFactorX;
		double m_scaleFactorY;
		RECT m_wRect;
	protected:
		void AffectScaleFactor(Rect *gdiplusRect);
		void AffectScaleFactor(RectF *gdiplusRect);
		SolidBrush* GetBrush(_int64 dwPenColor);
		Color GetGdiPlusColor(_int64 dwPenColor);
		int GetEncoderClsid(const WCHAR *format, CLSID *pClsid);
		Gdiplus::Font* GetFont(FONT *font);
		Pen* GetPen(_int64 dwPenColor, float width, int style);
		GraphicsPath* GetRoundRectPath(Rect gdiPlusRect, int cornerRadius);
	public:
		GdiPlusPaint();
		virtual ~GdiPlusPaint();
		virtual void BeginExport(const String& exportPath, const RECT& rect);
		virtual void BeginPaint(HDC hDC, const RECT& wRect, const RECT& pRect);
		virtual void ClearCaches();
		virtual void DrawEllipse(_int64 dwPenColor, float width, int style, const RECT& rect);
		virtual void DrawEllipse(_int64 dwPenColor, float width, int style, int left, int top, int right, int bottom);
		virtual void DrawGradientEllipse(_int64 dwFirst, _int64 dwSecond, const RECT& rect, int angle);
		virtual void DrawGradientRect(_int64 dwFirst, _int64 dwSecond, const RECT& rect, int cornerRadius, int angle);
		virtual void DrawImage(LPCWSTR imagePath, const RECT& rect);
		virtual void DrawLine(_int64 dwPenColor, float width, int style, const POINT& x, const POINT& y);
		virtual void DrawLine(_int64 dwPenColor, float width, int style, int x1, int y1, int x2, int y2);
		virtual void DrawPolygon(_int64 dwPenColor, float width, int style, POINT *apt, int cpt);
		virtual void DrawPolyline(_int64 dwPenColor, float width, int style, POINT *apt, int cpt);
		virtual void DrawRect(_int64 dwPenColor, float width, int style, int left, int top, int right, int bottom);
		virtual void DrawRect(_int64 dwPenColor, float width, int style, const RECT& rect);
		virtual void DrawRoundRect(_int64 dwPenColor, float width, int style, const RECT& rect, int cornerRadius);
		virtual void DrawText(LPCWSTR strText, _int64 dwPenColor, FONT *font, const RECT& rect);
		virtual void DrawText(LPCWSTR strText, _int64 dwPenColor, FONT *font, const RECTF& rect);
		virtual void DrawTextAutoEllipsis(LPCWSTR strText, _int64 dwPenColor, FONT *font, const RECT& rect);
		virtual void EndExport();
		virtual void EndPaint();
		virtual void FillEllipse(_int64 dwPenColor, const RECT& rect);
		virtual void FillPolygon(_int64 dwPenColor, POINT *apt, int cpt);
		virtual void FillRect(_int64 dwPenColor, const RECT& rect);
		virtual void FillRect(_int64 dwPenColor, int left, int top, int right, int bottom);
		virtual void FillRoundRect(_int64 dwPenColor, const RECT& rect, int cornerRadius);
		virtual _int64 GetColor(_int64 dwPenColor);
		virtual _int64 GetPaintColor(_int64 dwPenColor);
		virtual POINT GetOffset();
		virtual void SetClip(const RECT& rect);
		virtual void SetOffset(const POINT& offset);
		virtual void SetOpacity(float opacity);
        virtual void SetResourcePath(const String& resourcePath);
		virtual void SetRotateAngle(int rotateAngle);
		virtual void SetScaleFactor(double scaleFactorX, double scaleFactorY);
		virtual bool SupportTransparent();
		virtual SIZE TextSize(LPCWSTR strText, FONT *font);
		virtual SIZEF TextSizeF(LPCWSTR strText, FONT *font);
	};
}

#endif