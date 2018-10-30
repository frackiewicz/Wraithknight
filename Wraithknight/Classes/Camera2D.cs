using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Wraithknight{
    public class Camera2D
    {
        public static GraphicsDevice Graphics;

        public Matrix MatRotation = Matrix.CreateRotationZ(0.0f);
        public Matrix MatZoom;
        public Vector3 TranslateCenter;
        public Vector3 TranslateBody;
        public Matrix View;

        public Vector2 Delta;
        public float Distance;

        public float CurrentZoom = 1.0f;
        public float TargetZoom = 1.0f;
        public float ZoomSpeed = 0.05f;

        public float Speed = 5f; //how fast the camera moves
        public Vector2 CurrentPosition;
        public Vector2 CurrentPositionSnapped;
        public Vector2 TargetPosition;

        public Matrix Projection;
        Vector3 _; Point _t;

        public Camera2D(GraphicsDevice graphics)
        {
            Camera2D.Graphics = graphics;
            View = Matrix.Identity;
            TranslateCenter.Z = 0; //these two values dont change on a 2D camera
            TranslateBody.Z = 0;
            CurrentPosition = Vector2.Zero; //initially the camera is at 0,0
            TargetPosition = Vector2.Zero;
            SetView();
        }

        public Point ConvertScreenToWorld(int x, int y)
        {   //converts screen position to world position
            Projection = Matrix.CreateOrthographicOffCenter(0f, Graphics.Viewport.Width, Graphics.Viewport.Height, 0f, 0f, 1f);
            _.X = x; _.Y = y; _.Z = 0;
            _ = Graphics.Viewport.Unproject(_, Projection, View, Matrix.Identity);
            _t.X = (int)_.X; _t.Y = (int)_.Y; return _t;
        }

        public Point ConvertScreenToWorld(Point point)
        {
            return ConvertScreenToWorld(point.X, point.Y);
        }
        public Point ConvertWorldToScreen(int x, int y)
        {   //converts world position to screen position
            Projection = Matrix.CreateOrthographicOffCenter(0f, Graphics.Viewport.Width, Graphics.Viewport.Height, 0f, 0f, 1f);
            _.X = x; _.Y = y; _.Z = 0;
            _ = Graphics.Viewport.Project(_, Projection, View, Matrix.Identity);
            _t.X = (int)_.X; _t.Y = (int)_.Y; return _t;
        }

        public void Update(GameTime gameTime)
        {   //move the camera to the target position, match the target zoom
            Delta = TargetPosition - CurrentPosition;
            Distance = Delta.Length();
            //if camera is very close to target, then snap it to target
            if (Distance < 1) { CurrentPosition = TargetPosition; }
            else //camera is not close and should move according to speed
            { CurrentPosition += Delta * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds; }
            //round current position down to whole number - this prevents tearing/artifacting of sprites
            CurrentPositionSnapped.X = (int)CurrentPosition.X;
            CurrentPositionSnapped.Y = (int)CurrentPosition.Y;
            if (CurrentZoom != TargetZoom)
            {   //gradually match the zoom
                if (Math.Abs((CurrentZoom - TargetZoom)) < 0.05f) { CurrentZoom = TargetZoom; } //limit zoom
                if (CurrentZoom > TargetZoom) { CurrentZoom -= ZoomSpeed; } //zoom out
                if (CurrentZoom < TargetZoom) { CurrentZoom += ZoomSpeed; } //zoom in
                 
            }
            SetView();
        }

        public void SetView()
        {
            TranslateCenter.X = (int)Graphics.Viewport.Width / 2f;
            TranslateCenter.Y = (int)Graphics.Viewport.Height / 2f;
            TranslateCenter.Z = 0;

            TranslateBody.X = -CurrentPositionSnapped.X;
            TranslateBody.Y = -CurrentPositionSnapped.Y;
            TranslateBody.Z = 0;

            MatZoom = Matrix.CreateScale(CurrentZoom, CurrentZoom, 1); //allows camera to properly zoom
            View = Matrix.CreateTranslation(TranslateBody) *
                   MatRotation *
                   MatZoom *
                   Matrix.CreateTranslation(TranslateCenter);
        }
    }
}
