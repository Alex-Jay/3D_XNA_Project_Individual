﻿using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class UIProgressController : Controller
    {
        #region Fields
        private int maxValue, startValue, currentValue;
        private UITextureObject parentUITextureActor;
        private bool bDirty = false;
        #endregion

        #region Properties
        public int CurrentValue
        {
            get
            {
                return this.currentValue;
            }
            set
            {
                this.currentValue = ((value >= 0) && (value <= maxValue)) ? value : 0;
                bDirty = true;
            }
        }
        public int MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                this.maxValue = (value >= 0) ? value : 0;
            }
        }
        public int StartValue
        {
            get
            {
                return this.startValue;
            }
            set
            {
                this.startValue = (value >= 0) ? value : 0;
            }
        }
        #endregion

        public UIProgressController(string id, ControllerType controllerType, int startValue, int maxValue, EventDispatcher eventDispatcher)
            : base(id, controllerType)
        {
            this.StartValue = startValue;
            this.MaxValue = maxValue;
            this.CurrentValue = startValue;

            //register with the event dispatcher for the events of interest
            RegisterForEventHandling(eventDispatcher);
        }

        #region Event Handling
        protected virtual void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.PlayerChanged += EventDispatcher_PlayerChanged;
        }
        protected virtual void EventDispatcher_PlayerChanged(EventData eventData)
        {
            //the second value in additionalParameters holds target ID for the event
            string targetID = eventData.AdditionalEventParameters[0] as string;

            //was this event targeted at me?
            if (targetID.Equals(this.ID) && eventData.EventType == EventActionType.OnHealthChange)
            {
                //the second value in additionalParameters holds the gain/lose health value
                Integer healthInteger = (eventData.AdditionalEventParameters[1] as Integer);
                this.CurrentValue = this.currentValue + (int)healthInteger;
            }
        }
        #endregion

        public override void Update(GameTime gameTime, IActor actor)
        {
            //has the value changed?
            if (this.bDirty)
            {
                this.parentUITextureActor = actor as UITextureObject;
                //set the source rectangle according to whatever start value the user supplies
                UpdateSourceRectangle();
                this.bDirty = false;
            }
            base.Update(gameTime, actor);
        }
        private void UpdateSourceRectangle()
        {
            //how much of a percentage of the width of the image does the current value represent?
            float widthMultiplier = (float)this.currentValue / this.maxValue;

            //now set the amount of visible rectangle using the current value
            this.parentUITextureActor.SourceRectangleWidth
                = (int)(widthMultiplier * this.parentUITextureActor.OriginalSourceRectangle.Width);
        } 
    }
}
