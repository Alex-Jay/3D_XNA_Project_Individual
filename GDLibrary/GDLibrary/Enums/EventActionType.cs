﻿/*
Function: 		Enum to define event types generated by game entities e.g. menu, camera manager
Author: 		NMCG
Version:		1.0
Date Updated:	11/10/17
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum EventActionType : sbyte
    {
        //sent by audio, video
        OnPlay,
        OnStop,
        OnPause,

        //sent by menu manager
        OnStart,
        OnRestart,
        OnVolumeUp,
        OnVolumeDown,
        OnMute,
        OnExit,

        //send by mouse or gamepad manager
        OnClick,
        OnHover,

        //sent by camera manager
        OnCameraChanged,

        //sent by player
        OnLoseHealth,
        OnGainHealth,

        //sent by game state manager
        OnLose,
        OnWin,

        OnPickup,
        OnOpen,
        OnClose,

        //used to center mouse at start-up
        OnMouseCentre,
    }
}
