using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BPA_Tank_Racer_Game
{
    public class TankSelectionScreen : Screen
    {
        protected Texture2D backButton, backButtonDefault, backButtonSelected;
        protected Texture2D confirmButton, confirmButtonDefault, confirmButtonSelected;
        protected Texture2D border, borderSelected;
        protected Texture2D borderBase, borderGun;
        protected Texture2D lockScreen;

        protected Texture2D basicBase, basicGun, desertBase, desertGun, jungleBase, jungleGun,
            rainbowBase, rainbowGun, redBase, redGun, snowBase, snowGun, urbanBase, urbanGun;

        protected KeyboardState oldState;

        public int selectedButton { get; protected set; }
        protected int selectedBaseInt = 0;
        protected int selectedGunInt = 0;
        protected bool selectedBaseUnlocked;
        protected bool selectedGunUnlocked;

        protected List<Texture2D> bases = new List<Texture2D>();
        protected List<Texture2D> guns = new List<Texture2D>();

        protected GameObject leftDownArrow, leftUpArrow, rightDownArrow, rightUpArrow;

        public TankPartType selectedTankBase
        {
            get
            {
                if (bases[selectedBaseInt] == desertBase)
                    return TankPartType.desert;
                else if (bases[selectedBaseInt] == jungleBase)
                    return TankPartType.jungle;
                else if (bases[selectedBaseInt] == redBase)
                    return TankPartType.red;
                else if (bases[selectedBaseInt] == snowBase)
                    return TankPartType.snow;
                else if (bases[selectedBaseInt] == urbanBase)
                    return TankPartType.urban;
                else if (bases[selectedBaseInt] == rainbowBase)
                    return TankPartType.rainbow;
                else return TankPartType.basic;
            }
        }

        public TankPartType selectedTankGun
        {
            get
            {
                if (guns[selectedGunInt] == desertGun)
                    return TankPartType.desert;
                else if (guns[selectedGunInt] == jungleGun)
                    return TankPartType.jungle;
                else if (guns[selectedGunInt] == redGun)
                    return TankPartType.red;
                else if (guns[selectedGunInt] == snowGun)
                    return TankPartType.snow;
                else if (guns[selectedGunInt] == urbanGun)
                    return TankPartType.urban;
                else if (guns[selectedGunInt] == rainbowGun)
                    return TankPartType.rainbow;
                else return TankPartType.basic;
            }
        }

        public TankSelectionScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            selectedButton = 1;

            backButtonDefault = content.Load<Texture2D>("Back");
            confirmButtonDefault = content.Load<Texture2D>("Confirm");
            lockScreen = content.Load<Texture2D>("LockScreen");

            backButtonSelected = content.Load<Texture2D>("Back-Selected");
            confirmButtonSelected = content.Load<Texture2D>("Confirm-Selected");

            backButton = backButtonDefault;
            confirmButton = confirmButtonDefault;

            border = content.Load<Texture2D>("Border");
            borderSelected = content.Load<Texture2D>("Border-Selected");

            borderBase = borderSelected;
            borderGun = border;

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
            rainbowBase = content.Load<Texture2D>("BigTankParts//RainbowBase");
            rainbowGun = content.Load<Texture2D>("BigTankParts//RainbowGun");

            //Add textures to Lists
            bases.Add(basicBase);
            bases.Add(desertBase);
            bases.Add(snowBase);
            bases.Add(urbanBase);
            bases.Add(redBase);
            bases.Add(jungleBase);
            bases.Add(rainbowBase);
            guns.Add(basicGun);
            guns.Add(desertGun);
            guns.Add(snowGun);
            guns.Add(urbanGun);
            guns.Add(redGun);
            guns.Add(jungleGun);
            guns.Add(rainbowGun);

            Texture2D downArrow = content.Load<Texture2D>("DownArrow");
            Texture2D upArrow = content.Load<Texture2D>("UpArrow");

            leftDownArrow = new GameObject(downArrow);
            leftUpArrow = new GameObject(upArrow);
            rightDownArrow = new GameObject(downArrow);
            rightUpArrow = new GameObject(upArrow);

            leftDownArrow.position = new Vector2(Game1.WindowWidth / 3, (Game1.WindowHeight / 2 - 30) + 110);
            leftUpArrow.position = new Vector2(Game1.WindowWidth / 3, (Game1.WindowHeight / 2 - 30) - 110);
            rightDownArrow.position = new Vector2(Game1.WindowWidth * 2 / 3, (Game1.WindowHeight / 2 - 30) + 110);
            rightUpArrow.position = new Vector2(Game1.WindowWidth * 2 / 3, (Game1.WindowHeight / 2 - 30) - 110);
        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Escape))
            {
                selectedButton = 0;
                screenEvent.Invoke(this, new EventArgs());
            }

            if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
            {
                //Set old button to not selected icon
                if (selectedButton == 0)
                    backButton = backButtonDefault;
                else if (selectedButton == 1)
                    borderBase = border;
                else if (selectedButton == 2)
                    borderGun = border;
                else confirmButton = confirmButtonDefault;

                selectedButton--;
                if (selectedButton < 0)
                    selectedButton = 3;

                //Set new button to selected icon
                if (selectedButton == 0)
                    backButton = backButtonSelected;
                else if (selectedButton == 1)
                    borderBase = borderSelected;
                else if (selectedButton == 2)
                    borderGun = borderSelected;
                else confirmButton = confirmButtonSelected;
            }

            if (newState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
            {
                //Set old button to not selected icon
                if (selectedButton == 0)
                    backButton = backButtonDefault;
                else if (selectedButton == 1)
                    borderBase = border;
                else if (selectedButton == 2)
                    borderGun = border;
                else confirmButton = confirmButtonDefault;

                selectedButton++;
                selectedButton %= 4;

                //Set new button to selected icon
                if (selectedButton == 0)
                    backButton = backButtonSelected;
                else if (selectedButton == 1)
                    borderBase = borderSelected;
                else if (selectedButton == 2)
                    borderGun = borderSelected;
                else confirmButton = confirmButtonSelected;
            }

            if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                leftUpArrow.scale = 1.2f;
                rightUpArrow.scale = 1.2f;

                if (selectedButton == 1)
                {
                    selectedBaseInt++;
                    selectedBaseInt %= bases.Count;
                }
                else if (selectedButton == 2)
                {
                    selectedGunInt++;
                    selectedGunInt %= guns.Count;
                }
            }
            else
            {
                leftUpArrow.scale = 1;
                rightUpArrow.scale = 1;
            }

            if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                leftDownArrow.scale = 1.2f;
                rightDownArrow.scale = 1.2f;

                if (selectedButton == 1)
                {
                    selectedBaseInt--;
                    if (selectedBaseInt < 0)
                        selectedBaseInt = bases.Count - 1;
                }
                else if (selectedButton == 2)
                {
                    selectedGunInt--;
                    if (selectedGunInt < 0)
                        selectedGunInt = guns.Count - 1;
                }
            }
            else
            {
                leftDownArrow.scale = 1;
                rightDownArrow.scale = 1;
            }

            //Check if base is unlocked
            if (Game1.levelsUnlocked > selectedBaseInt)
                selectedBaseUnlocked = true;
            else if (bases[selectedBaseInt] == rainbowBase && Game1.hasRainbowBase)
                selectedBaseUnlocked = true;
            else selectedBaseUnlocked = false;

            //check if gun is unlocked
            if (Game1.levelsUnlocked > selectedGunInt)
                selectedGunUnlocked = true;
            else if (guns[selectedGunInt] == rainbowGun && Game1.hasRainbowGun)
                selectedGunUnlocked = true;
            else selectedGunUnlocked = false;

            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                if (selectedButton == 3) //Confirm
                {
                    if (selectedBaseUnlocked && selectedGunUnlocked)
                        screenEvent.Invoke(this, new EventArgs());
                }
                else screenEvent.Invoke(this, new EventArgs());
            }


            if (selectedButton == 1)
            {
                leftDownArrow.visible = true;
                leftUpArrow.visible = true;
                rightDownArrow.visible = false;
                rightUpArrow.visible = false;
            }
            else if (selectedButton == 2)
            {
                leftDownArrow.visible = false;
                leftUpArrow.visible = false;
                rightDownArrow.visible = true;
                rightUpArrow.visible = true;
            }
            else
            {
                leftDownArrow.visible = false;
                leftUpArrow.visible = false;
                rightDownArrow.visible = false;
                rightUpArrow.visible = false;
            }

            oldState = newState;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //Draw text buttons
            spritebatch.Draw(backButton, new Vector2(120, Game1.WindowHeight - 40), new Rectangle(0, 0,
                backButton.Width, backButton.Height), Color.White, 0, new Vector2(backButton.Width / 2, backButton.Height / 2),
                1, SpriteEffects.None, 1f);
            spritebatch.Draw(confirmButton, new Vector2(Game1.WindowWidth - 140, Game1.WindowHeight - 40), new Rectangle(0, 0,
                 confirmButton.Width, confirmButton.Height), Color.White, 0, new Vector2(confirmButton.Width / 2, confirmButton.Height / 2),
                 1, SpriteEffects.None, 1f);

            //Draw selected base and gun
            spritebatch.Draw(bases[selectedBaseInt], new Vector2(Game1.WindowWidth / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 bases[selectedBaseInt].Width, bases[selectedBaseInt].Height), Color.White, 0, new Vector2(bases[selectedBaseInt].Width / 2, bases[selectedBaseInt].Height / 2),
                 1, SpriteEffects.None, 1f);
            spritebatch.Draw(guns[selectedGunInt], new Vector2(Game1.WindowWidth * 2 / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 guns[selectedGunInt].Width, guns[selectedGunInt].Height), Color.White, 0, new Vector2(guns[selectedGunInt].Width / 2, guns[selectedGunInt].Height / 2),
                 1, SpriteEffects.None, 1f);

            //Draw lock screen(s)
            if (!selectedBaseUnlocked)
            {
                spritebatch.Draw(lockScreen, new Vector2(Game1.WindowWidth / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                     lockScreen.Width, lockScreen.Height), Color.White, 0, new Vector2(lockScreen.Width / 2, lockScreen.Height / 2),
                     1, SpriteEffects.None, 1f);
            }
            if (!selectedGunUnlocked)
            {
                spritebatch.Draw(lockScreen, new Vector2(Game1.WindowWidth * 2 / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                     lockScreen.Width, lockScreen.Height), Color.White, 0, new Vector2(lockScreen.Width / 2, lockScreen.Height / 2),
                     1, SpriteEffects.None, 1f);
            }

            //Draw borders
            spritebatch.Draw(borderBase, new Vector2(Game1.WindowWidth / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 borderBase.Width, borderBase.Height), Color.White, 0, new Vector2(borderBase.Width / 2, borderBase.Height / 2),
                 1, SpriteEffects.None, 1f);
            spritebatch.Draw(borderGun, new Vector2(Game1.WindowWidth * 2 / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 borderGun.Width, borderGun.Height), Color.White, 0, new Vector2(borderGun.Width / 2, borderGun.Height / 2),
                 1, SpriteEffects.None, 1f);

            //Draw arrows
            leftDownArrow.Draw(spritebatch);
            leftUpArrow.Draw(spritebatch);
            rightDownArrow.Draw(spritebatch);
            rightUpArrow.Draw(spritebatch);

        }
    }
}
