using System;
using System.Collections.Generic;
using CocosSharp;

namespace BouncingGame
{
    public class GameLayer : CCLayerColor
    {
        CCSprite paddle;
        CCSprite ball;
        CCLabel mainScore;
        CCLabel ultimateScore;
        CCLabel roundsPlayed;


        float ballXSpeed;
        float ballYSpeed;

        // How much to modify the ball's y velocity per second:
        const float gravity = 140;

        int score;
        int highScore;
        Boolean gamePlaying = true;






        public GameLayer() : base(CCColor4B.Gray)
        {

            // "paddle" refers to the paddle.png image
            paddle = new CCSprite("paddle");
            paddle.PositionX = 100;
            paddle.PositionY = 100;
            AddChild(paddle);

            ball = new CCSprite("ball");
            ball.PositionX = 320;
            ball.PositionY = 600;
            AddChild(ball);



            mainScore = new CCLabel("Score: 0", "Arial", 70, CCLabelFormat.SystemFont);
            mainScore.PositionX = 50;
            mainScore.PositionY = 1000;
            mainScore.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(mainScore);


            ultimateScore = new CCLabel("HighScore: 0", "Arial", 70, CCLabelFormat.SystemFont);
            ultimateScore.PositionX = 50;
            ultimateScore.PositionY = 900;
            ultimateScore.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(ultimateScore);

            roundsPlayed = new CCLabel("rounds played: 0", "Arial", 70, CCLabelFormat.SystemFont);
            roundsPlayed.PositionX = 50;
            roundsPlayed.PositionY = 800;
            roundsPlayed.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(roundsPlayed);

            Schedule(RunGameLogic);

        }

        void RunGameLogic(float frameTimeInSeconds)
        {
            // This is a linear approximation, so not 100% accurate
            ballYSpeed += frameTimeInSeconds * -gravity;
            ball.PositionX += ballXSpeed * frameTimeInSeconds;
            ball.PositionY += ballYSpeed * frameTimeInSeconds;

            float screenTop = VisibleBoundsWorldspace.MaxY;
            float screenBottom = VisibleBoundsWorldspace.MinY;

            // New Code:
            // Check if the two CCSprites overlap...
            bool doesBallOverlapPaddle = ball.BoundingBoxTransformedToParent.IntersectsRect(
                paddle.BoundingBoxTransformedToParent);
            // ... and if the ball is moving downward.
            bool isMovingDownward = ballYSpeed < 0;
            if (doesBallOverlapPaddle && isMovingDownward)
            {
                // First let's invert the velocity:
                ballYSpeed *= -1;

                const float minXVelocity = -30;
                const float maxXVelocity = 900;
                ballXSpeed = CCRandom.GetRandomFloat(minXVelocity, maxXVelocity);




                // New code:
                score++;
                mainScore.Text = "Score: " + score;
                if (score > highScore)
                {
                    highScore = score;
                    setHighScore(highScore);

                }









            }
            // First let’s get the ball position:   
            float ballRight = ball.BoundingBoxTransformedToParent.MaxX;
            float ballLeft = ball.BoundingBoxTransformedToParent.MinX;

            float ballBottom = ball.BoundingBoxTransformedToParent.MinY;
            // Then let’s get the screen edges
            float screenRight = VisibleBoundsWorldspace.MaxX;
            float screenLeft = VisibleBoundsWorldspace.MinX;




            // Check if the ball is either too far to the right or left:    
            bool shouldReflectXVelocity =
                (ballRight > screenRight && ballXSpeed > 0) ||
                (ballLeft < screenLeft && ballXSpeed < 0);

            bool ballOfBottomScreen = (ballBottom < screenBottom && ballXSpeed > 0);


            if (shouldReflectXVelocity)
            {
                ballXSpeed *= -1;
            }

            if (ballOfBottomScreen)
            {

            }






        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            CCRect bounds = VisibleBoundsWorldspace;

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            touchListener.OnTouchesMoved = HandleTouchesMoved;
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }

        void HandleTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            // we only care about the first touch:
            var locationOnScreen = touches[0].Location;
            paddle.PositionX = locationOnScreen.X;
        }

        void setHighScore(int HighScore)
        {
            CCUserDefault.SharedUserDefault.SetIntegerForKey("highscore", highScore);
            CCUserDefault.SharedUserDefault.Flush();

            highScore = CCUserDefault.SharedUserDefault.GetIntegerForKey("highscore");


            ultimateScore.Text = "HighScore: " + highScore;

        }

        void restartGame()
        {

        }









    }
}

