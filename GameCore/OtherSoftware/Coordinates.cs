using System;

namespace GameParts.GameCore.OtherSoftware
{
    public class Coordinates : ICloneable
    {
        int row, column;

        public int Row { get => row; set { if (value >= 0) row = value; else throw new ArgumentException("Row value must be larger than zero."); } }

        public int Column { get => column; set { if (value >= 0) column = value; else throw new ArgumentException("Column value must be larger than zero."); } }

        public Coordinates(int row, int column) { Row = row; Column = column; }

        public static bool operator ==(Coordinates a, Coordinates b)
        {
            return a.Row == b.Row && a.Column == b.Column;
        }

        public static bool operator !=(Coordinates a, Coordinates b)
        {
            return !(a == b);
        }

        public object Clone()
        {
            return new Coordinates(Row, Column);
        }

        public override string ToString()
        {
            return Row + ", " + Column + ";";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Coordinates)) return false;
            Coordinates other = obj as Coordinates;
            return this == other;
        }

        public override int GetHashCode()
        {
            int hashCode = -1663278630;
            hashCode = hashCode * -1521134295 + row.GetHashCode();
            hashCode = hashCode * -1521134295 + column.GetHashCode();
            return hashCode;
        }
    }
}
