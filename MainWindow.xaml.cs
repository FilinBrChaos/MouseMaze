using System;
using GameParts.GameCore.OtherSoftware;
using GameParts.GameCore.Players;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using MouseMaze.Windowses;
using System.IO;

namespace MouseMaze
{
    public partial class MainWindow : Window
    {
        MazeManager mazeWalker;
        Timer timer = new Timer();

        public MainWindow()
        {
            InitializeComponent();

            KeyDown += MainWindow_KeyDown;
            MouseDown += MainWindow_MouseDown;
            this.SizeChanged += Cont_SizeChanged;
            Blur.Radius = 0;

            string mediaFolderPath = "Media" + Path.DirectorySeparatorChar + "GameModels" + Path.DirectorySeparatorChar;

            Settings settings = SingletonSettings.GetInstance();
            settings.Maze = CreateMaze.ConvertMaze("Media" + Path.DirectorySeparatorChar + "Maze" + Path.DirectorySeparatorChar + "maze1.png", 14);

            settings.Player = new Player(mediaFolderPath + "mouse.png", settings.Maze);
            settings.Finish = new Player(mediaFolderPath + "cheese.png", settings.Maze);
            settings.ComputerPlayer = new ComputerPlayer(mediaFolderPath + "enemy.png", settings.Maze);
            settings.Finish.Coords = new Coordinates(2, 3);
            settings.Corridor = Colors.DarkGray;
            settings.ComputerPlayer.Difficulty = 0.7;

            mazeWalker = new MazeManager(settings);
            Cont.Content = mazeWalker.Initialize();

            timer.Interval = 300;
            timer.Tick += Timer_Tick;

            settings.Hard();
            settings.CurrentDifficulty = Settings.Difficulty.Hard;

            settings.MazeChanged += Settings_MazeChanged;
            mazeWalker.GameFinished += MazeWalker_GameFinished;
            RestartGame();
        }

        void RestartGame()
        {
            mazeWalker.Restart();
            DelayBeforeGameAndStart();
        }

        void DelayBeforeGameAndStart(int delay = 3)
        {
            KeyDown -= MainWindow_KeyDown;

            timer.Stop();
            Timer local = new Timer();
            local.Interval = 1000;
            string timerTag = "timerLabel";
            TextBlock textBlock = new TextBlock();
            textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Tag = timerTag;
            textBlock.FontSize = 80;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            MainGrid.Children.Add(textBlock);
            Blur.Radius = 3;
            local.Tick += (object sender, EventArgs e) => {
                if (delay <= 0)
                {
                    MainGrid.Children.Remove(textBlock);
                    Blur.Radius = 0;
                    KeyDown += MainWindow_KeyDown;
                    timer.Start();
                    local.Stop();
                }
                textBlock.Text = Convert.ToString(delay);
                delay--;
            };
            local.Start();
        }

        private void Settings_MazeChanged()
        {
            Cont.Content = mazeWalker.Initialize();
            mazeWalker.Restart();
        }

        private void MazeWalker_GameFinished(MazeManager.Winer w)
        {
            timer.Stop();

            Blur.Radius = 7;
            EndGameWindow endGameWindow = new EndGameWindow(this, w);
            endGameWindow.Owner = this;
            endGameWindow.ShowDialog();
            Blur.Radius = 0;

            RestartGame();
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void Cont_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double height = ActualHeight, width = ActualWidth;
            int diff = 150;
            if (height < width) { Cont.Height = height - diff; Cont.Width = height - diff; }
            else { Cont.Height = width - diff; Cont.Width = width - diff; }
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape) btnPause_Click(sender, e);
            SingletonSettings.GetInstance().Player.Step(new StepArgs(new StepArgs.KeyField(e.Key)));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Interval = SingletonSettings.GetInstance().ComputerPlayer.AverageSpeed;
            SingletonSettings.GetInstance().ComputerPlayer.Step(new StepArgs());
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            Blur.Radius = 7;
            SettingsWindow settingsWindow = new SettingsWindow(this);
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
            Blur.Radius = 0;

            DelayBeforeGameAndStart();
        }

        private void btnResize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized) WindowState = WindowState.Maximized;
            else WindowState = WindowState.Normal;
        }
    }
}
