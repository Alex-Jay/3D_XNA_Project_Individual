/*
Function: 		Use this class to say exactly how your game listens for events and responds with changes to the game.
Author: 		NMCG
Version:		1.0
Date Updated:	17/11/17
Bugs:			
Fixes:			None
*/

using GDLibrary;
using Microsoft.Xna.Framework;

namespace GDApp
{
    public class MyGameStateManager : GameStateManager
    {
        private MenuManager menuMgr;

        public MyGameStateManager(Game game, EventDispatcher eventDispatcher, StatusType statusType, MenuManager menuManager) 
            : base(game, eventDispatcher, statusType)
        {
            menuMgr = menuManager;
            RegisterForEventHandling(eventDispatcher);
        }

        #region Event Handling
        protected override void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.PlayerChanged += EventDispatcher_PlayerChanged;
        }

        protected virtual void EventDispatcher_PlayerChanged(EventData eventData)
        {
            if (eventData.EventType == EventActionType.OnLose)
            {
                EventDispatcher.Publish(new EventData(EventActionType.OnPause, EventCategoryType.MainMenu));
            }
        }
        #endregion

        protected override void ApplyUpdate(GameTime gameTime)
        {
            //to do...
            base.ApplyUpdate(gameTime);
        }
    }
}
