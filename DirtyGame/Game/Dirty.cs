﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using DirtyGame.game.Core;
using DirtyGame.game.SGraphics;
using DirtyGame.game.Systems;
using EntityFramework;
using EntityFramework.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using EntityFramework.Systems;
using DirtyGame.game.Core.Systems.Monster;

#endregion

namespace DirtyGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Dirty : Game
    {

        private World world;
        private GraphicsDeviceManager graphics;
        private Renderer renderer;
        private EntityFactory entityFactory;
        private ResourceManager resourceManager;
        private AISystem aiSystem;

        public Dirty()
        {
            graphics = new GraphicsDeviceManager(this);
            resourceManager = new ResourceManager(Content);                       
            renderer = new Renderer(graphics, new Camera());
            world = new World();
            entityFactory = new EntityFactory(world.EntityMgr, resourceManager);
            aiSystem = new AISystem();
            world.AddSystem(new SpriteRenderSystem(renderer));
            world.AddSystem(new SpawnerSystem(entityFactory));
            world.AddSystem(new MonsterSystem(aiSystem));

            Entity e = entityFactory.CreateTestEntity();
            e.Refresh();
            e = entityFactory.CreateTestEntity();
            e.Refresh();
            e = entityFactory.CreateSpawner(100, 100, resourceManager.GetResource<Texture2D>("monster2"), new Rectangle(0, 0, 46, 46), 30, new TimeSpan(0, 0, 0, 0, 1000));
            e.Refresh();
            e = entityFactory.CreateSpawner(300, 100, resourceManager.GetResource<Texture2D>("monster"), new Rectangle(0, 0, 46, 46), 30, new TimeSpan(0, 0, 0, 0, 2000));
            e.Refresh();
            e = entityFactory.CreateSpawner(100, 300, resourceManager.GetResource<Texture2D>("monster3"), new Rectangle(0, 0, 46, 46), 30, new TimeSpan(0, 0, 0, 0, 3000));
            e.Refresh();
            e = entityFactory.CreateSpawner(300, 300, resourceManager.GetResource<Texture2D>("monster4"), new Rectangle(0, 0, 46, 46), 30, new TimeSpan(0, 0, 0, 0, 500));
            e.Refresh();
        }

        protected override void LoadContent()
        {
        
        }

        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            world.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            renderer.Render();
            base.Draw(gameTime);
        }
    }
}
