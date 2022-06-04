using System;
using System.Windows.Input;
using GameParts.GameCore.Players;

namespace GameParts.GameCore.OtherSoftware
{
    public class StepArgs : ICloneable
    {
        KeyField key;
        DirectionField direction;

        public bool IsEmpty { get; protected set; }

        public KeyField Key { get => key; set { key = value; IsEmpty = false; } }

        public DirectionField Direction { get => direction; set { direction = value; IsEmpty = false; } }

        public StepArgs() { IsEmpty = true; Key = new KeyField(); Direction = new DirectionField(); }

        public StepArgs(KeyField key) { Key = key; Direction = new DirectionField(); }

        public StepArgs(DirectionField direction) { Direction = direction; Key = new KeyField(); }

        public StepArgs(KeyField key, DirectionField direction) { Key = key; Direction = direction; }

        public class KeyField : ICloneable
        {
            Key key;

            public bool IsEmpty { get; protected set; }

            public Key Value { get => key; set { key = value; IsEmpty = false; } }

            public KeyField() { IsEmpty = true; }

            public KeyField(Key key) { Value = key; }

            public object Clone()
            {
                if (IsEmpty) return new KeyField();
                return new KeyField(key);
            }
        }

        public class DirectionField : ICloneable
        {
            PlayerBase.Directions direction;

            public bool IsEmpty { get; protected set; }

            public PlayerBase.Directions Value { get => direction; set { direction = value; IsEmpty = false; } }

            public DirectionField() { IsEmpty = true; }

            public DirectionField(PlayerBase.Directions direction) { Value = direction; }

            public object Clone()
            {
                if (IsEmpty) return new DirectionField();
                return new DirectionField(direction);
            }
        }

        public object Clone()
        {
            return new StepArgs((KeyField)Key.Clone(), (DirectionField)Direction.Clone());
        }
    }
}
