using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GameParts.GameCore.OtherSoftware;

namespace GameParts.GameCore.Players
{
    public abstract class PlayerBase
    {
        private Image image;

        public Coordinates Coords { get; set; }

        public Image Image 
        { 
            get => image; 
            set { if (value != null) { image = value; image.Tag = image.GetHashCode(); } else throw new ArgumentException("Value can't be null."); } 
        }

        public bool[,] Maze { get; set; }

        public delegate void GameFinishedEventHandler(PlayerBase player);

        public event GameFinishedEventHandler PlayerPositionChanged;

        void OnPlayerPositionChanged(PlayerBase player)
        {
            PlayerPositionChanged?.Invoke(player);
        }

        public PlayerBase()
        {
            Image = new Image();
            Coords = new Coordinates(0, 0);
            Maze = new bool[2, 2];
        }

        public PlayerBase(Image image, bool[,] maze)
        { Image = image; Coords = new Coordinates(0, 0); Maze = maze; }

        public PlayerBase(string imgPath, bool[,] maze)
        {
            Uri uri = new Uri(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + imgPath);
            Image = new Image();
            Image.Source = new BitmapImage(uri);
            Coords = new Coordinates(1, 1);
            Maze = maze;
        }

        public bool IfStepPossible(Coordinates position, Directions direction)
        {
            switch (direction)
            {
                case Directions.Up: return position.Row > 0 && Maze[position.Row - 1, position.Column];
                case Directions.Down: return position.Row < Maze.GetLength(0) - 1 && Maze[position.Row + 1, position.Column];
                case Directions.Left: return position.Column > 0 && Maze[position.Row, position.Column - 1];
                case Directions.Right: return position.Column < Maze.GetLength(1) - 1 && Maze[position.Row, position.Column + 1];
            }
            return false;
        }

        protected Coordinates StepDown(Coordinates coords) { coords.Row += 2; return coords; }

        protected Coordinates StepLeft(Coordinates coords) { coords.Column -= 2; return coords; }

        protected Coordinates StepRight(Coordinates coords) { coords.Column += 2; return coords; }

        protected Coordinates StepUp(Coordinates coords) { coords.Row -= 2; return coords; }

        public virtual void Step(StepArgs args)
        {
            if (args.Direction.IsEmpty) throw new ArgumentException("Input step args.direction field cannot be empty.");

            switch (args.Direction.Value)
            {
                case Directions.Up: Coords = StepUp(Coords); break;
                case Directions.Down: Coords = StepDown(Coords); break;
                case Directions.Left: Coords = StepLeft(Coords); break;
                case Directions.Right: Coords = StepRight(Coords); break;
            }
            OnPlayerPositionChanged(this);
        }

        public enum Directions { Up, Down, Left, Right }
    }
}
