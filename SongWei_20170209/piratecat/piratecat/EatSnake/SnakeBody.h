/*****************************************************************************\
*                                                                             *
* SnakeBody.h -  SnakeBody functions, types, and definitions                  *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's snake. All rights reserved.   *
*                                                                             *
*******************************************************************************/

#ifndef __SNAKEBODY_H__
#define __SNAKEBODY_H__
#pragma once
#include "SEnums.h"

class SnakeBody:public ControlA
{
private:
	SnakeDirection m_direction;
	bool m_isHeader;
public:
	SnakeBody(const RECT& rect);
	virtual ~SnakeBody();
	SnakeDirection GetDirection();
	void SetDirection(SnakeDirection direction);
	void SetHeader();
public:
	virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
};

#endif