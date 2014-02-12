﻿using DirtyGame.game.Core.Components;
using DirtyGame.game.Core.Components.Render;
using DirtyGame.game.Core.Systems.Util;
using EntityFramework;
using EntityFramework.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirtyGame.game.Map;
using DirtyGame.game.SGraphics;

namespace DirtyGame.game.Core.Systems.Monster
{
    public class AISystem : EntitySystem
    {
        Random r = new Random();
        private Dirty game;
        private EntityFactory entityFactory;
        public float totaltime;
        private Physics physics;
        private Renderer renderer;
        
        
        //Current goal: Make monsters of different types rush towards each other.
        // If no monster of another type is nearby... wander.

        public AISystem(Dirty game, EntityFactory entityFactory, Physics physics, Renderer renderer)
            : base(SystemDescriptions.MonsterSystem.Aspect, SystemDescriptions.MonsterSystem.Priority)
        {
            this.game = game;
            this.entityFactory = entityFactory;
            this.physics = physics;
            this.renderer = renderer;
        }

        public override void ProcessEntities(IEnumerable<Entity> entities, float dt)
        {
            Vector2 playerPos = game.player.GetComponent<SpatialComponent>().Center;
            totaltime += (float)Math.Floor(dt * 1000);

            foreach (Entity e in entities)
            {
                if (e.HasComponent<InventoryComponent>())//has weapon
                {
                    Entity weapon = e.GetComponent<InventoryComponent>().CurrentWeapon;
                    WeaponComponent wc = weapon.GetComponent<WeaponComponent>();

                    Vector2 monsterPos = e.GetComponent<SpatialComponent>().Center;

                    if(wc.WeaponName == "SnipWeapon")
                    {
                        if (e.GetComponent<SnipComponent>().Locked == true)
                        {
                            e.GetComponent<SnipComponent>().Locked = false;
                            
                            game.weaponSystem.FireWeapon(weapon, e, playerPos);
                        }
                    }

                    else if (wc.Type != WeaponComponent.WeaponType.AOE)
                    {
                        double dist = getDistance(monsterPos.X, monsterPos.Y, playerPos.X, playerPos.Y);
                        if (wc.WeaponName == "BomberWeapon")
                        {
                            if (dist < wc.Range)
                            {
                                game.weaponSystem.FireWeapon(weapon, e, playerPos);
                            }
                        }
                        else
                        {
                            game.weaponSystem.FireWeapon(weapon, e, playerPos);
                        }

                    }
                }
            }
        }

        public override void OnEntityAdded(Entity e)
        {
            // do nothing
        }

        public override void OnEntityRemoved(Entity e)
        {
            // do nothing
        }


        //Sniper Movement
        public Vector2 snipMovement(Entity e, float dt)
        {
            SpatialComponent spatial = e.GetComponent<SpatialComponent>();
            SnipComponent snip = e.GetComponent<SnipComponent>();
            Boolean playerFound = false;
            double[] vel = new double[2];
            double dist;

            Vector2 Target = new Vector2(game.player.GetComponent<SpatialComponent>().Center.X, game.player.GetComponent<SpatialComponent>().Center.Y);
            if((dist = getDistance(Target.X, Target.Y, spatial.Center.X, spatial.Center.Y)) <= snip.Range) //Player Within Range
            {
                    playerFound = true;
                    
                    Vector2 dir = (Target - spatial.Center + snip.Offset);
                    dir.Normalize();

                    if (snip.LaserPres == true)
                    {
                        game.world.EntityMgr.GetEntity(snip.Laser).GetComponent<SpriteComponent>().Scale = (float)dist / (float)snip.Range;

                        if (game.world.EntityMgr.GetEntity(snip.Laser).GetComponent<LaserComponent>().PlayerPres == true)
                        {
                            game.world.EntityMgr.GetEntity(snip.Laser).GetComponent<SpatialComponent>().ConstantRotation = laserFollow(e, game.world.EntityMgr.GetEntity(snip.Laser), dt, Target - spatial.Center);
                        }

                        
                    }

                    if (snip.LaserPres == false && snip.IsRunning == false)
                    {
                        snip.LaserPres = true;
                        Entity laser = game.entityFactory.CreateLaserEntity("laser", "sniplaser", spatial.Center, dir, (float)dist / (float)snip.Range);
                        laser.Refresh();
                        snip.Laser = laser.Id;
                      
                        
                    }


                    else if (dist <= snip.FleeDistance) //Snip Run
                    {
                        snip.IsRunning = true;
                        if (snip.LaserPres == true)
                        {
                            game.world.DestroyEntity(game.world.EntityMgr.GetEntity(snip.Laser));
                            snip.LaserPres = false;
                        }

                        vel = getChaseVector(-dir.X, dir.Y, spatial.Center.X, spatial.Center.Y);



                    }

                    else if (dist > snip.FleeDistance) //Snip Camp
                    {
                        snip.IsRunning = false;
                        vel[0] = 0;
                        vel[1] = 0;
                    }

                }

            

            if (playerFound == false && snip.LaserPres == true)
            {
                game.world.DestroyEntity(game.world.EntityMgr.GetEntity(snip.Laser));
                snip.LaserPres = false;
            }



            setDirection(vel, e);
            return new Vector2((float)vel[0], (float)vel[1]);
        }



