/*****************************************************************************\
*                                                                             *
* Food.h -      Food functions, types, and definitions                        *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's snake. All rights reserved.   *
*                                                                             *
*******************************************************************************/

#ifndef __FOOD_H__
#define __FOOD_H__
#pragma once

class Food:public ControlA
{
public:
	Food();
	virtual ~Food();
	virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
};

#endif