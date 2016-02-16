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
    public class LevelSelectionScreen : Screen
    {
        private Texture2D backButton, backButtonDefault, backButtonSelected;
        private Texture2D confirmButton, confirmButtonDefault, confirmButtonSelected;
        private Texture2D border, borderDefault, borderSelected;
        private Texture2D logo;
        private Texture2D lockScreen;

        private KeyboardState oldState;

        private GameObject downArrow, upArrow;

        private List<Texture2D> levels;

        public int selectedButton { get; private set; }
        public int selectedLevel { get; private set; }
        private bool selectedLevelUnlocked;

        public LevelSelectionScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            selectedButton = 1;

            logo = content.Load<Texture2D>("LevelSelection");
            lockScreen = content.Load<Texture2D>("LockScreen");

            backButtonDefault = content.Load<Texture2D>("Back");
            confirmButtonDefault = content.Load<Texture2D>("Confirm");

            backButtonSelected = content.Load<Texture2D>("Back-Selected");
            confirmButtonSelected = content.Load<Texture2D>("Confirm-Selected");

            backButton = backButtonDefault;
            confirmButton = confirmButtonDefault;

            borderDefault = content.Load<Texture2D>("Border");
            borderSelected = content.Load<Texture2D>("Border-Selected");

            border = borderSelected;

            downArrow = new GameObject(content.Load<Texture2D>("DownArrow"));
            upArrow = new GameObject(content.Load<Texture2D>("UpArrow"));

            downArrow.position = new Vector2(Game1.WindowWidth / 2, (Game1.WindowHeight / 2 - 30) + 110);
            upArrow.position = new Vector2(Game1.WindowWidth / 2, (Game1.WindowHeight / 2 - 30) - 110);

            levels = new List<Texture2D>();
            levels.Add(content.Load<Texture2D>("Level1Icon"));
            levels.Add(content.Load<Texture2D>("Level2Icon"));
            levels.Add(content.Load<Texture2D>("Level3Icon"));
            levels.Add(content.Load<Texture2D>("Level4Icon"));
            levels.Add(content.Load<Texture2D>("Level5Icon"));
            levels.Add(content.Load<Texture2D>("Level6Icon"));
        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
            {
                //Set old button to not selected icon
                if (selectedButton == 0)
                    backButton = backButtonDefault;
                else if (selectedButton == 1)
                    border = borderDefault;
                else confirmButton = confirmButtonDefault;

                selectedButton--;
                if (selectedButton < 0)
                    selectedButton = 2;

                //Set new button to selected icon
                if (selectedButton == 0)
                    backButton = backButtonSelected;
                else if (selectedButton == 1)
                    border = borderSelected;
                else confirmButton = confirmButtonSelected;
            }

            if (newState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
            {
                //Set old button to not selected icon
                if (selectedButton == 0)
                    backButton = backButtonDefault;
                else if (selectedButton == 1)
                    border = borderDefault;
                else confirmButton = confirmButtonDefault;

                selectedButton++;
                selectedButton %= 3;

                //Set new button to selected icon
                if (selectedButton == 0)
                    backButton = backButtonSelected;
                else if (selectedButton == 1)
                    border = borderSelected;
                else confirmButton = confirmButtonSelected;
            }

            if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                upArrow.scale = 1.2f;

                if (selectedButton == 1)
                {
                    selectedLevel++;
                    selectedLevel %= levels.Count;
                }
            }
            else upArrow.scale = 1;

            if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                downArrow.scale = 1.2f;

                if (selectedButton == 1)
                {
                    selectedLevel--;
                    if (selectedLevel < 0)
                        selectedLevel = levels.Count - 1;
                }
            }
            else downArrow.scale = 1;

            //check if level is unlocked
            if (Game1.levelsUnlocked > selectedLevel)
                selectedLevelUnlocked = true;
            else selectedLevelUnlocked = false;

            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                if (selectedButton == 2) //Confirm Button
                {
                    if (selectedLevelUnlocked)
                        screenEvent.Invoke(this, new EventArgs());
                }
                else screenEvent.Invoke(this, new EventArgs());
            }

            if (selectedButton == 1)
            {
                upArrow.visible = true;
                downArrow.visible = true;
            }
            else
            {
                upArrow.visible = false;
                downArrow.visible = false;
            }

            oldState = newState;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //Draw logo
            spritebatch.Draw(logo, new Vector2(Game1.WindowWidth / 2, 25), new Rectangle(0, 0,
                logo.Width, logo.Height), Color.White, 0, new Vector2(logo.Width / 2, logo.Height / 2),
                1, SpriteEffects.None, 1f);

            //Draw text buttons
            spritebatch.Draw(backButton, new Vector2(120, Game1.WindowHeight - 40), new Rectangle(0, 0,
                backButton.Width, backButton.Height), Color.White, 0, new Vector2(backButton.Width / 2, backButton.Height / 2),
                1, SpriteEffects.None, 1f);
            spritebatch.Draw(confirmButton, new Vector2(Game1.WindowWidth - 140, Game1.WindowHeight - 40), new Rectangle(0, 0,
                 confirmButton.Width, confirmButton.Height), Color.White, 0, new Vector2(confirmButton.Width / 2, confirmButton.Height / 2),
                 1, SpriteEffects.None, 1f);

            //Draw level icons
            spritebatch.Draw(levels[selectedLevel], new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 levels[selectedLevel].Width, levels[selectedLevel].Height), Color.White, 0, new Vector2(levels[selectedLevel].Width / 2, levels[selectedLevel].Height / 2),
                 1, SpriteEffects.None, 1f);

            if (!selectedLevelUnlocked)
            {
                //Draw lock screen
                spritebatch.Draw(lockScreen, new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                     lockScreen.Width, lockScreen.Height), Color.White, 0, new Vector2(lockScreen.Width / 2, lockScreen.Height / 2),
                     1, SpriteEffects.None, 1f);
            }

            //Draw borders
            spritebatch.Draw(border, new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 border.Width, border.Height), Color.White, 0, new Vector2(border.Width / 2, border.Height / 2),
                 1, SpriteEffects.None, 1f);

            //Draw Arrows
            downArrow.Draw(spritebatch);
            upArrow.Draw(spritebatch);
        }
    }
}
