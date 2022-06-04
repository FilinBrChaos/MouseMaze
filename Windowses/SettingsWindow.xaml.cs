using System;
using System.IO;
using GameParts.GameCore.OtherSoftware;
using System.Windows;
using System.Windows.Input;

namespace MouseMaze.Windowses
{
    public partial class SettingsWindow : Window
    {
        MainWindow parent;
        string maze1 = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Media/Maze/maze1.png";
        string maze2 = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Media/Maze/maze2.png";
        string maze3 = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Media/Maze/maze3.png";

        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            parent = mainWindow;
            KeyDown += SettingsWindow_KeyDown;
            Maze1.Content = maze1;
            Maze2.Content = maze2;
            Maze3.Content = maze3;

            Maze1.Checked += Maze1_Checked;
            Maze2.Checked += Maze2_Checked;
            Maze3.Checked += Maze3_Checked;

            LevelSlider.Value = (int)SingletonSettings.GetInstance().CurrentDifficulty;
            levelTextBlok.Text = ((Settings.Difficulty)Convert.ToInt32(LevelSlider.Value)).ToString();
            LevelSlider.ValueChanged += LevelSlider_ValueChanged;
        }

        private void LevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            levelTextBlok.Text = ((Settings.Difficulty)Convert.ToInt32(LevelSlider.Value)).ToString();
            Settings settings = SingletonSettings.GetInstance();
            switch (Convert.ToInt32(LevelSlider.Value))
            {
                case 0: SingletonSettings.GetInstance().Easy(0.5, 150); settings.CurrentDifficulty = Settings.Difficulty.VeryEasy; break;
                case 1: SingletonSettings.GetInstance().Hard(0.5); settings.CurrentDifficulty = Settings.Difficulty.Easy; break;
                case 2: SingletonSettings.GetInstance().Hard(0.6); settings.CurrentDifficulty = Settings.Difficulty.Difficult; break;
                case 3: SingletonSettings.GetInstance().Hard(0.7); settings.CurrentDifficulty = Settings.Difficulty.Hard; break;
                case 4: SingletonSettings.GetInstance().Hard(0.9, 300); settings.CurrentDifficulty = Settings.Difficulty.Extreme; break;
            }
        }

        private void Maze3_Checked(object sender, RoutedEventArgs e)
        {
            SingletonSettings.GetInstance().Maze = CreateMaze.ConvertMaze(maze3, 7);
        }

        private void Maze2_Checked(object sender, RoutedEventArgs e)
        {
            SingletonSettings.GetInstance().Maze = CreateMaze.ConvertMaze(maze2, 7);
        }

        private void Maze1_Checked(object sender, RoutedEventArgs e)
        {
            SingletonSettings.GetInstance().Maze = CreateMaze.ConvertMaze(maze1, 14);
        }

        private void SettingsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { btnContinue_Click(sender, e); }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            parent.Close();
            Close();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
