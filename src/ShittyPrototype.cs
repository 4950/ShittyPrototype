﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using ShittyPrototype.src.core;
using ShittyPrototype.src.graphics;
#endregion

namespace ShittyPrototype
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ShittyPrototype : Game
    {
        GraphicsDeviceManager graphics;
        InputManager inputManager;
        SceneManager sceneManager;

        public ShittyPrototype()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            inputManager = InputManager.GetSingleton();
            sceneManager = new SceneManager(graphics);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Entity entity = new Entity();
            RenderComponent renderComp = new RenderComponent();

            renderComp.texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            renderComp.texture.SetData(new Color[] { Color.AliceBlue });

            renderComp.rectangle = new Rectangle(0, 0, 50, 50);

            entity.AddComponent(renderComp);

            sceneManager.Add(entity);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            inputManager.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sceneManager.Render();

            base.Draw(gameTime);
        }
    }
}
