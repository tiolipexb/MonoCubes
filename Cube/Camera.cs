﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Cube
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera : GameComponent
    {
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        public Vector3 cameraPosition { get; protected set; }
        Vector3 cameraDirection;
        Vector3 cameraUp;

        //defines speed of camera movement
        float speed = 0.5f;

        MouseState prevMouseState;

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Game.Window.ClientBounds.Width / (float)Game.Window.ClientBounds.Height, 1, 500);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            // Set mouse position and do initial get state
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);

            prevMouseState = Mouse.GetState();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            // Move forward and backward
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                cameraPosition += cameraDirection * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                cameraPosition -= cameraDirection * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                cameraPosition += Vector3.Cross(cameraUp, cameraDirection) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                cameraPosition -= Vector3.Cross(cameraUp, cameraDirection) * speed;

            Vector3 fixY = cameraPosition;
            fixY.Y = 8;
            cameraPosition = fixY;

            // Rotation in the world
            cameraDirection = Vector3.Transform(cameraDirection, Matrix.CreateFromAxisAngle(cameraUp, (-MathHelper.PiOver4 / 150) * (Mouse.GetState().X - prevMouseState.X)));
            cameraDirection = Vector3.Transform(cameraDirection, Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection), (MathHelper.PiOver4 / 100) * (Mouse.GetState().Y - prevMouseState.Y)));
            //cameraUp = Vector3.Transform(cameraUp, Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection), (MathHelper.PiOver4 / 100) * (Mouse.GetState().Y - prevMouseState.Y)));

            // Reset prevMouseState
            prevMouseState = Mouse.GetState();

            CreateLookAt();

            base.Update(gameTime);
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, cameraUp);
        }

    }
}