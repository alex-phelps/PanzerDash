using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PanzerDash
{
    public class CareerEnemySelectionScreen : Screen
    {
        private Texture2D backButton, backButtonDefault, backButtonSelected;
        private Texture2D confirmButton, confirmButtonDefault, confirmButtonSelected;
        private Texture2D border, borderDefault, borderSelected;
        private Texture2D logo;
        private Texture2D lockScreen;

        private Texture2D basicBase, basicGun, desertBase, desertGun, jungleBase, jungleGun,
            redBase, redGun, snowBase, snowGun, urbanBase, urbanGun;

        private KeyboardState oldState;

        public int selectedButton { get; private set; }
        public int selectedEnemy { get; private set; }
        private bool selectedEnemyUnlocked;

        private List<Texture2D> bases, guns;

        private GameObject upArrow, downArrow;

        public TankPartType selectedTankPart
        {
            get
            {
                if (bases[selectedEnemy] == desertBase)
                    return TankPartType.desert;
                else if (bases[selectedEnemy] == jungleBase)
                    return TankPartType.jungle;
                else if (bases[selectedEnemy] == redBase)
                    return TankPartType.red;
                else if (bases[selectedEnemy] == snowBase)
                    return TankPartType.snow;
                else if (bases[selectedEnemy] == urbanBase)
                    return TankPartType.urban;
                else return TankPartType.basic;
            }
        }

        /// <summary>
        /// Checks if the game to be played will unlock new content
        /// </summary>
        public bool unlockContent
        {
            get
            {
                if (bases[selectedEnemy] == desertBase)
                {
                    if (Game1.levelsUnlocked > 1)
                        return false;
                    else return true;
                }
                else if (bases[selectedEnemy] == snowBase)
                {
                    if (Game1.levelsUnlocked > 2)
                        return false;
                    else return true;
                }
                else if (bases[selectedEnemy] == urbanBase)
                {
                    if (Game1.levelsUnlocked > 3)
                        return false;
                    else return true;
                }
                else if (bases[selectedEnemy] == redBase)
                {
                    if (Game1.levelsUnlocked > 4)
                        return false;
                    else return true;
                }
                else if (bases[selectedEnemy] == jungleBase)
                {
                    if (Game1.levelsUnlocked > 5)
                        return false;
                    else return true;
                }
                else // basic
                {
                    if (Game1.levelsUnlocked > 0)
                        return false;
                    else return true;
                }
            }
        }

        public CareerEnemySelectionScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            selectedButton = 1;

            bases = new List<Texture2D>();
            guns = new List<Texture2D>();

            logo = content.Load<Texture2D>("EnemyTankSelectionLogo");
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

            //Define tank textures
            basicBase = content.Load<Texture2D>("BigTankParts//BasicBase");
            basicGun = content.Load<Texture2D>("BigTankParts//BasicGun");
            desertBase = content.Load<Texture2D>("BigTankParts//DesertBase");
            desertGun = content.Load<Texture2D>("BigTankParts//DesertGun");
            jungleBase = content.Load<Texture2D>("BigTankParts//JungleBase");
            jungleGun = content.Load<Texture2D>("BigTankParts//JungleGun");
            redBase = content.Load<Texture2D>("BigTankParts//RedBase");
            redGun = content.Load<Texture2D>("BigTankParts//RedGun");
            snowBase = content.Load<Texture2D>("BigTankParts//SnowBase");
            snowGun = content.Load<Texture2D>("BigTankParts//SnowGun");
            urbanBase = content.Load<Texture2D>("BigTankParts//UrbanBase");
            urbanGun = content.Load<Texture2D>("BigTankParts//UrbanGun");

            //Add textures to Lists
            bases.Add(basicBase);
            guns.Add(basicGun);
            bases.Add(desertBase);
            guns.Add(desertGun);
            bases.Add(snowBase);
            guns.Add(snowGun);
            bases.Add(urbanBase);
            guns.Add(urbanGun);
            bases.Add(redBase);
            guns.Add(redGun);
            bases.Add(jungleBase);
            guns.Add(jungleGun);

            downArrow = new GameObject(content.Load<Texture2D>("DownArrow"));
            upArrow = new GameObject(content.Load<Texture2D>("UpArrow"));

            downArrow.position = new Vector2(Game1.WindowWidth / 2, (Game1.WindowHeight / 2 - 30) + 110);
            upArrow.position = new Vector2(Game1.WindowWidth / 2, (Game1.WindowHeight / 2 - 30) - 110);
        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
            {
                Game1.selectFX.Play();

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
                Game1.selectFX.Play();

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
                    Game1.selectFX.Play();
                    selectedEnemy++;
                    selectedEnemy %= bases.Count;
                }
            }
            else upArrow.scale = 1;

            if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                downArrow.scale = 1.2f;

                if (selectedButton == 1)
                {
                    Game1.selectFX.Play();
                    selectedEnemy--;
                    if (selectedEnemy < 0)
                        selectedEnemy = bases.Count - 1;
                }
            }
            else downArrow.scale = 1;

            //Check if enemy is unlocked
            if (Game1.levelsUnlocked >= selectedEnemy)
                selectedEnemyUnlocked = true;
            else selectedEnemyUnlocked = false;


            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                if (selectedButton == 2) //Confirm Button
                {
                    if (selectedEnemyUnlocked)
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

            //Draw enemy tank
            spritebatch.Draw(bases[selectedEnemy], new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 bases[selectedEnemy].Width, bases[selectedEnemy].Height), Color.White, 0, new Vector2(bases[selectedEnemy].Width / 2, bases[selectedEnemy].Height / 2),
                 1, SpriteEffects.None, 1f);
            spritebatch.Draw(guns[selectedEnemy], new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 guns[selectedEnemy].Width, guns[selectedEnemy].Height), Color.White, 0, new Vector2(guns[selectedEnemy].Width / 2, guns[selectedEnemy].Height / 2),
                 1, SpriteEffects.None, 1f);

            if (!selectedEnemyUnlocked)
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
