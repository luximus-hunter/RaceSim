using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Race
    {
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = DateTime.Now;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
        }

        public SectionData GetSectionData(Section section)
        {
            SectionData? value = _positions.GetValueOrDefault(section, null);

            if (value == null)
            {
                value = new SectionData(null, 0, null, 0);
                _positions.Add(section, value);
            }

            return value;
        }

        public void RandomizeEquipment()
        {
            for (int i = 0; i < Participants.LongCount(); i++)
            {
                IParticipant participant = Participants[i];

                Random _random = new Random(DateTime.Now.Millisecond);
                participant.Equipment.Quality = (int)_random.NextInt64();
                participant.Equipment.Performance = (int)_random.NextInt64();

                Participants[i] = participant;
            }
        }
    }
}
