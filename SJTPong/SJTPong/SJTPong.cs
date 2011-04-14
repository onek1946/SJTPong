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

namespace SJTPongGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SJTPong : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Barra _player1 = new Barra(10,300);
        Barra _player2 = new Barra(780,300);
        Pelota _pelota = new Pelota(400,300);
        TimeSpan _timeballmove;

        public SJTPong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.IsFullScreen = false;
            Window.Title = "SJTPong version 0.1";
        
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
            _player1.Load_texture(Content.Load<Texture2D>("Imagenes/barra1"));
            _pelota.Load_texture(Content.Load<Texture2D>("Imagenes/pelota"));
            _player2.Load_texture(Content.Load<Texture2D>("Imagenes/barra2"));
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //Movimiento teclado
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && _player1.Pos.Y<=500 )
                _player1.Pos.Y += 5;

            if (ks.IsKeyDown(Keys.Up) && _player1.Pos.Y >= 0 )
                _player1.Pos.Y -= 5;

            //Detectar colision de pelota con barra
            Boolean condition_bc_y_p1 = _pelota.Pos.Y + _pelota.Size.Y >= _player1.Pos.Y && _pelota.Pos.Y <= _player1.Pos.Y + _player1.Size.Y;
            Boolean condition_bc_y_p2 = _pelota.Pos.Y + _pelota.Size.Y >= _player2.Pos.Y && _pelota.Pos.Y <= _player2.Pos.Y + _player2.Size.Y;
            Boolean condition_bc_x_p1 = _pelota.Pos.X <= (10 + _player1.Size.X) ;
            Boolean condition_bc_x_p2 = _pelota.Pos.X + _pelota.Size.X >= 790 - _player2.Size.X; 
            if ((condition_bc_x_p2&&condition_bc_y_p2)||(condition_bc_y_p1 && condition_bc_x_p1))
            {
                 _pelota.Velocity.X *= -1;           
            }
            else
            {
                //Detectar fin de camino y poner en el medio
                if (_pelota.Pos.X < (10 + _player1.Size.X))
                {
                    _pelota.Pos.X = 400;
                    _pelota.Pos.Y = 300;
                    //Punto para el lado izquierdo
                }

                if (_pelota.Pos.X > 790 - _player2.Size.X)
                {
                    _pelota.Pos.X = 400;
                    _pelota.Pos.Y = 300;
                    //Punto para el lado derecho
                }
            }
            //Colision de pelota con bordes
            if (_pelota.Pos.Y <= 0 || _pelota.Pos.Y +_pelota.Size.Y>= 600)
                _pelota.Velocity.Y *= -1;

            //mover pelota
            if ((_timeballmove += gameTime.ElapsedGameTime) > TimeSpan.FromMilliseconds(1))
            {
                _pelota.Pos.X += (_pelota.Velocity.X); //no es tan asi, despues cambio esto
                _pelota.Pos.Y += (_pelota.Velocity.Y);    
                _timeballmove -= TimeSpan.FromMilliseconds(1);

            }
        
            
            //mover juegador2
            //Aca practicamente estoy haciendo la AI de la PC
            //Me fijo el centro de la pelota e intento ir hacia alla.
            UInt32 next_move = (UInt32)_pelota.Pos.Y + (UInt32)(_pelota.Size.Y/2 ) ;

            if (_pelota.Velocity.X > 0 && next_move > ((UInt32)_player2.Pos.Y + (UInt32)(_player2.Size.Y / 2)) && _player2.Pos.Y < 500)
                _player2.Pos.Y += 5;

            if (_pelota.Velocity.X > 0 && next_move < ((UInt32)_player2.Pos.Y + (UInt32)(_player2.Size.Y / 2)) && _player2.Pos.Y >= 5)
                _player2.Pos.Y -= 5;

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
            
            spriteBatch.Draw(_player1.Textura,_player1.Pos , Color.White);
            spriteBatch.Draw(_player2.Textura, _player2.Pos, Color.White);
            spriteBatch.Draw(_pelota.Textura, _pelota.Pos, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }   
    }
    public class Barra 
    {
        public Texture2D Textura;
        public Vector2 Pos = new Vector2(0,0); 
        //Por ahora el tamaño lo pongo hardcodeado
        public Vector2 Size = new Vector2(10, 100);

        public Barra(UInt32 A,UInt32 B) 
        {
            Pos.X = A; Pos.Y = B;
        }
        public UInt32 Load_texture(Texture2D textura)
        {
            Textura=textura;   
            return 0;
        }
    }
    public class Pelota
    {
        public Texture2D Textura;
        public Vector2 Pos = new Vector2(0,0); 
        //Por ahora el tamaño lo pongo hardcodeado
        public Vector2 Size = new Vector2(10, 10);
        public Vector2 Velocity = new Vector2(0,0);

        public Pelota(UInt32 A=0,UInt32 B=0) 
        {
            Pos.X = A; Pos.Y = B;
            Velocity.X = 5; Velocity.Y = 5;
        }
        public UInt32 Load_texture(Texture2D textura)
        {
            Textura=textura;   
            return 0;
        }
    }


}