        //Make monsters of different types rush towards each other.
        // If no monster of another type is nearby... wander.
        public Vector2 calculateMoveVector(IEnumerable<Entity> entities, Entity m, float dt)
        {
            bool[,] collMap = renderer.ActiveMap.getPassabilityMap();
            int mapWidth = renderer.ActiveMap.getPixelWidth() / 32;
            int mapHeight = renderer.ActiveMap.getPixelHeight() / 32;

            double[] vel = new double[2];
                String type = m.GetComponent<PropertyComponent<String>>("MonsterType").value;

            if (type == "Flametower")
            {
                //don't move
            }

            else if (type == "SnipMonster")
            {
                return snipMovement(m, dt);
            }

            else if (type == "WallHugger")
            {
                return WallHuggerMovement(m, collMap, mapWidth, mapHeight) * 5 * (m.GetComponent<StatsComponent>().MoveSpeed / 100.0f);
            }

            else if (m.HasComponent<InventoryComponent>())//has weapon
            {
                Entity weapon = m.GetComponent<InventoryComponent>().CurrentWeapon;
                WeaponComponent wc = weapon.GetComponent<WeaponComponent>();
                if (wc.Type == WeaponComponent.WeaponType.Landmine)
                {
                    vel = seekPlayer(entities, m, 0, 200, false, collMap);//if player is close, run

                    if (vel[0] == vel[1] && vel[0] == 0)//player not in sight or in range, wander
                        vel = randDir();
                }
                if (wc.Type == WeaponComponent.WeaponType.Ranged)
                {
                    vel = seekPlayer(entities, m, 0, 200, false, collMap);//if player is close, run
                    if (vel[0] == vel[1] && vel[0] == 0)
                        seekPlayer(entities, m, (int)wc.Range - 50, 600, true, collMap);//if player is not within weapon range but in sight range, chase
                    if (vel[0] == vel[1] && vel[0] == 0)//player not in sight or in range, wander
                        vel = randDir();
                }
                else if (wc.Type == WeaponComponent.WeaponType.Melee)
                {
                    vel = seekPlayer(entities, m, (int)wc.Range, 600, true, collMap);//if player is not within weapon range but in sight range, chase
                    if (vel[0] == vel[1] && vel[0] == 0)//player not in sight or in range, wander
                        vel = randDir();
                }
            }
            else//old ai
            {
                vel = seekPlayer(entities, m, 0, 600, true, collMap);

                if (vel[0] == vel[1] && vel[0] == 0)
                    vel = randDir();

            }

            setDirection(vel, m);

            return new Vector2((float)vel[0], (float)vel[1]) * 5 * (m.GetComponent<StatsComponent>().MoveSpeed / 100.0f);
        }

        
        private double[] seekPlayer(IEnumerable<Entity> entities, Entity m, int minrange, int maxrange, bool seek, bool[,] collMap)
        {
            foreach (Entity e in entities)
            {
                if (e.HasComponent<PlayerComponent>())
                {

                    int otherX = (int)e.GetComponent<SpatialComponent>().Position.X;
                    int otherY = (int)e.GetComponent<SpatialComponent>().Position.Y;
                    if (getDistance(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y, otherX, otherY) < maxrange &&
                        getDistance(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y, otherX, otherY) > minrange)
                    {
                        double[] chaseVector;

                        if (seek)
                        {
                            bool wall = false;
                            //List<Entity> rayCast = physics.RayCast(new Vector2(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y), new Vector2(otherX, otherY));
                            //foreach (Entity w in rayCast)
                            //{
                            //    if (w.GetComponent<BorderComponent>() != null)
                            //    {
                            //        wall = true;
                            //        break;
                            //    }
                            //    else
                            //    {


                            //    }
                            //}

                            chaseVector = getChaseVector(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y, otherX, otherY);
                            //if (m.GetComponent<MovementComponent>().prevHorizontal != 0)
                            //{
                            //    m.GetComponent<MovementComponent>().prevVelocity = new Vector2(0, 0);
                            //}
                            MovementComponent oldMovement = m.GetComponent<MovementComponent>();

                            int monsterX = (int)Math.Floor(m.GetComponent<SpatialComponent>().Center.X / 32);
                            int monsterY = (int)Math.Floor(m.GetComponent<SpatialComponent>().Center.Y / 32);
                            if (Math.Abs(chaseVector[0]) > Math.Abs(chaseVector[1]))
                            {
                                if (chaseVector[0] > 0)
                                {
                                    //wall = (collMap[monsterX + 1, monsterY - 1] || collMap[monsterX + 1, monsterY] || collMap[monsterX + 1, monsterY + 1]);
                                    wall = collMap[monsterX + 1, monsterY];
                                }
                                else
                                {
                                    //wall = (collMap[monsterX - 1, monsterY - 1] || collMap[monsterX - 1, monsterY] || collMap[monsterX - 1, monsterY + 1]);
                                    wall = collMap[monsterX - 1, monsterY];
                                }
                            }
                            else
                            {
                                if (chaseVector[1] > 0)
                                {
                                    //wall = (collMap[monsterX - 1, monsterY + 1] || collMap[monsterX, monsterY + 1] || collMap[monsterX + 1, monsterY + 1]);
                                    wall = collMap[monsterX, monsterY + 1];
                                }
                                else
                                {
                                    //wall = (collMap[monsterX - 1, monsterY - 1] || collMap[monsterX, monsterY - 1] || collMap[monsterX + 1, monsterY - 1]);
                                    wall = collMap[monsterX, monsterY - 1];
                                }
                            }

                            if (wall)
                            {
                                if(Math.Abs(chaseVector[0]) > Math.Abs(chaseVector[1]))
                                {
                                    if (chaseVector[1] > 0)
                                    {
                                        chaseVector = WalkAroundWallVertical(m, oldMovement, "up");
                                    }
                                    else 
                                    {
                                        chaseVector = WalkAroundWallVertical(m, oldMovement, "down");
                                    }
                                   
                                }
                                else
                                {
                                    if (chaseVector[0] > 0)
                                    {
                                        chaseVector = WalkAroundWallHorizontal(m, oldMovement, "right");

                                    }
                                    else
                                    {
                                        chaseVector = WalkAroundWallHorizontal(m, oldMovement, "left");
                                    }
                                }
                            }
                            else
                            {
                                
                            }
                        }
                        else
                        {
                            chaseVector = getChaseVector(otherX, otherY, m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y);
                        }
                        return chaseVector;
                    }
                }
            }

            return new double[2];
        }
        private void setDirection(double[] vel, Entity m)
        {
            DirectionComponent direction = m.GetComponent<DirectionComponent>();


            if (vel[0] == 0 && vel[1] == 0)
            {
                direction.Heading = "Down";
                m.GetComponent<MovementComponent>().Vertical = 0;
                m.GetComponent<MovementComponent>().Horizontal = 0;
                AnimationComponent animation = new AnimationComponent();
                animation.CurrentAnimation = "Idle" + direction.Heading;
                m.AddComponent(animation);
                m.Refresh();
            }

            else if (Math.Abs(vel[0]) > Math.Abs(vel[1]))
            {
                if (vel[0] > 0)
                {
                    direction.Heading = "Right";
                    m.GetComponent<MovementComponent>().Horizontal = 1;
                    AnimationComponent animation = new AnimationComponent();
                    animation.CurrentAnimation = "Walk" + direction.Heading;
                    m.AddComponent(animation);
                    m.Refresh();
                }
                else if (vel[0] < 0)
                {
                    direction.Heading = "Left";
                    m.GetComponent<MovementComponent>().Horizontal = -1;
                    AnimationComponent animation = new AnimationComponent();
                    animation.CurrentAnimation = "Walk" + direction.Heading;
                    m.AddComponent(animation);
                    m.Refresh();
                }
            }
            else
            {
                if (vel[1] > 0)
                {
                    direction.Heading = "Down";
                    m.GetComponent<MovementComponent>().Vertical = 1;
                    AnimationComponent animation = new AnimationComponent();
                    animation.CurrentAnimation = "Walk" + direction.Heading;
                    m.AddComponent(animation);
                    m.Refresh();
                }
                else if (vel[1] < 0)
                {
                    direction.Heading = "Up";
                    m.GetComponent<MovementComponent>().Vertical = -1;
                    AnimationComponent animation = new AnimationComponent();
                    animation.CurrentAnimation = "Walk" + direction.Heading;
                    m.AddComponent(animation);
                    m.Refresh();
                }
            }
        }

        

