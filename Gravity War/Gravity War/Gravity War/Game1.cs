using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gravity_War
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PlanetGenerator planetGenerator;

        public Game1()
        {
            Bullet.radius = 10;
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            int windowX = GraphicsDevice.Viewport.Width;
            int windowY = GraphicsDevice.Viewport.Height;
            planetGenerator = new PlanetGenerator(windowX, windowY);
            planetGenerator.clearImages();
            planetGenerator.loadImage(Content.Load<Texture2D>("bluePlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("brownPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("earthPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("goldPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("moonPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("orangePlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("sunPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("yellowPlanet"));
            Bullet.image = Content.Load<Texture2D>("bullet");
            Random r = new Random();
            for(int a = 0; a < 1000; a++)
            {   
                Bullets.add(new Bullet(new Vector2(/*r.Next(windowX)*/0, ((float)windowY * a / 1000)/*r.Next(windowY)*/), new Vector2((float)r.NextDouble()*0+1, (float)r.NextDouble()*0)));
            }
            Planets.clear();
            planetGenerator.generate(r.Next(15));
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            for (int a = 0; a < Bullets.getBullets().Count; a++ )
            {
                Bullets.getBullets().ElementAt<Bullet>(a).run(Planets.getGravityField(Bullets.getBullets().ElementAt<Bullet>(a).getLocation()));
                if (Planets.collides(Bullets.getBullets().ElementAt<Bullet>(a).getLocation()))
                {
                    Bullets.remove(a);
                    a--;
                }
            }

            if (Bullets.getBullets().Count == 0)
            {
                LoadContent();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            foreach (Planet p in Planets.getPlanets())
            {
                spriteBatch.Draw(p.getImage(), p.getLocation(), null, Color.White, 0f, p.getOrigin(), p.getScale(), SpriteEffects.None, 0f);
            }
            foreach (Bullet b in Bullets.getBullets())
            {
                spriteBatch.Draw(Bullet.image, b.getLocation(), null, Color.White, b.getRotation(), Bullet.origin, Bullet.scale, SpriteEffects.None, 0f);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
