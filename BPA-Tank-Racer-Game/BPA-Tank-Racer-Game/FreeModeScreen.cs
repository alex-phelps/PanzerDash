using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BPA_Tank_Racer_Game
{
    public class FreeModeScreen : Screen
    {
        private Screen currentScreen;
        private PlayerTankSelectionScreen playerSelectionScreen;
        private EnemyTankSelectionScreen enemyTankSelectionScreen;

        private ContentManager content;

        public bool gameReady { get; private set; }
        public PlayerTank playerTank { get; private set; }
        public AITank enemyTank { get; private set; }
        public int level { get; private set; }
        public BulletHandler bulletHandler { get; private set; }

        public FreeModeScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            gameReady = false;
            this.content = content;

            playerSelectionScreen = new PlayerTankSelectionScreen(content, new EventHandler(PlayerSelectionScreenEvent));
            enemyTankSelectionScreen = new EnemyTankSelectionScreen(content, new EventHandler(EnemyTankSelectionScreenEvent));

            currentScreen = playerSelectionScreen;
        }

        public override void Update(GameTime gametime)
        {
            currentScreen.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            currentScreen.Draw(spritebatch);
        }

        private void PlayerSelectionScreenEvent(object sender, EventArgs e)
        {
            if (playerSelectionScreen.selectedButton == 0) //Back
                screenEvent.Invoke(this, new EventArgs());
            else if (playerSelectionScreen.selectedButton == 3) // Confirm
                currentScreen = enemyTankSelectionScreen;
        }

        private void EnemyTankSelectionScreenEvent(object sender, EventArgs e)
        {
            if (enemyTankSelectionScreen.selectedButton == 0) //Back
                currentScreen = playerSelectionScreen;
            else if (enemyTankSelectionScreen.selectedButton == 3) // Confirm
            {
                //Temp
                StartGame();
            }
        }

        private void StartGame()
        {
            level = 2; //Temp
            bulletHandler = new BulletHandler();
            playerTank = new PlayerTank(content, bulletHandler, playerSelectionScreen.selectedTankBase, playerSelectionScreen.selectedTankGun);
            enemyTank = new AITank(content, bulletHandler, enemyTankSelectionScreen.selectedTankBase, enemyTankSelectionScreen.selectedTankGun,
                new Vector2(playerTank.position.X + 100, playerTank.position.Y));

            gameReady = true;

            screenEvent.Invoke(this, new EventArgs());
        }
    }
}
