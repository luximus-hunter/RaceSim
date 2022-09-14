using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {
        public string Name { get; }
        public LinkedList<Section> Sections { get; }

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = new LinkedList<Section>();

            foreach (SectionTypes sectionType in sections)
            {
                Sections.AddLast(new Section(sectionType));
            }
        }
    }
}