﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Assert.IsNull(_competition.NextTrack());
        }
    }
}
