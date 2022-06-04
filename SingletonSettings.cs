using GameParts.GameCore.OtherSoftware;

namespace MouseMaze
{
    public class SingletonSettings
    {
        static Settings settings;

        public static Settings GetInstance()
        {
            if (settings == null) settings = new Settings();
            return settings;
        }
    }
}
