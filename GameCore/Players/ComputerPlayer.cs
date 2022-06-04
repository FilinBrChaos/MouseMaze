using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GameParts.GameCore.OtherSoftware;

namespace GameParts.GameCore.Players
{
    public class ComputerPlayer : PlayerBase
    {
        protected Directions previousStep;
        protected double difficulty;
        protected int averageSpeed;
        protected Random random;

        public List<Directions> Path { get; set; }

        public double Difficulty
        {
            get => difficulty;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentException("Difficulty must be between 0 and 1");
                else difficulty = value;
            }
        }

        public virtual int AverageSpeed
        {
            get => averageSpeed;
            set { if (value > 0) averageSpeed = value; else throw new ArgumentException("Average speed must be larger than zero"); }
        }

        public ComputerPlayer() : base() 
        {
            Path = new List<Directions>();
            Difficulty = 0.5;
            AverageSpeed = 400;
        }

        public ComputerPlayer(Image image, bool[,] maze) : base(image, maze) 
        {
            Coords = new Coordinates(1, 1);
            Path = new List<Directions>();
            Difficulty = 0.5;
            AverageSpeed = 300;
            random = new Random(DateTime.Now.Millisecond);
        }

        public ComputerPlayer(string imgPath, bool[,] maze) : base(imgPath, maze) 
        {
            Coords = new Coordinates(1, 1);
            Path = new List<Directions>();
            Difficulty = 0.5;
            AverageSpeed = 300;
            random = new Random(DateTime.Now.Millisecond);
        }

        protected List<Directions> PossibleSteps()
        {
            List<Directions> result = new List<Directions>();
            if (IfStepPossible(Coords, Directions.Up)) result.Add(Directions.Up);
            if (IfStepPossible(Coords, Directions.Down)) result.Add(Directions.Down);
            if (IfStepPossible(Coords, Directions.Left)) result.Add(Directions.Left);
            if (IfStepPossible(Coords, Directions.Right)) result.Add(Directions.Right);
            return result;
        }

        protected Directions GetOppositDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up: return Directions.Down;
                case Directions.Down: return Directions.Up;
                case Directions.Left: return Directions.Right;
                case Directions.Right: return Directions.Left;
            }
            return Directions.Down;
        }

        protected Directions FindDirection(GraphCell startCell, GraphCell directionCell)
        {
            int dirRow = directionCell.Coords.Row, startRow = startCell.Coords.Row;
            int dirCol = directionCell.Coords.Column, startCol = startCell.Coords.Column;
            if (dirRow < startRow) return Directions.Up;
            if (dirRow > startRow) return Directions.Down;
            if (dirCol < startCol) return Directions.Left;
            if (dirCol > startCol) return Directions.Right;
            return Directions.Down;
        }

        public virtual List<Directions> FindPath(PlayerBase finish)
        {
            return new List<Directions>();
        }

        //item created for BFS
        public class GraphCell : ICloneable
        {
            public List<GraphCell> ParentCells { get; set; }

            public List<GraphCell> ChildCells { get; set; }

            public int Distance { get; set; }

            public Coordinates Coords { get; set; }

            public GraphCell(int distance, Coordinates coords) { Distance = distance; Coords = coords; ParentCells = new List<GraphCell>(); ChildCells = new List<GraphCell>(); }

            public void AddChildCells(GraphCell childCell)
            {
                childCell.ParentCells.Add(this);
                ChildCells.Add(childCell);
            }

            public bool IsEqualToParentCells(GraphCell graphCell)
            {
                for (int i = 0; i < ParentCells.Count; i++) if (graphCell.Equals(ParentCells[i])) return true;
                return false;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                if (!(obj is GraphCell)) return false;
                GraphCell other = obj as GraphCell;
                return this.Coords == other.Coords;
            }

            public GraphCell GetShorterParent()
            {
                int betterIndx = 0;
                for (int i = 0; i < ParentCells.Count; i++) { if (ParentCells[i].Distance < ParentCells[betterIndx].Distance) betterIndx = i; }
                return ParentCells[betterIndx];
            }

            public object Clone()       //doesent work when parent or child cells lists are null
            {
                GraphCell graphCell = new GraphCell(Distance, (Coordinates)Coords.Clone());
                graphCell.ParentCells = new List<GraphCell>(ParentCells);
                graphCell.ChildCells = new List<GraphCell>(ChildCells);
                return graphCell;
            }

            public override int GetHashCode()
            {
                int hashCode = 1987197245;
                hashCode = hashCode * -1521134295 + EqualityComparer<List<GraphCell>>.Default.GetHashCode(ParentCells);
                hashCode = hashCode * -1521134295 + EqualityComparer<List<GraphCell>>.Default.GetHashCode(ChildCells);
                hashCode = hashCode * -1521134295 + Distance.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<Coordinates>.Default.GetHashCode(Coords);
                return hashCode;
            }
        }
    }
}
