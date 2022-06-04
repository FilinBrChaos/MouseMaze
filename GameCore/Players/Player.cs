using System;
using System.Windows.Controls;
using System.Windows.Input;
using GameParts.GameCore.OtherSoftware;

namespace GameParts.GameCore.Players
{
    public class Player : PlayerBase
    {
        public Player() : base() { }

        public Player(Image image, bool[,] maze) : base(image, maze) { }

        public Player(string imgPath, bool[,] maze) : base(imgPath, maze) { }

        public override void Step(StepArgs args)
        {
            StepArgs arguments = (StepArgs)args.Clone();
            if (arguments.Key.IsEmpty) throw new ArgumentException("Input step args.Key field cannot be empty.");
            Key key = arguments.Key.Value;
            if (key == Key.Down || key == Key.S) arguments.Direction.Value = Directions.Down;
            else if (key == Key.Up || key == Key.W) arguments.Direction.Value = Directions.Up;
            else if (key == Key.Right || key == Key.D) arguments.Direction.Value = Directions.Right;
            else if (key == Key.Left || key == Key.A) arguments.Direction.Value = Directions.Left;

            if (!arguments.Direction.IsEmpty) if (IfStepPossible(Coords, arguments.Direction.Value))  base.Step(arguments);
        }
    }
}
