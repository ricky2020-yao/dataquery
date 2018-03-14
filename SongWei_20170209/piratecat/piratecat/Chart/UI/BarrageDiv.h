/*****************************************************************************\
*                                                                             *
* BarrageDiv.h - Barrage div functions, types, and definitions.               *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by QiChunyou 2016/6/1.                                *
*                                                                             *
******************************************************************************/

#ifndef __BARRAGEDIV__H__
#define __BARRAGEDIV__H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\BaseService.h"

class Barrage
{
private:
	__int64 m_color;
	FONT *m_font;
	int m_mode;
	RECT m_rect;
	int m_speed;
	String m_text;
	int m_tick;
public:
	Barrage();
	virtual ~Barrage();
	__int64 GetColor();
	void SetColor(__int64 color);
	FONT* GetFont();
	void SetFONT(FONT *font);
	int GetMode();
	void SetMode(int mode);
	int GetSpeed();
	void SetSpeed(int speed);
	RECT GetRect();
	void SetRect(const RECT& rect);
	String GetText();
	void SetText(const String& text);
	int GetTick();
	void SetTick(int tick);
public:
	void Calculate();	
};

class BarrageDiv : public ControlA
{
private:
	vector<Barrage*> m_barrages;
	_int64 m_sysColors[6];
	int m_tick;
	int m_timerID;
private:
	CLockEx m_lock;
public:
	BarrageDiv();
	~BarrageDiv();
public:
	void AddBarrage(Barrage *barrage);
	virtual bool ContainsPoint(const POINT& point);
	virtual void OnLoad();
	virtual void OnPaintBackground(CPaint* paint, const RECT& clipRect);
	virtual void OnTimer(int timerID);
};

#endif