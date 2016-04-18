using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.Entities
{
    class Bet
    {
        private int amount; //amount of money which was betted
        private int dog; //dog's id
        private Player bettor; //who's beting

        public string GetDescription()
        {
            string message;
            if (GetAmount() != 0)
            {
                message = teste.Resources.Strings.Bettor + ": " + bettor.GetName() + " " + teste.Resources.Strings.BetMoney + ": " + GetAmount() + " " + teste.Resources.Strings.BetDog + ": " + GetDog();
            }
            else
            {
                message = bettor.GetName() + teste.Resources.Strings.DidNotBet;
            }
            return message;
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }

        public void SetDog(int dog)
        {
            this.dog = dog;
        }

        public void SetBettor(Player bettor)
        {
            this.bettor = bettor;
        }

        public int GetAmount()
        {
            return amount;
        }

        public int GetDog()
        {
            return dog;
        }

        public Player GetBettor()
        {
            return bettor;
        }
    }
}
