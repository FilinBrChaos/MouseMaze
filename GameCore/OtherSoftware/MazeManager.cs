using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameParts.GameCore.Players;

namespace GameParts.GameCore.OtherSoftware
{
    public class MazeManager
    {
        Settings settings;
        Grid gridMaze;

        public enum Winer { Player, Computer }

        public delegate void GameFinishedEventHandler(Winer w);

        public event GameFinishedEventHandler GameFinished;

        void OnGameFinished(Winer w)
        {
            GameFinished?.Invoke(w);
        }

        public MazeManager(Settings settings)
        {
            gridMaze = new Grid();
            this.settings = settings;
            settings.PlayersInstancesChanged += Settings_PlayersInstancesChanged;
            Settings_PlayersInstancesChanged();
        }

        public Grid Initialize()        //this mrthod create and fill new maze (technically grid)
        {
            RemovePlayer(settings.Player);
            RemovePlayer(settings.ComputerPlayer);
            RemovePlayer(settings.Finish);
            gridMaze = new Grid();
            gridMaze.Background = new SolidColorBrush(settings.Corridor);

            for (int i = 0; i < settings.Maze.GetLength(0); i++)
            {
                RowDefinition row = new RowDefinition();
                if (i % 2 == 0) { row.Height = new GridLength(settings.BarrierThickness); }
                gridMaze.RowDefinitions.Add(row);

                for (int j = 0; j < settings.Maze.GetLength(1); j++)
                {
                    if (i == 0)
                    {
                        ColumnDefinition column = new ColumnDefinition();
                        if (j % 2 == 0) { column.Width = new GridLength(settings.BarrierThickness); }
                        gridMaze.ColumnDefinitions.Add(column);
                    }

                    Canvas canvas = new Canvas();
                    if (settings.Maze[i, j]) canvas.Background = new SolidColorBrush(settings.Corridor);
                    else canvas.Background = new SolidColorBrush(settings.Barrier);
                    AddToGrid(canvas, i, j);
                }
            }
            Refresh();

            return gridMaze;
        }

        public void Restart()       //this method put players and finish in a new different places
        {
            Coordinates[] points = GetRandomPlacesOnMaze(settings.Maze);
            settings.Player.Coords = points[0];
            settings.ComputerPlayer.Coords = points[1];
            settings.Finish.Coords = points[2];
            settings.ComputerPlayer.Path = settings.ComputerPlayer.FindPath(settings.Finish);
            Refresh();
        }

        public void Refresh()
        {
            Player_PlayerPositionChanged(settings.Player);
            Player_PlayerPositionChanged(settings.ComputerPlayer);
            Player_PlayerPositionChanged(settings.Finish);
        }

        private void Settings_PlayersInstancesChanged()     //this eventHandler refresh other eventHandlers when some players instances have been changed
        {
            settings.Player.PlayerPositionChanged -= Player_PlayerPositionChanged;
            settings.Player.PlayerPositionChanged += Player_PlayerPositionChanged;
            settings.ComputerPlayer.PlayerPositionChanged -= Player_PlayerPositionChanged;
            settings.ComputerPlayer.PlayerPositionChanged += Player_PlayerPositionChanged;
            settings.Finish.PlayerPositionChanged -= Player_PlayerPositionChanged;
            settings.Finish.PlayerPositionChanged += Player_PlayerPositionChanged;
        }

        private void Player_PlayerPositionChanged(PlayerBase player)        //eventHandler refresh player position when it's coords have been changed
        {
            RemovePlayer(player);
            DrawPlayer(player);
            if (!player.Equals(settings.Finish) && player.Coords == settings.Finish.Coords)
            {
                if (player.GetType().Equals(new Player().GetType())) OnGameFinished(Winer.Player);
                else OnGameFinished(Winer.Computer);
            }
        }

        public void DrawPlayer(PlayerBase player)
        {
            AddToGrid(player.Image, player.Coords.Row, player.Coords.Column);
        }

        public void RemovePlayer(PlayerBase player)
        {
            //Image image = FindControlWithTag<Image>(gridMaze, tag);
            //if (image == null) return;
            gridMaze.Children.Remove(player.Image);
        }

        void AddToGrid(UIElement element, int i, int j)
        {
            Grid.SetRow(element, i);
            Grid.SetColumn(element, j);
            gridMaze.Children.Add(element);
        }

        T FindControlWithTag<T>(DependencyObject parent, FrameworkElement tag) where T : UIElement
        {
            List<UIElement> elements = new List<UIElement>();

            int count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (typeof(FrameworkElement).IsAssignableFrom(child.GetType())
                        && (((FrameworkElement)child).Tag == tag))
                    {
                        return child as T;
                    }
                    var item = FindControlWithTag<T>(child, tag);
                    if (item != null) return item as T;
                }
            }
            return null;
        }

        public static Coordinates[] GetRandomPlacesOnMaze(bool[,] maze)         //generate random different coordinates
        {
            Coordinates[] points = new Coordinates[3];
            Coordinates zero = new Coordinates(0, 0);
            points[0] = new Coordinates(0, 0);
            points[1] = new Coordinates(0, 0);
            points[2] = new Coordinates(0, 0);
            Random random = new Random();
            bool noZero;

            //i can do it better but...

            while (true)
            {
                int i = (random.Next(0, maze.GetLength(0) / 2) * 2) + 1;
                int j = (random.Next(0, maze.GetLength(1) / 2) * 2) + 1;

                Coordinates point = new Coordinates(i, j);
                if (maze[i, j] && points[0] == zero)
                { if (point != points[1] && point != points[2]) points[0] = point; }

                if (maze[i, j] && points[1] == zero)
                { if (point != points[0] && point != points[2]) points[1] = point; }

                if (maze[i, j] && points[2] == zero)
                { if (point != points[0] && point != points[1]) points[2] = point; }

                noZero = true;
                for (int k = 0; k < points.Length; k++) { if (points[k] == zero) noZero = false; }
                if (noZero) break;
            }

            return points;
        }

    }
}
