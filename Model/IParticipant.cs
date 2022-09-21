using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
    }

    public enum TeamColors
    {
        White,
        Orange,
        Magenta,
        Sky,
        Yellow,
        Lime,
        Pink,
        Grey,
        Silver,
        Cyan,
        Purple,
        Blue,
        Brown,
        Green,
        Red,
        Black
    }
}