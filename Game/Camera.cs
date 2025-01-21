using KaneUI7;
using KaneUI7.Foundation;
using Raylib_cs;
using System.Numerics;

namespace ParticleLife.Game
{
    /// <summary>
    /// 2D Camera to pan zoom and rotate a transform
    /// Transforms the worldSpace coordinates to the cameras local screenSpace
    /// </summary>
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;
        public Constraints DragBoundry = new Constraints(0);
        public float Rotation = 0f;
        public float Zoom = 1f;
        public float MinZoom = 0.1f;
        public float MaxZoom = 50f;
        public int MaxX = 40000;
        public int MaxY = 40000;
        public int MinX = -40000;
        public int MinY = -40000;

        public int ViewportWidth
        {
            get
            {
                return Raylib.GetScreenWidth();
            }
        }
        public int ViewportHeight
        {
            get
            {
                return Raylib.GetScreenHeight();
            }
        }

        public Camera(Vector2 position, float rotation, float zoom)
        {
            Position = position;
            Rotation = rotation;
            Zoom = zoom;
        }

        public Camera()
        {
            Position = Vector2.Zero;
            Rotation = 0f;
            Zoom = 1f;
        }

        public Rectangle TransformRectangle(Rectangle rectangle)
        {
            return new Rectangle(
                WorldToScreen(rectangle.Position),
                Scale(rectangle.Width),
                Scale(rectangle.Height)
            );
        }

        public Rect ScaledRectFromRectangle(Rectangle rectangle)
        {
            Vector2 vec = WorldToScreen(rectangle.Position);

            return new Rect(
                (int)vec.X, (int)vec.Y, (int)Scale(rectangle.Width), (int)Scale(rectangle.Height)
            );
        }

        /// <summary>
        /// Default set of camera controls
        /// </summary>
        public void CameraControls(bool mouseControl = true)
        {
            if (Raylib.IsMouseButtonDown(0) && !Raylib.IsKeyDown(KeyboardKey.LeftShift) && !KaneUI.IsDraggingSlider && DragBoundry.IsIn(Raylib.GetMouseX(), Raylib.GetMouseY()))
            {
                //CameraPos += Raylib.GetMouseDelta() / zoomLevel;
                Position -= Raylib.GetMouseDelta() * (1f / Zoom);
                if (Position.X > MaxX)
                {
                    Position.X = MaxX;
                }

                if (Position.X < MinX)
                {
                    Position.X = MinX;
                }

                if (Position.Y > MaxY)
                {
                    Position.Y = MaxY;
                }

                if (Position.Y < MinY)
                {
                    Position.Y = MinY;
                }
            }

            float zoomDelta = (Raylib.GetMouseWheelMove() / 10f) * (MathF.Sqrt(Zoom));
            if (MathF.Abs(zoomDelta) > 0)
            {
                Zoom += zoomDelta;
            }
            if (Zoom < MinZoom)
            {
                Zoom = MinZoom;
            }
            if (Zoom > MaxZoom)
            {
                Zoom = MaxZoom;
            }
            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                Zoom += (Zoom / MaxZoom) * 100f * Time.DeltaTime;
            }
            if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                Zoom -= (Zoom / MaxZoom) * 100f * Time.DeltaTime;
            }
            //Raylib.DrawCircle((int)View.ConvertXToScreenSpace(1000f), (int)View.ConvertYToScreenSpace(1000f), 10f, Color.Green);
            //if (zoomLevel != 0f) View.ScaleFromPoint(1000f, 1000f, zoomLevel);
            //if (View.Scale < 0.1f) View.Scale = 0.1f;
        }

        /// <summary>
        /// Default set of camera controls
        /// </summary>
        public void ToolCameraControls(bool mouseControl = true)
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Middle) && !Raylib.IsKeyDown(KeyboardKey.LeftShift) && !KaneUI.IsDraggingSlider && DragBoundry.IsIn(Raylib.GetMouseX(), Raylib.GetMouseY()))
            {
                //CameraPos += Raylib.GetMouseDelta() / zoomLevel;
                Position -= Raylib.GetMouseDelta() * (1f / Zoom);
                if (Position.X > MaxX)
                {
                    Position.X = MaxX;
                }

                if (Position.X < MinX)
                {
                    Position.X = MinX;
                }

                if (Position.Y > MaxY)
                {
                    Position.Y = MaxY;
                }

                if (Position.Y < MinY)
                {
                    Position.Y = MinY;
                }
            }

            float zoomDelta = (Raylib.GetMouseWheelMove() / 10f) * (MathF.Sqrt(Zoom));
            if (MathF.Abs(zoomDelta) > 0)
            {
                Zoom += zoomDelta;
            }
            if (Zoom < MinZoom)
            {
                Zoom = MinZoom;
            }
            if (Zoom > MaxZoom)
            {
                Zoom = MaxZoom;
            }
            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                Zoom += 10f * Time.DeltaTime;
            }
            if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                Zoom -= 10f * Time.DeltaTime;
            }
            //Raylib.DrawCircle((int)View.ConvertXToScreenSpace(1000f), (int)View.ConvertYToScreenSpace(1000f), 10f, Color.Green);
            //if (zoomLevel != 0f) View.ScaleFromPoint(1000f, 1000f, zoomLevel);
            //if (View.Scale < 0.1f) View.Scale = 0.1f;
        }

        // Convert world space to screen space
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Matrix3x2 transformMatrix = Matrix3x2.CreateTranslation(-Position) *
                                         Matrix3x2.CreateRotation(-Rotation) *
                                         Matrix3x2.CreateScale(Zoom) *
                                         Matrix3x2.CreateTranslation(ViewportWidth / 2, ViewportHeight / 2);

            return Vector2.Transform(worldPosition, transformMatrix);
        }

        // Convert screen space to world space
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Matrix3x2 transformMatrix =
                Matrix3x2.CreateTranslation(-Position) *
                Matrix3x2.CreateRotation(-Rotation) *
                Matrix3x2.CreateScale(Zoom) *
                Matrix3x2.CreateTranslation(ViewportWidth / 2, ViewportHeight / 2);

            Matrix3x2 inverseTransformMatrix;
            Matrix3x2.Invert(transformMatrix, out inverseTransformMatrix);

            return Vector2.Transform(screenPosition, inverseTransformMatrix);
        }

        public float Scale(float scale)
        {
            return Zoom * scale;
        }
    }
}
