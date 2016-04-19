using ADayOfBets.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace teste.Entities
{
    class Player
    {
        private string name;  //player's name
        private Bet playerBet; //player's bet
        private int cash; //player's cash

        //private RadioButton myRadioButton;
        private Label label;

        public Player(string name, int cash)
        {
            SetName (name);
            SetCash (cash);
        }

        public void UpdateLabels()
        {
            //label = Strings.Bet + playerBet.GetDescription();
            //myRadioButton.text = GetName() + string.Format(Strings.Have + ": {0:C}", GetCash()); 
        }

        public void ClearBet()
        {
            playerBet.SetAmount(0); //set the player's bet to 0
        }

        public bool PlaceBet(int amount, int dog)
        {
            bool enoughtMoney= false;
            if (cash >= 5) //minimum value to place a bet is $5
            {
                playerBet = new Bet(amount, dog, this);
                enoughtMoney = true;
            }
            return enoughtMoney;
        }

        public void Collect(int winner)
        {
            SetCash(GetCash() + playerBet.PayOut(winner)); //get the acctual cash + Bet's returns which is a value positive or negative
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetCash(int cash)
        {
            this.cash = cash;
        }

        public string GetName()
        {
            return name;
        }

        public int GetCash()
        {
            return cash;
        }
    }
}
