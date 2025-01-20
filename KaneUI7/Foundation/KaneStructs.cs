using ParticleLife.Game;

namespace KaneUI7.Foundation
{
    /// <summary>
    /// All purpose Integer Rectangle for KaneUI
    /// </summary>
    public struct Rect
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;



        public Rect(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Rect(XY pos, int width, int height)
        {
            this.X = pos.X;
            this.Y = pos.Y;
            this.Width = width;
            this.Height = height;
        }

        public XY GetPosition()
        {
            return new XY(X, Y);
        }

        public static Rect Grow(Rect rect, int amount)
        {
            return new Rect(rect.X - amount, rect.Y - amount, rect.Width + amount * 2, rect.Height + amount * 2);
        }

        public static Rect FromCenter(int x, int y, int width, int height)
        {
            return new Rect(x - width / 2, y - height / 2, width, height);
        }

        public static Rect Zero => new Rect(0, 0, 0, 0);

        public static bool operator ==(Rect rect1, Rect rect2)
        {
            return rect1.Equals(rect2);
        }

        public static bool operator !=(Rect rect1, Rect rect2)
        {
            return !rect1.Equals(rect2);
        }

        public override readonly bool Equals(object obj)
        {
            if (!(obj is Rect))
            {
                return false;
            }

            Rect otherRect = (Rect)obj;
            return X == otherRect.X &&
                   Y == otherRect.Y &&
                   Width == otherRect.Width &&
                   Height == otherRect.Height;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }
    }

    /// <summary>
    /// Integer Vector 2 that represents X and Y coordinates
    /// </summary>
    public struct XY
    {
        public int X, Y;
        public XY(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static XY operator +(XY A, XY B)
        {
            return new XY(A.X + B.X, A.Y + B.Y);
        }

        public static bool operator ==(XY A, XY B)
        {
            return A.Equals(B);
        }

        public static bool operator !=(XY A, XY B)
        {
            return !A.Equals(B);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is XY))
            {
                return false;
            }
            XY xy = (XY)obj;
            return X == xy.X && Y == xy.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }

    /// <summary>
    /// 4 byte Color
    /// </summary>
    public struct RGBA
    {
        public byte R, G, B, A;

        public RGBA()
        {
            this.R = 0;
            this.G = 0;
            this.B = 0;
            this.A = 255;
        }

        public RGBA(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = 255;
        }

        public RGBA(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public RGBA(string hex)
        {
            this.R = 255;
            this.G = 255;
            this.B = 255;
            this.A = 255;

            if (hex.Length != 6 || hex.Length != 4)
            {
                for (int i = 0; i < hex.Length; i += 2)
                {
                    int high = HexCharToDecimal(hex[i]);
                    int low = HexCharToDecimal(hex[i + 1]);
                    if (i == 0)
                    {
                        this.R = (byte)((high << 4) | low);
                    }
                    else if (i == 2)
                    {
                        this.G = (byte)((high << 4) | low);
                    }
                    else if (i == 4)
                    {
                        this.B = (byte)((high << 4) | low);
                    }
                    else if (i == 6)
                    {
                        this.A = (byte)((high << 4) | low);
                    }
                }
            }
        }

        private static int HexCharToDecimal(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }

            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }

            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }

            throw new ArgumentException($"Invalid hex character: {c}");
        }
    }

    /// <summary>
    /// Define a Rectangle from the screen edges used to constrain moving panels (Just windows)
    /// </summary>
    public struct Constraints
    {
        public static readonly Constraints Zero = new Constraints();

        public int Left, Right, Top, Bottom;

        public Constraints()
        {
            Left = 0;
            Right = 0;
            Top = 0;
            Bottom = 0;
        }
        public Constraints(int allSides)
        {
            Left = allSides;
            Right = allSides;
            Top = allSides;
            Bottom = allSides;
        }
        /// <summary>
        /// left, right, top, bottom
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        public Constraints(int left, int right, int top, int bottom)
        {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }

        public bool IsIn(int x, int y)
        {
            return x >= Left && x <= Screen.Width - Right && y >= Top && y <= Screen.Height - Bottom;
        }

        /// <summary>
        /// Adjusts the given rectangle to ensure it is inside the constraints.
        /// </summary>
        /// <param name="rect">The rectangle to adjust.</param>
        /// <returns>A new rectangle that is inside the constraints.</returns>
        public Rect AdjustToFit(Rect rect)
        {
            int newX = rect.X;
            int newY = rect.Y;

            if (rect.X < Left)
            {
                newX = Left;
            }
            else if (rect.X + rect.Width > Screen.Width - Right)
            {
                newX = Screen.Width - Right - rect.Width;
            }

            if (rect.Y < Top)
            {
                newY = Top;
            }
            else if (rect.Y + rect.Height > Screen.Height - Bottom)
            {
                newY = Screen.Height - Bottom - rect.Height;
            }

            return new Rect(newX, newY, rect.Width, rect.Height);
        }
    }
}
