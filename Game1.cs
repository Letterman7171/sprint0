using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using System;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using MonoGameLibrary.GameObjects;

namespace sprint0
{
    public class Game1 : Core
    {

        /*
         * 
         * 
         * INSTANCE MEMBERS
         * 
         * 
         */

        private Player playerOne;
        private Player playerTwo;
        private PlayerController controlOne;
        private PlayerController controlTwo;
        SpriteFont font;
        private AnimatedSprite player1Sprite;
        private Sprite player2Sprite;
        private AnimatedSprite forwardSprite;
        private AnimatedSprite backwardSprite;
        private AnimatedSprite leftSprite;
        private AnimatedSprite rightSprite;

        public Game1() : base("Funnee Game", 1280, 720, false)
        {

        }
        /*~~~~~~~~~~~~~~~~~~
         * 
         * 
         *    MY METHODS
         * 
         * 
         *~~~~~~~~~~~~~~~~~~
         */

        //Finds the angle between two vectors
        protected float PointAt(Vector2 start, Vector2 end)
        {
            Vector2 direction = start - end;

            float angle = (float)(Math.PI / 2) + (float)Math.Atan2(direction.Y, direction.X);

            return angle;
        }

        protected void GenericAnimationHandler(Player curPlayer)
        {
            //New Position is for Centering the player
            Vector2 newPos;
            //The last position before it updates
            Vector2 prevPos = curPlayer.delta;

            //For player one, the change in position determines which way the sprite faces
            if (curPlayer.playerIndex == PlayerIndex.One && curPlayer.animatedPlayerSprite != null)
            {
                float spriteOffsetX = curPlayer.animatedPlayerSprite.Width / 2;
                float spriteOffsetY = curPlayer.animatedPlayerSprite.Height / 2;

                newPos = new Vector2(curPlayer.curPos.X - spriteOffsetX, curPlayer.curPos.Y - spriteOffsetY);
                if ((prevPos.X) < curPlayer.curPos.X)
                {
                    rightSprite.Draw(SpriteBatch, newPos);
                    curPlayer.animatedPlayerSprite = rightSprite;
                }
                else if ((prevPos.X) > curPlayer.curPos.X)
                {
                    leftSprite.Draw(SpriteBatch, newPos);
                    curPlayer.animatedPlayerSprite = leftSprite;
                }
                else if (prevPos.Y > curPlayer.curPos.Y)
                {
                    backwardSprite.Draw(SpriteBatch, newPos);
                    curPlayer.animatedPlayerSprite = backwardSprite;
                }
                else if (prevPos.Y < curPlayer.curPos.Y)
                {
                    forwardSprite.Draw(SpriteBatch, newPos);
                    curPlayer.animatedPlayerSprite = forwardSprite;
                }
                else
                { 
                    //If no direction has changed, use the same sprite again
                    curPlayer.animatedPlayerSprite.Draw(SpriteBatch, newPos);
                }
            }
            else
            {
                //For player 2, set the rotation to face the cursor and update position
                newPos = new Vector2(curPlayer.curPos.X, curPlayer.curPos.Y);
                MouseInfo playerMouse = controlTwo.input.Mouse;
                Vector2 mousePos = new Vector2(playerMouse.Position.X, playerMouse.Position.Y);
                curPlayer.regPlayerSprite.Rotation = PointAt(newPos, mousePos);
                curPlayer.regPlayerSprite.Draw(SpriteBatch, newPos);
            }
        }

        /*~~~~~~~~~~~~~~~~~~
         * 
         * 
         * BUILT IN METHODS
         * 
         * 
         *~~~~~~~~~~~~~~~~~~
         */
        protected override void Initialize()
        {
            
            playerOne = new Player(PlayerIndex.One, player1Sprite, new Rectangle(0,0,100,100));
            playerTwo = new Player(PlayerIndex.Two, player2Sprite, new Rectangle(base.Window.ClientBounds.Width, base.Window.ClientBounds.Height, 100, 100));
            controlOne = new PlayerController(playerOne);
            controlTwo = new PlayerController(playerTwo);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Load Font from Content
            font = Content.Load<SpriteFont>("fonts/newFont");

            //  Create a TextureAtlas instance from the atlas
            TextureAtlas atlas = TextureAtlas.FromFile(Content, "sprites/testatlas.xml");

            // add the player region to the atlas and assigns to player1
            forwardSprite = atlas.CreateAnimatedSprite("player-animationFORWARD");
            forwardSprite.Scale = new Vector2(4.0f, 4.0f);
            backwardSprite = atlas.CreateAnimatedSprite("player-animationBACKWARD");
            backwardSprite.Scale = new Vector2(4.0f, 4.0f);
            rightSprite = atlas.CreateAnimatedSprite("player-animationRIGHT");
            rightSprite.Scale = new Vector2(4.0f, 4.0f);
            leftSprite = atlas.CreateAnimatedSprite("player-animationLEFT");
            leftSprite.Scale = new Vector2(4.0f, 4.0f);
            playerOne.animatedPlayerSprite = forwardSprite;

            //adds the second player region to the atlas and assigns to player2
            Sprite player2Texture = atlas.CreateSprite("player2");
            player2Texture.Scale = new Vector2(4.0f, 4.0f);
            player2Texture.CenterOrigin();
            playerTwo.regPlayerSprite = player2Texture;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            /*~~~~~~~~~~~~~~~~~~~
             * 
             *  PLAYER CONTROL
             * 
             *~~~~~~~~~~~~~~~~~~~
             */

            //Mouse Input
            controlOne.Update(gameTime);

            //Keyboard / Gamepad Input (Static / Grid)
            controlTwo.Update(gameTime);


            // Keyboard / Gamepad Input (Non Static)
            /*
             * NOT IMPLEMENTED
             */


            //Updates the animation of all sprites simutaneously for consistency
            backwardSprite.Update(gameTime);
            forwardSprite.Update(gameTime);
            leftSprite.Update(gameTime);
            rightSprite.Update(gameTime);

            /*
             * Included, DO NOT TOUCH
             */

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the sprite batch to prepare for rendering.
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            /* ------------------------
             * 
             * DRAWING TEXT TO SCREEN
             * 
             * ------------------------
             */
            SpriteBatch.DrawString(font, "Credits:", new Vector2(15, 400), Color.Black);
            SpriteBatch.DrawString(font, "Credits:", new Vector2(10, 400), Color.White);
            SpriteBatch.DrawString(font, "Joseph Painter", new Vector2(15, 500), Color.Black);
            SpriteBatch.DrawString(font, "Joseph Painter", new Vector2(10, 500), Color.White);
            SpriteBatch.DrawString(font, "Monogame Tutorials", new Vector2(15, 600), Color.Black);
            SpriteBatch.DrawString(font, "Monogame Tutorials", new Vector2(10, 600), Color.White);

            SpriteBatch.DrawString(font, "Arrow Keys Moves Player 1", new Vector2(425, 0), Color.Black);
            SpriteBatch.DrawString(font, "Arrow Keys Moves Player 1", new Vector2(420, 0), Color.White);
            SpriteBatch.DrawString(font, "Mouse Moves Player 2", new Vector2(595, 100), Color.Black);
            SpriteBatch.DrawString(font, "Mouse Moves Player 2", new Vector2(590, 100), Color.White);


            // Given the change in direction, play the appropriate animation
            GenericAnimationHandler(playerOne);
            GenericAnimationHandler(playerTwo);

            // Always end the sprite batch when finished.
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        


        
    }
}