        private float laserFollow(Entity Enemy, Entity e, float dt, Vector2 target)
        {
            float rotation = 2f;
            float angle = e.GetComponent<SpriteComponent>().Angle;
            float tarangle = (float)Math.Atan2(-target.X, target.Y);

            if (e.GetComponent<TimeComponent>().timeofLock == 0)
            {
                e.GetComponent<TimeComponent>().timeofLock = totaltime;
            }


            if (totaltime - e.GetComponent<TimeComponent>().timeofLock > 4000)
            {

                e.GetComponent<SpatialComponent>().ConstantRotation = e.GetComponent<SpatialComponent>().DefaultRotationCons;
                e.GetComponent<LaserComponent>().PlayerPres = false;
                e.GetComponent<LaserComponent>().Reset = true;
            }

            if (e.GetComponent<LaserComponent>().LockedOn == true)
            {
                //fire

                Enemy.GetComponent<SnipComponent>().Locked = true;
                e.GetComponent<LaserComponent>().LockedOn = false;
                e.GetComponent<SpatialComponent>().ConstantRotation = e.GetComponent<SpatialComponent>().DefaultRotationCons;
                e.GetComponent<LaserComponent>().PlayerPres = false;
                e.GetComponent<LaserComponent>().Reset = true;
            }

            


            return rotation;
        }

