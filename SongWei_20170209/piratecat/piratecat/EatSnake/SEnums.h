/*****************************************************************************\
*                                                                             *
* SEnums.h -    Snake types                                                   *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's snake. All rights reserved.   *
*                                                                             *
*******************************************************************************/

#ifndef __SENUMS_H__
#define __SENUMS_H__
#pragma once
typedef enum GameState
{
    GameState_Begin,
    GameState_Lose,
    GameState_Playing,
    GameState_Suspend
};

typedef enum SnakeDirection
{
	SnakeDirection_Down,
	SnakeDirection_Left,
	SnakeDirection_Right,
	SnakeDirection_Up
};
#endif