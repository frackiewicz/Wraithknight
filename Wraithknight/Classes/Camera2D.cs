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

        public Vector2 DeltaPosition;
        public float Distance;

        public Matrix MatZoom;

        public Matrix Projection;

        //To be kind on the GC
        private Vector3 _vector3Helper;
        private Point _calculatedPoint;

        internal AABB CullRec = new AABB();

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
            UpdateVariables();
            
            MoveToTargetPos(gameTime);
            HandelZoom(gameTime);

            SetView();
        }

        public void SetView()
        {
            TranslateCenter.X = (int)Graphics.Viewport.Width / 2f;
            TranslateCenter.Y = (int)Graphics.Viewport.Height / 2f;
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

        private void UpdateVariables()
        {
            DeltaPosition = TargetPosition - CurrentPosition;
            Distance = DeltaPosition.Length();

            SetCullRectangle();
        }

        private void SetCullRectangle()
        {
            _calculatedPoint = ConvertScreenToWorld(0, 0);
            CullRec.X = _calculatedPoint.X;
            CullRec.Y = _calculatedPoint.Y;
            CullRec.Width = (int)((Graphics.Viewport.Width * 1 / CurrentZoom));
            CullRec.Height = (int)((Graphics.Viewport.Height * 1 / CurrentZoom));
        }

        private void MoveToTargetPos(GameTime gameTime) 
        {
            if (Distance < 0.001f) { CurrentPosition = TargetPosition; }
            else
            { CurrentPosition += DeltaPosition * CameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; }
        }

        private void HandelZoom(GameTime gameTime)
        {
            if (CurrentZoom != TargetZoom)
            {
                if (Math.Abs((CurrentZoom - TargetZoom)) < ZoomSpeed * CurrentZoom * (float)gameTime.ElapsedGameTime.TotalSeconds) { CurrentZoom = TargetZoom; }
                if (CurrentZoom > TargetZoom) { CurrentZoom -= ZoomSpeed * CurrentZoom * (float)gameTime.ElapsedGameTime.TotalSeconds; }
                if (CurrentZoom < TargetZoom) { CurrentZoom += ZoomSpeed * CurrentZoom * (float)gameTime.ElapsedGameTime.TotalSeconds; }

            }
        }
    }
}