        private double[] randDir()
        {
            int randInt;
            randInt = r.Next(0, 101);
            double[] randDir = new double[2];
            if (randInt < 26)
            {
                randDir[0] = 1.0;
                randDir[1] = 0.0;
            }
            else if (randInt < 51)
            {
                randDir[0] = -1.0;
                randDir[1] = 0.0;
            }
            else if (randInt < 76)
            {
                randDir[0] = 0.0;
                randDir[1] = 1.0;
            }
            else
            {
                randDir[0] = 0.0;
                randDir[1] = -1.0;
            }

            return randDir;
        }


        private double getDistance(double x, double y, double ox, double oy)
        {
            return Math.Sqrt(
                (Math.Pow(ox - x, 2.0))
                + (Math.Pow(oy - y, 2.0))
                );
        }

        private double[] getChaseVector(double x, double y, double ox, double oy)
        {
            double[] vect = new double[2];
            double dx = ox - x;
            double dy = oy - y;
            double angle = Math.Atan2(dy, dx); // This is opposite y angle.
            vect[0] = Math.Cos(angle);
            vect[1] = Math.Sin(angle);
            return vect;
        }

        private double[] WalkAroundWallVertical(Entity m, MovementComponent oldVector, string direction)
        {
            double[] chaseVector = new double[2];
            if (direction == "up") //Target is above us
            {
                if (oldVector.prevVertical <= 0) //We were moving down before
                {
                    //keep moving down
                    chaseVector = getChaseVector(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y, m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y + 32);
                    oldVector.prevHorizontal = (float)chaseVector[0];
                    oldVector.prevVertical = (float)chaseVector[1];
                }
                else
                {
                    // Use Previous Movement Vector
                    chaseVector[0] = oldVector.prevHorizontal;
                    chaseVector[1] = oldVector.prevVertical;
                }
            }
            if (direction == "down") //Tearget is below us
            {
                if (oldVector.prevVertical >= 0) //We were moving up before
                {
                    //Keep moving up
                    chaseVector = getChaseVector(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y, m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y - 32);
                    oldVector.prevHorizontal = (float)chaseVector[0];
                    oldVector.prevVertical = (float)chaseVector[1];
                }
                else
                {
                    // Use Previous Movement Vector
                    chaseVector[0] = oldVector.prevHorizontal;
                    chaseVector[1] = oldVector.prevVertical;
                }
            }

            return chaseVector;
        }

