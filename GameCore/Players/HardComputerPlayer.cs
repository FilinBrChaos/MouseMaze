using System;
using System.Windows.Controls;
using System.Collections.Generic;
using GameParts.GameCore.OtherSoftware;

namespace GameParts.GameCore.Players
{
    public class HardCompuerPlayer : ComputerPlayer
    {
        public HardCompuerPlayer() : base() { }

        public HardCompuerPlayer(Image image, bool[,] maze) : base(image, maze) { }

        public HardCompuerPlayer(string imgPath, bool[,] maze) : base(imgPath, maze) { }

        public override void Step(StepArgs args)        //override step with mistake chance
        {
            if (Path.Count < 1) return;
            if (random.Next(0, 100) > Difficulty * 100)         //mistake
            {
                List<Directions> possibleDirections = PossibleSteps();
                possibleDirections.Remove(Path[0]);
                if (possibleDirections.Count > 1)
                {
                    possibleDirections.Remove(GetOppositDirection(previousStep));
                    Directions direction = possibleDirections[random.Next(possibleDirections.Count)];
                    Path.Insert(0, GetOppositDirection(direction));
                    Path.Insert(0, direction);
                }
            }
            Directions thisStep = Path[0];
            previousStep = thisStep;
            Path.RemoveAt(0);
            base.Step(new StepArgs(new StepArgs.DirectionField(thisStep)));
        }

        public override List<Directions> FindPath(PlayerBase finish)
        {
            List<Directions> path = new List<Directions>();
            List<GraphCell> graph = new List<GraphCell>();
            GraphCell thisCell = new GraphCell(0, finish.Coords);
            GraphCell startCell = new GraphCell(-1, finish.Coords);
            GraphCell childCell;

            //first iteration because first element doesen't have parents and it will be exception in loop
            if (IfStepPossible(thisCell.Coords, Directions.Right))
            {
                childCell = new GraphCell(thisCell.Distance + 1, StepRight((Coordinates)thisCell.Coords.Clone()));
                thisCell.AddChildCells(childCell); graph.Add(childCell);
            }
            if (IfStepPossible(thisCell.Coords, Directions.Left))
            {
                childCell = new GraphCell(thisCell.Distance + 1, StepLeft((Coordinates)thisCell.Coords.Clone()));
                thisCell.AddChildCells(childCell); graph.Add(childCell);
            }
            if (IfStepPossible(thisCell.Coords, Directions.Up))
            {
                childCell = new GraphCell(thisCell.Distance + 1, StepUp((Coordinates)thisCell.Coords.Clone()));
                thisCell.AddChildCells(childCell); graph.Add(childCell);
            }
            if (IfStepPossible(thisCell.Coords, Directions.Down))
            {
                childCell = new GraphCell(thisCell.Distance + 1, StepDown((Coordinates)thisCell.Coords.Clone()));
                thisCell.AddChildCells(childCell); graph.Add(childCell);
            }

            //BFS serch
            for (int i = 0; i < Maze.GetLength(0) * Maze.GetLength(1); i++)
            {
                if (i >= graph.Count) break;
                thisCell = graph[i];

                if (IfStepPossible(thisCell.Coords, Directions.Right))
                {
                    childCell = new GraphCell(thisCell.Distance + 1, StepRight((Coordinates)thisCell.Coords.Clone()));

                    if (graph.Contains(childCell)) { graph[graph.IndexOf(childCell)].ParentCells.Add(thisCell); }       //this row deal with crossroads in maze
                    else if (!thisCell.IsEqualToParentCells(childCell)) { thisCell.AddChildCells(childCell); graph.Add(childCell); }
                    if (childCell.Coords == Coords) { startCell = childCell; break; }
                }
                if (IfStepPossible(thisCell.Coords, Directions.Left))
                {
                    childCell = new GraphCell(thisCell.Distance + 1, StepLeft((Coordinates)thisCell.Coords.Clone()));

                    if (graph.Contains(childCell)) { graph[graph.IndexOf(childCell)].ParentCells.Add(thisCell); }
                    else if (!thisCell.IsEqualToParentCells(childCell)) { thisCell.AddChildCells(childCell); graph.Add(childCell); }
                    if (childCell.Coords == Coords) { startCell = childCell; break; }
                }
                if (IfStepPossible(thisCell.Coords, Directions.Up))
                {
                    childCell = new GraphCell(thisCell.Distance + 1, StepUp((Coordinates)thisCell.Coords.Clone()));

                    if (graph.Contains(childCell)) { graph[graph.IndexOf(childCell)].ParentCells.Add(thisCell); }
                    else if (!thisCell.IsEqualToParentCells(childCell)) { thisCell.AddChildCells(childCell); graph.Add(childCell); }
                    if (childCell.Coords == Coords) { startCell = childCell; break; }
                }
                if (IfStepPossible(thisCell.Coords, Directions.Down))
                {
                    childCell = new GraphCell(thisCell.Distance + 1, StepDown((Coordinates)thisCell.Coords.Clone()));

                    if (graph.Contains(childCell)) { graph[graph.IndexOf(childCell)].ParentCells.Add(thisCell); }
                    else if (!thisCell.IsEqualToParentCells(childCell)) { thisCell.AddChildCells(childCell); graph.Add(childCell); }
                    if (childCell.Coords == Coords) { startCell = childCell; break; }
                }
            }

            if (startCell.Distance == -1) throw new ArgumentException("Path not found");
            thisCell = startCell;

            //find shorter path from player[thisCell] to finish[first cell]
            while (thisCell.Distance > 0)
            {
                GraphCell tempCell = thisCell.GetShorterParent();
                path.Add(FindDirection(thisCell, tempCell));
                thisCell = tempCell;
            }

            return path;
        }
    }
}
