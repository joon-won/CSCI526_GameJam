using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    public class Dice
    {
        public int sides;
        public int face;

        public Dice(int sides)
        {
            this.sides = sides;            
        }

        public void Roll()
        {            
            this.face = UnityEngine.Random.Range(1, sides+1);
        }
    }

    public class DiceRoll
    {
        public List<Dice> dice;

        public DiceRoll()
        {
            dice = new List<Dice>();
        }

        public void AddDice(int side)
        {
            dice.Add(new Dice(side));
        }

        public void Roll()
        {
            for (int diceno=0; diceno < dice.Count; diceno++)
            {
                dice[diceno].Roll();
            }
        }

        public int TotalValue()
        {
            int value = 0;
            for (int idx=0; idx < dice.Count; idx++)
            {
                value += dice[idx].face;
            }

            return value;
        }
    }
}
