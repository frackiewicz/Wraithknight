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

        public bool FollowingHero = false;

        public Matrix MatRotation = Matrix.CreateRotationZ(0.0f);
        public Vector3 TranslateCenter;
        public Vector3 TranslateBody;
        public Matrix View;

        public Matrix MatZoom;

        public Matrix Projection;

        //To be kind on the GC
        private Vector3 _vector3Helper;
        private Point _calculatedPoint;

        internal AABB CullRec;

        public float CurrentZoom = 1.0f;
        public float TargetZoom = 1.0f;
        public float ZoomSpeed = 1f;

        public float CameraSpeed = 10f;
        public Vector2 CurrentPosition;
        public Vector2 TargetPosition;



        public Camera2D(GraphicsDevice graphics)
        {
            Graphics = graphics;
            View = Matrix.Identity;
            TranslateCenter.Z = 0;
            TranslateBody.Z = 0;
            CurrentPosition = Vector2.Zero; //initially the camera is at 0,0
            TargetPosition = Vector2.Zero;
            SetView();
        }

        public Point ConvertScreenToWorld(int x, int y)
        {   
            Projection = Matrix.CreateOrthographicOffCenter(0f, Graphics.Viewport.Width, Graphics.Viewport.Height, 0f, 0f, 1f);

            _vector3Helper.X = x;
            _vector3Helper.Y = y;
            _vector3Helper.Z = 0;

            _vector3Helper = Graphics.Viewport.Unproject(_vector3Helper, Projection, View, Matrix.Identity);

            _calculatedPoint.X = (int)_vector3Helper.X;
            _calculatedPoint.Y = (int)_vector3Helper.Y;
            return _calculatedPoint;
        }

        public Point ConvertScreenToWorld(Point point)
        {
            return ConvertScreenToWorld(point.X, point.Y);
        }
        public Point ConvertWorldToScreen(int x, int y)
        {   //converts world position to screen position
            Projection = Matrix.CreateOrthographicOffCenter(0f, Graphics.Viewport.Width, Graphics.Viewport.Height, 0f, 0f, 1f);
            _vector3Helper.X = x; _vector3Helper.Y = y; _vector3Helper.Z = 0;
            _vector3Helper = Graphics.Viewport.Project(_vector3Helper, Projection, View, Matrix.Identity);
            _calculatedPoint.X = (int)_vector3Helper.X; _calculatedPoint.Y = (int)_vector3Helper.Y; return _calculatedPoint;
        }

        public void Update(GameTime gameTime)
        {
            MoveToTargetPos(gameTime);
            ApplyZoom(gameTime);

            SetCullRectangle();
            SetView();
        }

        private void MoveToTargetPos(GameTime gameTime)
        {
            Vector2 positionDelta = TargetPosition - CurrentPosition;
            Vector2 positionChange = positionDelta * CameraSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (positionDelta.Length() < 0.01f) CurrentPosition = TargetPosition;
            else CurrentPosition += positionChange;
        }

        private void ApplyZoom(GameTime gameTime)
        {
            if (CurrentZoom != TargetZoom)
            {
                if (Math.Abs((CurrentZoom - TargetZoom)) < ZoomSpeed * CurrentZoom * (float)gameTime.ElapsedGameTime.TotalSeconds) { CurrentZoom = TargetZoom; }
                else if (CurrentZoom > TargetZoom) { CurrentZoom -= ZoomSpeed * CurrentZoom * (float)gameTime.ElapsedGameTime.TotalSeconds; }
                else if (CurrentZoom < TargetZoom) { CurrentZoom += ZoomSpeed * CurrentZoom * (float)gameTime.ElapsedGameTime.TotalSeconds; }
            }
        }
    
        private void SetCullRectangle()
        {
            _calculatedPoint = ConvertScreenToWorld(0, 0);
            int buffer = 16; //lazy
            CullRec.X = _calculatedPoint.X - buffer;
            CullRec.Y = _calculatedPoint.Y - buffer;
            CullRec.Width = Graphics.Viewport.Width * (1 / CurrentZoom) + 2 * buffer;
            CullRec.Height = Graphics.Viewport.Height * (1 / CurrentZoom) + 2 * buffer;
        }

        public void SetView()
        {
            TranslateCenter.X = Graphics.Viewport.Width / 2f;
            TranslateCenter.Y = Graphics.Viewport.Height / 2f;
            TranslateCenter.Z = 0;

            TranslateBody.X = -CurrentPosition.X;
            TranslateBody.Y = -CurrentPosition.Y;
            TranslateBody.Z = 0;

            MatZoom = Matrix.CreateScale(CurrentZoom, CurrentZoom, 1); //allows camera to properly zoom
            View = Matrix.CreateTranslation(TranslateBody) *
                   MatRotation *
                   MatZoom *
                   Matrix.CreateTranslation(TranslateCenter);
        }
    }
}
