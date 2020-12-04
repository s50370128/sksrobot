using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Building
{
    public class Camera
    {
        #region MEMBER

        float _theta;

        public float HorizontalAngle
        {
            get { return _theta; }
            set { _theta = value; }
        }

        float _phi;

        public float VerticalAngle
        {
            get { return _phi; }
            set { _phi = value; }
        }

        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        float _orthWidth;

        public float OrthWidth
        {
            get { return _orthWidth; }
            set { _orthWidth = value; }
        }

        float _orthHeight;

        public float OrthHeight
        {
            get { return _orthHeight; }
            set { _orthHeight = value; }
        }

        float _nearPlane;

        public float NearPlane
        {
            get { return _nearPlane; }
            set { _nearPlane = value; }
        }

        float _farPlane;

        public float FarPlane
        {
            get { return _farPlane; }
            set { _farPlane = value; }
        }

        bool _ifPerspective;

        public bool IfPerspective
        {
            get { return _ifPerspective; }
            set { _ifPerspective = value; }
        }

        Vector3 _eyePosition;

        public Vector3 Position
        {
            get { return _eyePosition; }
            set { _eyePosition = value; }
        }

        Vector3 _targetPosition;

        public Vector3 Target
        {
            get { return _targetPosition; }
            set { _targetPosition = value; }
        }

        Vector3 _direction;

        public Vector3 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        Quaternion _rotation;

        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        float _fov;

        public float Fov
        {
            get { return _fov; }
            set { _fov = value; }
        }

        float _aspectRatio;

        public float Aspecture
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; }
        }

        Viewport _viewport;

        public Viewport Viewport
        {
            get { return _viewport; }
            set { _viewport = value; }
        }

        Matrix _view;

        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
        }

        Matrix _projection;

        public Matrix Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }


        int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        int _height;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        #endregion

        public Camera(string name, int width, int height, float aspectRatio, float fov, float near, float far, bool perspective,Viewport viewport)
        {
            _nearPlane = near;
            _farPlane = far;
            _ifPerspective = perspective;
            _fov = fov;
            _width = width;
            _height = height;
            _aspectRatio = aspectRatio;
            _name = name;
            Viewport = viewport;
        }
   
        public void Initialize(Vector3 eyePosition, Vector3 targetPosition)
        {
            _eyePosition = eyePosition;
            _targetPosition = targetPosition;
            _direction = _targetPosition - _eyePosition;
            _direction.Normalize();
            _phi = MathHelper.ToDegrees((float)Math.Asin(_direction.Y / _direction.Length()));
            if (_direction.X > 0)
            {
                _theta = MathHelper.ToDegrees((float)Math.Atan(_direction.Z/_direction.X));
            }
            else
            {
                _theta = MathHelper.ToDegrees((float)Math.Atan(_direction.Z / _direction.X)) - 180;
            }
            UpdateCamera();
        }

        public void UpdateCamera()
        {
            
            _view = Matrix.CreateLookAt(_eyePosition, _targetPosition, Vector3.Up);
            if (_ifPerspective)
            {
                _projection = Matrix.CreatePerspectiveFieldOfView(_fov, _aspectRatio, _nearPlane, _farPlane);
            }
            else
            {
                _projection = Matrix.CreateOrthographic(_orthWidth, _orthHeight, _nearPlane, _farPlane);
            }
        }

        public void UpdateCamera(float Hangle, float Vangle)
        {
            float r = (float)Math.Cos(MathHelper.ToRadians(Vangle));
            Hangle = MathHelper.ToRadians(Hangle);
            Vangle = MathHelper.ToRadians(Vangle);
            _direction = new Vector3(r * (float)Math.Cos(Hangle), (float)Math.Sin(Vangle), r * (float)Math.Sin(Hangle));
            _targetPosition = _eyePosition + _direction;
            _direction.Normalize();
            UpdateCamera();
        }

        public void UpdateCamera(Vector3 EyePosition, Vector3 TargetPosition, Vector3 Updirection)
        {

            _view = Matrix.CreateLookAt(EyePosition, TargetPosition, Updirection);
            if (_ifPerspective)
            {
                _projection = Matrix.CreatePerspectiveFieldOfView(_fov, _aspectRatio, _nearPlane, _farPlane);
            }
            else
            {
                _projection = Matrix.CreateOrthographic(_orthWidth, _orthHeight, _nearPlane, _farPlane);
            }
        }

        public void MoveForward(float distance)
        {
            _eyePosition += _direction * distance;
            _targetPosition += _direction * distance;
            UpdateCamera();
        }

        public void MoveHorizontal(float distance)
        {
            Vector3 pan = Vector3.Cross(Vector3.UnitY, _direction);
            _eyePosition += pan * distance;
            _targetPosition += pan * distance;
            UpdateCamera();
        }

        public void MoveForwardRight(float distance)
        {
            Vector3 pan = -Vector3.Cross(Vector3.UnitY, _direction) + _direction;
            pan.Normalize();
            _eyePosition += pan * distance;
            _targetPosition += pan * distance;
            UpdateCamera();
        }

        public void MoveForwardLeft(float distance)
        {
            Vector3 pan = Vector3.Cross(Vector3.UnitY, _direction) + _direction;
            pan.Normalize();
            _eyePosition += pan * distance;
            _targetPosition += pan * distance;
            UpdateCamera();
        }

        public void Turning(float degree)
        {
            _theta += degree;          
            UpdateCamera(_theta, _phi);
        }

        public void RotateVertical(float degree)
        {
            _phi += degree;
            if (_phi > 89)
            {
                _phi = 89;
            }
            else if (_phi < -89)
            {
                _phi = -89;
            }
            UpdateCamera(_theta, _phi);
        }

        public void MoveUpDown(float distance)
        {
            _eyePosition.Y += distance;
            _targetPosition.Y += distance;
            UpdateCamera();
        }

        public void Zoom(float z)
        {
            float distance = z;
            _eyePosition -= _direction * distance;
            _targetPosition -= _direction * distance;
            UpdateCamera();
        }
    }
}