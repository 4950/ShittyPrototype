﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirtyGame.game.Core.Components;
using DirtyGame.game.Core.Components.Render;
using DirtyGame.game.Core.Systems.Util;
using DirtyGame.game.SGraphics;
using DirtyGame.game.SGraphics.Commands;
using DirtyGame.game.SGraphics.Commands.DrawCalls;
using EntityFramework;
using EntityFramework.Systems;
using EntityFramework.Managers;
using Microsoft.Xna.Framework;


namespace DirtyGame.game.Systems
{
    class AnimationSystem : EntitySystem
    {
        public AnimationSystem()
            : base(SystemDescriptions.AnimationSystem.Aspect, SystemDescriptions.AnimationSystem.Priority)
        {
        }

        public override void OnEntityAdded(Entity e)
        {           
         
        }

        public override void OnEntityRemoved(Entity e)
        {           
            
        }

        public override void ProcessEntities(IEnumerable<Entity> entities, float dt)
        {
            foreach (Entity e in entities)
            {
                //Getting components for this entity
                Animation animation = e.GetComponent<Animation>();
                Sprite sprite = e.GetComponent<Sprite>();

                //Move the sprite to the next frame
                //Adding to the time since last draw
                animation.TimeElapsed += dt;
                //Saving the time between frames
                double timeBetweenFrames = 1.0f / sprite.SpriteSheet.Animation[animation.CurrentAnimation].Length;
                //Check to see if enough time has passed to render the next frame
                if (animation.TimeElapsed > timeBetweenFrames)
                {
                    //Subtracting the time to get ready for the next frame
                    animation.TimeElapsed -= timeBetweenFrames;
                    //Checking to make sure we are not going over the number of frames
                    if (animation.CurrentFrame < (sprite.SpriteSheet.Animation[animation.CurrentAnimation].Length - 1))
                    {
                        animation.CurrentFrame++;
                    }
                    //Starting back at frame 0
                    else
                    {
                        animation.CurrentFrame = 0;
                    }
                }

                //Setting the rectangle of the sprite sheet to draw
                sprite.SrcRect = sprite.SpriteSheet.Animation[animation.CurrentAnimation][animation.CurrentFrame];
            }
        }
    }
}
