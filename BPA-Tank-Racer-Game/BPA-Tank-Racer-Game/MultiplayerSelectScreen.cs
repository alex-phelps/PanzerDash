using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PanzerDash
{
    public class MultiplayerSelectScreen : Screen
    {
        private Screen currentScreen;
        private PlayerTankSelectionScreen player1SelectionScreen;
        private PlayerTankSelectionScreen player2SelectionScreen;
        private LevelSelectionScreen levelSelectionScreen;

        private ContentManager content;

        public bool gameReady { get; private set; }
        public PlayerTank player1 { get; private set; }
        public PlayerTank player2 { get; private set; }
        public BulletHandler bulletHandler { get; private set; }
        public int level { get; private set; }

        public MultiplayerSelectScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            gameReady = false;
            this.content = content;

            player1SelectionScreen = new PlayerTankSelectionScreen(content, new EventHandler(Player1SelectionScreenEvent), false)
            {
                logo = content.Load<Texture2D>("Player1")
            };
            player2SelectionScreen = new PlayerTankSelectionScreen(content, new EventHandler(Player2SelectionScreenEvent), false) 
            {
                logo = content.Load<Texture2D>("Player2")
            };
            levelSelectionScreen = new LevelSelectionScreen(content, new EventHandler(LevelSelectionScreenEvent));

            currentScreen = player1SelectionScreen;
        }

        public override void Update(GameTime gametime)
        {
            currentScreen.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            currentScreen.Draw(spritebatch);
        }

        private void Player1SelectionScreenEvent(object sender, EventArgs e)
        {
            if (player1SelectionScreen.selectedButton == 0) //Back
                screenEvent.Invoke(this, new EventArgs());
            else if (player1SelectionScreen.selectedButton == 3) // Confirm
                currentScreen = player2SelectionScreen;
        }

        private void Player2SelectionScreenEvent(object sender, EventArgs e)
        {
            if (player2SelectionScreen.selectedButton == 0) //Back
                screenEvent.Invoke(this, new EventArgs());
            else if (player2SelectionScreen.selectedButton == 3) // Confirm
                currentScreen = levelSelectionScreen;
        }

        private void LevelSelectionScreenEvent(object sender, EventArgs e)
        {
            if (levelSelectionScreen.selectedButton == 0) //Back
                currentScreen = player2SelectionScreen;
            else if (levelSelectionScreen.selectedButton == 2) //Confirm
                StartGame();
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        private void StartGame()
        {
            level = levelSelectionScreen.selectedLevel + 1; // +1 because selectedLevel by default starts at 0
            bulletHandler = new BulletHandler();
            player1 = new PlayerTank(content, bulletHandler, player1SelectionScreen.selectedTankBase, player1SelectionScreen.selectedTankGun,
                Keys.W, Keys.S, Keys.A, Keys.D, Keys.F, Keys.G, Keys.C);
            player1.position = new Vector2(Game1.WindowWidth / 4, Game1.WindowHeight / 2);
            player2 = new PlayerTank(content, bulletHandler, player2SelectionScreen.selectedTankBase, player2SelectionScreen.selectedTankGun,
                Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.OemComma, Keys.OemPeriod, Keys.RightControl);
            player2.position = new Vector2(player1.position.X + Game1.WindowWidth / 2, player1.position.Y);

            gameReady = true;

            screenEvent.Invoke(this, new EventArgs());
        }
    }
}
