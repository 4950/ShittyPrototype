﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DirtyGame.game.Core.GameStates
{
    public class Spawner
    {
        //Data about the spawner
        //Location
        private int xPosition;
        private int yPosition;
        //? ? ? ? ?
        private Rectangle spawnerRectangle;
        //Monster Type
        private string monsterType;
        //Monster Weapon
        private string monsterWeapon;
        //Number of Monsters
        private int numberOfMonsters;
        //TimeSpan for Monsters to Spawn
        private TimeSpan timePerSpawn;
        //Modifier
        private string modifier;

        #region Properties
        public int XPosition
        {
            get
            {
                return xPosition;
            }
            set
            {
                xPosition = value;
            }
        }
        public int YPosition
        {
            get
            {
                return yPosition;
            }
            set
            {
                yPosition = value;
            }
        }
        public Rectangle SpawnerRectangle
        {
            get
            {
                return spawnerRectangle;
            }
            set
            {
                spawnerRectangle = value;
            }
        }
        public string MonsterType
        {
            get
            {
                return monsterType;
            }
            set
            {
                monsterType = value;
            }
        }
        public string MonsterWeapon
        {
            get
            {
                return monsterWeapon;
            }
            set
            {
                monsterWeapon = value;
            }
        }
        public int NumberOfMonsters
        {
            get
            {
                return numberOfMonsters;
            }
            set
            {
                numberOfMonsters = value;
            }
        }
        public TimeSpan TimePerSpawn
        {
            get
            {
                return timePerSpawn;
            }
            set
            {
                timePerSpawn = value;
            }
        }
        public string Modifier
        {
            get
            {
                return modifier;
            }
            set
            {
                modifier = value;
            }
        }
        #endregion

        public Spawner()
        {
        }
        public Spawner(int xPos, int yPos, Rectangle spawnerRec, string mType, string mWeapon, int numMonsters, TimeSpan tPerSpawn, string mod)
        {
            this.xPosition = xPos;
            this.yPosition = yPos;
            this.spawnerRectangle = spawnerRec;
            this.monsterType = mType;
            this.monsterWeapon = mWeapon;
            this.numberOfMonsters = numMonsters;
            this.timePerSpawn = tPerSpawn;
            this.modifier = mod;
        }
    }
}