using ADayOfBets.Resources;
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

        public Bet(int amount, int dog, Player bettor)
        {
            SetAmount(amount);
            SetDog(dog);
            SetBettor(bettor);
        }

        public string GetDescription()
        {
            string message;
            if (GetAmount() != 0)
            {
                message = Strings.Bettor + ": " + bettor.GetName() + " " + Strings.BetMoney + ": " + GetAmount() + " " + Strings.BetDog + ": " + GetDog();
            }
            else
            {
                message = bettor.GetName() + Strings.DidNotBet;
            }
            return message;
        }

        public int PayOut(int winner) //player will inform the winner dog to the Bet, and then Bet will check if the winner equals the player's dog
        {
            int results;
            if (winner.Equals(dog))
            {
                results = this.amount;
            }
            else
            {
                results = -(this.amount);
            }
            return results;
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