        private double[] WalkAroundWallHorizontal(Entity m, MovementComponent oldVector, string direction)
        {
            double[] chaseVector = new double[2];
            if (direction == "left") //Target is to our left
            {
                if (oldVector.prevHorizontal >= 0) //We were moving right before
                {
                    //Keep moving right
                    chaseVector = getChaseVector(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y, m.GetComponent<SpatialComponent>().Position.X - 32, m.GetComponent<SpatialComponent>().Position.Y);
                    oldVector.prevHorizontal = (float)chaseVector[0];
                    oldVector.prevVertical = (float)chaseVector[1];
                }
                else
                {
                    // Use Previous Movement Vector
                    chaseVector[0] = oldVector.prevHorizontal;
                    chaseVector[1] = oldVector.prevVertical;
                }

            }
            if(direction == "right") //Target is to our right
            {
                if (oldVector.prevHorizontal <= 0) //We were moving left before
                {
                    //keep moving left
                    chaseVector = getChaseVector(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y, m.GetComponent<SpatialComponent>().Position.X + 32, m.GetComponent<SpatialComponent>().Position.Y);
                    oldVector.prevHorizontal = (float)chaseVector[0];
                    oldVector.prevVertical = (float)chaseVector[1];
                }
                else if (oldVector.prevHorizontal != 1)
                {
                    chaseVector = getChaseVector(m.GetComponent<SpatialComponent>().Position.X, m.GetComponent<SpatialComponent>().Position.Y, m.GetComponent<SpatialComponent>().Position.X - 32, m.GetComponent<SpatialComponent>().Position.Y);
                    oldVector.prevHorizontal = (float)chaseVector[0];
                    oldVector.prevVertical = (float)chaseVector[1];
                }
                else
                {
                    // Use Previous Movement Vector
                    chaseVector[0] = oldVector.prevHorizontal;
                    chaseVector[1] = oldVector.prevVertical;
                }
            }
            return chaseVector;
        }

