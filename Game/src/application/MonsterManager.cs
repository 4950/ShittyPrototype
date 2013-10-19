﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ShittyPrototype.src.application.core;

namespace ShittyPrototype.src.application
{
    class MonsterManager
    {
        ListEvent<Monster> monsters = new ListEvent<Monster>();

        public ICollection<Monster> Monsters
        {
            get
            {
                return monsters;
            }            
        } 

        public MonsterManager()
        {
            ListEventListener addMListener = new ListEventListener(monsters);
        }

        public void Add(Monster m)
        {
            monsters.Add(m);
        }

        public void Remove(Monster m)
        {
            monsters.Remove(m);
        }

        public void UpdateEntities(GameTime gameTime)
        {
            Random r = new Random();
            int i;
            TimeSpan t = new TimeSpan(0,0,0,0,500);
            foreach (Monster m in monsters)
            {
                if(m.render.timeOfLastDraw + t <= gameTime.TotalGameTime)
                {
                    m.render.timeOfLastDraw = gameTime.TotalGameTime;
                    i = r.Next(0, 101);
                    if (i < 26)
                    {
                        m.render.rectangle.X += 10;
                        m.pos.x = m.render.rectangle.X;
                    }
                    else if (i < 51)
                    {
                        m.render.rectangle.X -= 10;
                        m.pos.x = m.render.rectangle.X;
                    }
                    else if (i < 76)
                    {
                        m.render.rectangle.Y += 10;
                        m.pos.y = m.render.rectangle.Y;
                    }
                    else
                    {
                        m.render.rectangle.Y -= 10;
                        m.pos.y = m.render.rectangle.Y;
                    }
                }
            }
        }

    }
}
