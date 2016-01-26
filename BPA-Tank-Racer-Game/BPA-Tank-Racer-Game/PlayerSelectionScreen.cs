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
    public class PlayerSelectionScreen : Screen
    {
        private Texture2D backButton, backButtonDefault, backButtonSelected;
        private Texture2D confirmButton, confirmButtonDefault, confirmButtonSelected;
        private Texture2D border, borderSelected;
        private Texture2D borderBase, borderGun;

        private Texture2D basicBase, basicGun, desertBase, desertGun, jungleBase, jungleGun,
            rainbowBase, rainbowGun, redBase, redGun, snowBase, snowGun, urbanBase, urbanGun;

        private KeyboardState oldState;

        public int selectedButton { get; private set; }
        private int selectedBase = 0;
        private int selectedGun = 0;

        private List<Texture2D> bases = new List<Texture2D>();
        private List<Texture2D> guns = new List<Texture2D>();

        private GameObject leftDownArrow, leftUpArrow, rightDownArrow, rightUpArrow;

        public PlayerSelectionScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            selectedButton = 1;

            backButtonDefault = content.Load<Texture2D>("Back");
            confirmButtonDefault = content.Load<Texture2D>("Confirm");

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

            bases.Add(basicBase);
            bases.Add(desertBase);
            bases.Add(jungleBase);
            bases.Add(redBase);
            bases.Add(snowBase);
            bases.Add(urbanBase);
            bases.Add(rainbowBase);
            guns.Add(basicGun);
            guns.Add(desertGun);
            guns.Add(jungleGun);
            guns.Add(redGun);
            guns.Add(snowGun);
            guns.Add(urbanGun);
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
                    selectedBase++;
                    selectedBase %= bases.Count;
                }
                else if (selectedButton == 2)
                {
                    selectedGun++;
                    selectedGun %= guns.Count;
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
                    selectedBase--;
                    if (selectedBase < 0)
                        selectedBase = bases.Count - 1;
                }
                else if (selectedButton == 2)
                {
                    selectedGun--;
                    if (selectedGun < 0)
                        selectedGun = guns.Count - 1;
                }
            }
            else
            {
                leftDownArrow.scale = 1;
                rightDownArrow.scale = 1;
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

            //Draw borders
            spritebatch.Draw(borderBase, new Vector2(Game1.WindowWidth / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 borderBase.Width, borderBase.Height), Color.White, 0, new Vector2(borderBase.Width / 2, borderBase.Height / 2),
                 1, SpriteEffects.None, 1f);
            spritebatch.Draw(borderGun, new Vector2(Game1.WindowWidth * 2 / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 borderGun.Width, borderGun.Height), Color.White, 0, new Vector2(borderGun.Width / 2, borderGun.Height / 2),
                 1, SpriteEffects.None, 1f);

            //Draw selected base and gun
            spritebatch.Draw(bases[selectedBase], new Vector2(Game1.WindowWidth / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 bases[selectedBase].Width, bases[selectedBase].Height), Color.White, 0, new Vector2(bases[selectedBase].Width / 2, bases[selectedBase].Height / 2),
                 1, SpriteEffects.None, 1f);
            spritebatch.Draw(guns[selectedGun], new Vector2(Game1.WindowWidth * 2 / 3, Game1.WindowHeight / 2 - 30), new Rectangle(0, 0,
                 guns[selectedGun].Width, guns[selectedGun].Height), Color.White, 0, new Vector2(guns[selectedGun].Width / 2, guns[selectedGun].Height / 2),
                 1, SpriteEffects.None, 1f);

            //Draw arrows
            leftDownArrow.Draw(spritebatch);
            leftUpArrow.Draw(spritebatch);
            rightDownArrow.Draw(spritebatch);
            rightUpArrow.Draw(spritebatch);

        }
    }
}
