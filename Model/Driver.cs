using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipiment Equipiment { get; set; }
        public TeamColors TeamColor { get; set; }

        public Driver(string name, int points, IEquipiment equipiment, TeamColors teamColor)
        {
            Name = name;
            Points = points;
            Equipiment = equipiment;
            TeamColor = teamColor;
        }
    }
}
