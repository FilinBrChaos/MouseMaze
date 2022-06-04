using GameParts.GameCore.Players;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;

namespace GameParts.GameCore.OtherSoftware
{
    public class Settings
    {
        bool[,] maze;

        public Player Player { get; set; }

        public ComputerPlayer ComputerPlayer { get; set; }

        public PlayerBase Finish { get; set; }

        public Color Barrier { get; set; }

        public int BarrierThickness { get; set; }

        public Color Corridor { get; set; }

        public Difficulty CurrentDifficulty { get; set; }

        public enum Difficulty { VeryEasy, Easy, Difficult, Hard, Extreme }

        public bool[,] Maze { get => maze; set { maze = value; OnMazeChanged(); } }

        public event Action PlayersInstancesChanged;

        public event Action MazeChanged;

        public Settings()
        {
            Player = new Player();
            ComputerPlayer = new ComputerPlayer();
            Finish = new Player();
            Barrier = Colors.Black;
            Corridor = Colors.White;
            BarrierThickness = 2;
            Maze = Player.Maze;
        }

        public Settings(Player player, ComputerPlayer computerPlayer, bool[,] maze, Player finish, Color barrier, Color corridor, int barrierThickness)
        {
            Player = player;
            ComputerPlayer = computerPlayer;
            Finish = finish;
            Barrier = barrier;
            Corridor = corridor;
            BarrierThickness = barrierThickness;
            Maze = maze;
        }

        public void Hard(double difficulty = 0.8, int averageSpeed = 250)       //this method changed ComputerPlayer instance and some parameters 
        {                                                                       //to change game difficulty
            
            ComputerPlayer temp = ComputerPlayer;
            ComputerPlayer = new HardCompuerPlayer(temp.Image, Maze);
            ComputerPlayer.Image.Tag = temp.Image.Tag;
            ComputerPlayer.Difficulty = difficulty;
            ComputerPlayer.AverageSpeed = averageSpeed;
            ComputerPlayer.Coords = temp.Coords;
            if (temp.GetType().Equals(new EasyComputerPlayer().GetType()))
            {
                ComputerPlayer.Path = ComputerPlayer.FindPath(Finish);
            }
            else ComputerPlayer.Path = temp.Path;
            OnPlayersInstancesChanged();
        }

        public void Easy(double difficulty = 0.5, int averageSpeed = 300)       //same as Hard method
        {
            ComputerPlayer temp = ComputerPlayer;
            ComputerPlayer = new EasyComputerPlayer(temp.Image, Maze);
            ComputerPlayer.Image.Tag = temp.Image.Tag;
            ComputerPlayer.Difficulty = difficulty;
            ComputerPlayer.AverageSpeed = averageSpeed;
            ComputerPlayer.Coords = temp.Coords;
            ComputerPlayer.Path = temp.Path;
            OnPlayersInstancesChanged();
        }

        void OnPlayersInstancesChanged()
        {
            PlayersInstancesChanged?.Invoke();
        }

        void OnMazeChanged()
        {
            Player.Maze = Maze;
            ComputerPlayer.Maze = Maze;
            Finish.Maze = Maze;
            MazeChanged?.Invoke();
        }

        public static Image GetImage(string path)
        {
            Uri uri = new Uri(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + path);
            Image image = new Image();
            image.Source = new BitmapImage(uri);
            return image;
        }
    }
}
