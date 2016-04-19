using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.Entities
{
    class GreyHouse
    {
        private int startingPosition;
        private int raceTrackLength;
        //private PictureBox myPictureBox;
        public int location;
        public Random randomizer;

        public bool Run()
        {
            bool winner = false;
            int random = randomizer.Next(0,4); //minValue = 0 and maxValue = 4
            location += random;
            if (location >= raceTrackLength)
            {
                winner = true;
            }
            return winner; 
        }

        public void TakeStartingPosition()
        {
            SetLocation(startingPosition);
        }

        public void SetStartingPosition(int startingPosition)
        {
            this.startingPosition = startingPosition;
        }

        public void SetRaceTrackLength(int raceTrackLength)
        {
            this.raceTrackLength = raceTrackLength;
        }

        public void SetLocation (int location)
        {
            this.location = location;
        }

        public int GetStartingPosition ()
        {
            return startingPosition;
        }

        public int GetRaceTrackLength ()
        {
            return raceTrackLength;
        }

        public int GetLocation ()
        {
            return location;
        }
    }
}
