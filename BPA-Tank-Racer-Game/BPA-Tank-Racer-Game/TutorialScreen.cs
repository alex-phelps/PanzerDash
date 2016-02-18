using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BPA_Tank_Racer_Game
{
    public class TutorialScreen : GameScreen
    {
        private bool isPopup = true;
        private string popup = "welcome";

        private bool shootingTutDone;
        private bool powerUpTutDone;
        private bool objectiveTutDone;

        private SpriteFont popupFont;
        private SpriteFont popupBigFont;

        private KeyboardState oldState;

        public TutorialScreen(ContentManager content, EventHandler screenEvent) 
            : base(content, screenEvent, "")
        {
            oldState = Keyboard.GetState();

            popupFont = content.Load<SpriteFont>("PopupText");
            popupBigFont = content.Load<SpriteFont>("WinTextFont");

            bulletHandler = new BulletHandler();
            random = new Random();
            unlockContent = false;

            playerTank = new PlayerTank(content, bulletHandler, TankPartType.red, TankPartType.red);
            enemyTank = new AITank(content, bulletHandler, TankPartType.basic, TankPartType.basic,
                playerTank.position + new Vector2(100, 0));

            level = 1;
            Setup(content);
            SoundInit();
        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (!isPopup || firstUpdate)
                base.Update(gametime);

            //Check to see if there should be a popup
            if (!shootingTutDone && cooldownZeroFirstTime)
            {
                isPopup = true;
                popup = "shootingTut";
                shootingTutDone = true;
            }
            else if (!powerUpTutDone && collectedPowerupFirstTime)
            {
                isPopup = true;
                popup = "powerupTut";
                powerUpTutDone = true;
            }
            else if (!objectiveTutDone && reachedObjective)
            {
                isPopup = true;
                popup = "objectiveTut";
                objectiveTutDone = true;
            }

            if (isPopup && newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                if (popup == "welcome")
                {
                    popup = "initTut";
                }
                else if (popup == "initTut")
                {
                    popup = "guiTut";
                }
                else
                {
                    isPopup = false;
                    popup = "";
                }
            }

            oldState = newState;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            //Draw any popups
            if (isPopup)
            {
                if (popup == "welcome")
                {
                    spritebatch.DrawString(popupBigFont, "Welcome to PANZER DASH", 
                        new Vector2(Game1.WindowWidth / 2 - popupBigFont.MeasureString("Welcome to PAZER DASH").X / 2, 50), Color.White);
                    string synopsisText = "The objective of Panzer Dash is \nto be the first tank to make it to the end of \nthe level and destroy the objective." + 
                        "\nAlong the way you must battle \nagainst your opponent by stunning \nthem with your gun while also \navoiding their shots.";
                    spritebatch.DrawString(popupFont, synopsisText, 
                        new Vector2(Game1.WindowWidth / 2 - popupFont.MeasureString(synopsisText).X / 2, 150), Color.White);

                }
                else if (popup == "initTut")
                {
                    spritebatch.DrawString(popupBigFont, "Controls",
                        new Vector2(Game1.WindowWidth / 2 - popupBigFont.MeasureString("Controls").X / 2, 50), Color.White);
                    string synopsisText = "Use the W, A, S, and D keys\nto control your tank.\nUse the left and right\narrow keys to control your turret.";
                    spritebatch.DrawString(popupFont, synopsisText,
                        new Vector2(Game1.WindowWidth / 2 - popupFont.MeasureString(synopsisText).X / 2, 150), Color.White);
                }
                else if (popup == "guiTut")
                {
                    spritebatch.DrawString(popupBigFont, "GUI",
                        new Vector2(Game1.WindowWidth / 2 - popupBigFont.MeasureString("GUI").X / 2, 175), Color.White);
                    string objHealthText = "This is the objective health bar.\nTo win, you must reduce this to zero by shooting\nthe final objective at the end of the level.";
                    string coolDownText = "This is the\ncooldown for\nyour gun.\nYou can only\nshoot when\nit is empty.";
                    spritebatch.DrawString(popupFont, objHealthText,
                        new Vector2(Game1.WindowWidth / 2 - popupFont.MeasureString(objHealthText).X / 2, 50), Color.White);
                    spritebatch.DrawString(popupFont, coolDownText, new Vector2(70, 250), Color.White);
                }
                else if (popup == "shootingTut")
                {
                    spritebatch.DrawString(popupBigFont, "Shooting",
                        new Vector2(Game1.WindowWidth / 2 - popupBigFont.MeasureString("Shooting").X / 2, 50), Color.White);
                    string synopsisText = "Your cooldown has reached 0!\n Press spacebar to fire!";
                    spritebatch.DrawString(popupFont, synopsisText,
                        new Vector2(Game1.WindowWidth / 2 - popupFont.MeasureString(synopsisText).X / 2, 150), Color.White);
                }
                else if (popup == "powerupTut")
                {
                    spritebatch.DrawString(popupBigFont, "Powerups",
                        new Vector2(Game1.WindowWidth / 2 - popupBigFont.MeasureString("Powerups").X / 2, 50), Color.White);
                    string synopsisText = "You collected a powerup! When you collect a powerup,\n you will gain a short, powerfule boost!";
                    spritebatch.DrawString(popupFont, synopsisText,
                        new Vector2(Game1.WindowWidth / 2 - popupFont.MeasureString(synopsisText).X / 2, 150), Color.White);
                    string powerUpText = "This is the\npowerup cooldown\nbar. It shows\n how much longer\nyour powerup\nwill last.";
                    spritebatch.DrawString(popupFont, powerUpText,
                        new Vector2(Game1.WindowWidth - 50 - popupFont.MeasureString(powerUpText).X, 230), Color.White);
                }
                else if (popup == "objectiveTut")
                {
                    spritebatch.DrawString(popupBigFont, "The Objective",
                        new Vector2(Game1.WindowWidth / 2 - popupBigFont.MeasureString("The Objective").X / 2, 50), Color.White);
                    string synopsisText = "You're almost there! All thats left if for you\nto destroy the finish objective!\nGo for it!";
                    spritebatch.DrawString(popupFont, synopsisText,
                        new Vector2(Game1.WindowWidth / 2 - popupFont.MeasureString(synopsisText).X / 2, 150), Color.White);
                }


                spritebatch.DrawString(popupFont, "Press enter to continue.",
                    new Vector2(Game1.WindowWidth / 2 - popupFont.MeasureString("Press enter to continue.").X / 2, 425), Color.White);
            }
        }
    }
}
