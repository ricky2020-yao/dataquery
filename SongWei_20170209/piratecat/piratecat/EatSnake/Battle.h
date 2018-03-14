/*****************************************************************************\
*                                                                             *
* Battle.h -      Battle functions, types, and definitions                    *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's snake. All rights reserved.   *
*                                                                             *
*******************************************************************************/

#ifndef __BATTLE_H__
#define __BATTLE_H__
#pragma once
#include "SEnums.h"
#include "Food.h"
#include "SnakeBody.h"
#include "Play.h"
#define SNAKE_LENGTH 10
#define SNAKE_SIZE 14

class Play;

class Battle:public ControlA
{
private:
	int m_alpha;
	int m_alphaDirection;
	vector<SnakeBody*> m_bodys;
	int m_eatCount;
	vector<Food*> m_foods;
	Play* m_play;
	int m_speed;
	int m_timerID;
private:
	void AddFood();
	void ChangeState();
	bool IsLose();
	void MoveSnake();
	void ResetSnake();
public:
	Battle();
	virtual ~Battle();
	int GetSpeed();
	void SetSpeed(int speed);
public:
	virtual void OnLoad();
	virtual void OnKeyDown(char key);
	virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
	virtual void OnRemove();
	virtual void OnTimer(int timerID);
};

#endif