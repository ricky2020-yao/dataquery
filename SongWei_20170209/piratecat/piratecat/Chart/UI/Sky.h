/*****************************************************************************\
*                                                                             *
* Sky.h - Sky functions, types, and definitions.                              *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Sky. All rights reserved.            *
*               Created by Lord 2016/6/1.                                     *
*                                                                             *
******************************************************************************/
#include "..\\..\\stdafx.h"

#ifndef __SKY__H__
#define __SKY__H__
#pragma once

class Bullet;
class Plane;

class Sky : public ControlA
{
private:
	vector<Bullet*> m_bullets;
	int m_difficult;
	double m_high;
	Plane *m_plane;
	POINT m_point;
	bool m_showRecord;
	_int64 m_sysColors[6];
	_int64 m_tick;
	_int64 m_tick2;
	int m_ticks;
	int m_timerID;
private:
	void CheckAttach();
	void CreateBullets();
	void MoveBullets();
	void RemoveOutBullets();
public:
	Sky();
	virtual ~Sky();
	bool ShowRecord();
	void SetShowRecord(bool showRecord);
public:
	virtual void OnAdd();
	virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnPaint(CPaint *paint, const RECT& clipRect);
	virtual void OnTimer(int timerID);
};

class Plane
{
private:
	_int64 m_backColor;
	POINT m_location;
	int m_tick;
	int m_tick2;
public:
	Plane();
	virtual ~Plane();
	_int64 GetBackColor();
	void SetBackColor(_int64 backColor);
	POINT GetLocation();
	void SetLocation(POINT location);
public:
	void OnPaintBackground(CPaint *paint);
};

class Bullet
{
private:
	float m_b;
	_int64 m_backColor;
	float m_k;
	POINT m_location;
	int m_mode;
	int m_speed;
	int m_tick;
	int m_tick2;
public:
	Bullet();
	virtual ~Bullet();
	float GetB();
	void SetB(float b);
	_int64 GetBackColor();
	void SetBackColor(_int64 backColor);
	float GetK();
	void SetK(float k);
	POINT GetLocation();
	void SetLocation(POINT location);
	int GetSpeed();
	void SetSpeed(int speed);
public:
	void OnPaintBackground(CPaint *paint);
};

#endif