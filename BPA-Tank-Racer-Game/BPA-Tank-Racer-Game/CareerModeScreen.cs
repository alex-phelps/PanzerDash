using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BPA_Tank_Racer_Game
{
    public class CareerModeScreen : Screen
    {
        private Screen currentScreen;
        private PlayerTankSelectionScreen playerTankSelectionScreen;
        private CareerEnemySelectionScreen careerEnemySelectionScreen;

        private ContentManager content;

        public bool gameReady { get; private set; }
        public PlayerTank playerTank { get; private set; }
        public AITank enemyTank { get; private set; }
        public BulletHandler bulletHandler { get; private set; }
        public int level { get; private set; }
        public bool unlockContent { get; private set; }

        public CareerModeScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            this.content = content;

            playerTankSelectionScreen = new PlayerTankSelectionScreen(content,
                new EventHandler(PlayerTankSelectionScreenEvent), true);
            careerEnemySelectionScreen = new CareerEnemySelectionScreen(content,
                new EventHandler(CareerEnemySelectionScreenEvent));

            currentScreen = playerTankSelectionScreen;
        }

        public override void Update(GameTime gametime)
        {
            currentScreen.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            currentScreen.Draw(spritebatch);
        }

        private void PlayerTankSelectionScreenEvent(object sender, EventArgs e)
        {
            if (playerTankSelectionScreen.selectedButton == 0) //Back
                screenEvent.Invoke(this, new EventArgs());
            else if (playerTankSelectionScreen.selectedButton == 3) // Confirm
                currentScreen = careerEnemySelectionScreen;
        }
        private void CareerEnemySelectionScreenEvent(object sender, EventArgs e)
        {
            if (careerEnemySelectionScreen.selectedButton == 0) //Back
                currentScreen = playerTankSelectionScreen;
            else if (careerEnemySelectionScreen.selectedButton == 2) //Confirm
                StartGame();
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        private void StartGame()
        {
            level = careerEnemySelectionScreen.selectedEnemy + 1; // +1 because selectedEnemy by default starts at 0
            bulletHandler = new BulletHandler();
            playerTank = new PlayerTank(content, bulletHandler, playerTankSelectionScreen.selectedTankBase, playerTankSelectionScreen.selectedTankGun);
            enemyTank = new AITank(content, bulletHandler, careerEnemySelectionScreen.selectedTankPart, careerEnemySelectionScreen.selectedTankPart,
                new Vector2(playerTank.position.X + 100, playerTank.position.Y));

            unlockContent = careerEnemySelectionScreen.unlockContent;

            gameReady = true;

            screenEvent.Invoke(this, new EventArgs());
        }
    }
}