        private Vector2 WallHuggerMovement(Entity m, bool[,] collMap, int mapWidth, int mapHeight)
        {
            int monsterX = (int) Math.Floor(m.GetComponent<SpatialComponent>().Center.X / 32);
            int monsterY = (int)Math.Floor(m.GetComponent<SpatialComponent>().Center.Y / 32);
            
            Vector2 velocity = new Vector2();
            bool t = collMap[monsterX, Math.Max(monsterY - 1, 0)];
            bool b = collMap[monsterX, Math.Min(monsterY + 1, mapHeight-1)];
            bool l = collMap[Math.Max(monsterX - 1, 0), monsterY];
            bool r = collMap[Math.Min(monsterX + 1, mapWidth-1), monsterY];
            Random rand = new Random();
            
            if (r && l)
            {
                if (b)
                {
                    //Move up
                    velocity = new Vector2(0.0f, -1.0f);
                }
                if (t)
                {
                    //Move Down
                    velocity = new Vector2(0.0f, 1.0f);
                }
                //Move up or down
                if (rand.NextDouble() > .5)
                {
                    velocity = new Vector2(0.0f, -1.0f);
                }
                else
                {
                    velocity = new Vector2(0.0f, 1.0f);
                }
            }
            else if (t && b)
            {
                if (l)
                {
                    //Move right
                    velocity = new Vector2(1.0f, 0.0f);
                }
                if (r)
                {
                    //Move left
                    velocity = new Vector2(-1.0f, 0.0f);
                }
                //Move left or right
                if (rand.NextDouble() > .5)
                {
                    velocity = new Vector2(-1.0f, 0.0f);
                }
                else
                {
                    velocity = new Vector2(1.0f, 0.0f);
                }
            }
            else if (b && l)
            {
                //Move up
                velocity = new Vector2(0.0f, -1.0f);
            }
            else if (b && r)
            {
                //Moveleft
                velocity = new Vector2(-1.0f, 0.0f);
            }
            else if (t && l)
            {
                //Move right
                velocity = new Vector2(1.0f, 0.0f);
            }
            else if (t && r)
            {
                //Move down 
                velocity = new Vector2(0.0f, 1.0f);
            }
            else if (l)
            {

                //Move up
                velocity = new Vector2(0.0f, -1.0f);
            }
            else if (r)
            {
                //Move Down
                velocity = new Vector2(0.0f, 1.0f);
                
            }
            else if (b)
            {
                //Move left 
                velocity = new Vector2(-1.0f, 0.0f);
            }
            else if(t)
            {
                //Move Right
                velocity = new Vector2(1.0f, 0.0f);
            }
            else
            {
                bool tr = collMap[Math.Min(monsterX + 1, mapWidth-1), Math.Max(monsterY - 1, 0)];
                bool tl = collMap[Math.Max(monsterX - 1, 0), Math.Max(monsterY - 1, 0)];
                bool bl = collMap[Math.Max(monsterX - 1, 0), Math.Min(monsterY + 1, mapHeight-1)];
                bool br = collMap[Math.Min(monsterX + 1, mapWidth-1), Math.Min(monsterY + 1, mapHeight-1)];
                MovementComponent oldMovement = m.GetComponent<MovementComponent>();
                if (tr)
                {
                    //Move Up or right
                    if (oldMovement.prevVertical == 0)
                    {
                        velocity = new Vector2(0.0f, -1.0f);
                    }
                    else //if(oldMovement.prevVertical>0)
                    {
                        velocity = new Vector2(1.0f, 0.0f);
                    }
                }
                else if (tl)
                {
                    //Move up or left
                    if (oldMovement.prevVertical == 0)
                    {
                        velocity = new Vector2(0.0f, -1.0f);
                    }
                    else //if(oldMovement.prevVertical>0)
                    {
                        velocity = new Vector2(-1.0f, 0.0f);
                    }
                }
                else if (bl)
                {
                    //Move down or left
                    if (oldMovement.prevVertical == 0)
                    {
                        velocity = new Vector2(0.0f, 1.0f);
                    }
                    else //if(oldMovement.prevVertical<0)
                    {
                        velocity = new Vector2(-1.0f, 0.0f);
                    }
                }
                else if (br)
                {
                    //Move down or right
                    if (oldMovement.prevVertical == 0)
                    {
                        velocity = new Vector2(0.0f, 1.0f);
                    }
                    else //if (oldMovement.prevVertical < 0)
                    {
                        velocity = new Vector2(1.0f, 0.0f);
                    }
                }
                else
                {
                    //go find a wall//go find a wall
                    velocity = new Vector2(-1.0f, 0.0f);
                }
            }

            m.GetComponent<MovementComponent>().prevHorizontal = velocity.X;
            m.GetComponent<MovementComponent>().prevVertical = velocity.Y;
            return velocity;
        }
        private bool WallCheck(List<Entity> list)
        {
            if (list.Count != 0)
            {
                foreach (Entity e in list)
                {
                    if (e.GetComponent<BorderComponent>() != null)
                    {
                        return true;

                    }
                }
                return false;
            }
            return false;
        }
    }
}
