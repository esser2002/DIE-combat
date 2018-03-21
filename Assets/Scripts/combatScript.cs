using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class combatScript : MonoBehaviour {

    public Text mainText;
    public int roundCounter;
    System.Random rnd = new System.Random();

    //private int hp;
    //private int attack;
    //private int defence;
    //private int dam;


    public class Warrior
    {
        public string Name;
        public int Hp;
        public int Attack;
        public int Defence;
        public int Dam;        

        public Warrior(int hp, int attack, int defence, int dam, string name)
        {
            Hp = hp;
            Attack = attack;
            Defence = defence;
            Dam = dam;
            Name = name;
        } 

        public void TakeDamage(int dam)
        {
            Hp -= dam;
        }

        public int CalcAttack(System.Random rnd)
        {
            return (rnd.Next(1, Attack +1));
        }       

       public int CalcDefence(System.Random rnd)
        {
            return (rnd.Next(1, Defence + 1));
        }

        public int CalcDamage(System.Random rnd)
        {
            return (rnd.Next(1, Dam +1) + Dam);
        }

        public override string ToString()
        {
            return Name + " HP: " + Hp;
        }
    }

	// Use this for initialization
	void Start () {
        mainText.text = "";        
        Warrior orc1 = new Warrior(1, 10, 5, 3, "Orc");                
        Warrior player = new Warrior(1, 10, 5, 3, "Fjeldulf");
        Warrior Gundar = new Warrior(10, 5, 5, 2, "Gundar");
        CombatStart(player, orc1);
        CombatStart(player, Gundar);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CombatStart(Warrior w1, Warrior w2)
    {
        printNewline("The combat begins!");
        roundCounter = 0;
        combatOverCheck(w1, w2);
    }

    public void CombatRound (Warrior w1, Warrior w2)
    {
        roundCounter += 1;     
        int attackValueW1 = w1.CalcAttack(rnd);
        int attackValueW2 = w2.CalcAttack(rnd);
        int defenceValueW1 = w1.CalcDefence(rnd);
        int defenceValueW2 = w2.CalcDefence(rnd);        
        if (attackValueW1 > defenceValueW2)
        {
            int currentDam = w1.CalcDamage(rnd);
            w2.TakeDamage(currentDam);
            printNewline(w1.Name + " strikes " + w2.Name + " for " + currentDam + " points of damage");
        }
        else
        {
            printNewline(w1.Name + " is parried by " + w2.Name);
        }
        
        if (attackValueW2 > defenceValueW1)
        {
            int currentDam = w2.CalcDamage(rnd);
            w1.TakeDamage(currentDam);
            printNewline(w2.Name + " strikes " + w1.Name + " for " + currentDam + " points of damage");
        }
        else
        {
            printNewline(w2.Name + " is parried by " + w1.Name);
        }
        combatOverCheck(w1, w2);
        
    }

    public void combatOverCheck(Warrior w1, Warrior w2)
    {
        printNewline(w1.Name + " has " + w1.Hp + " HP left");
        printNewline(w2.Name + " has " + w2.Hp + "HP left");
        if (w1.Hp > 0 && w2.Hp > 0)
        {
            printNewline("combat carries on");
            CombatRound(w2, w1);
        }
        else
        {            
            if (w1.Hp < 1 && w2.Hp < 1)
            {
                printNewline(w1.Name + " and " + w2.Name + " are both dead");
                printNewline("Nobody is Victorious");
            }
            else
            {
                if (w1.Hp < 1)
                {
                    printNewline(w1.Name + " has been slain");
                    printNewline(w2.Name + " is victorious");
                }
                else
                {
                    printNewline(w2.Name + " has been slain");
                    printNewline(w1.Name + " is victorious");
                }
            }
        }
    }

    public void printNewline(string newText)
    {        
        mainText.text += newText.ToString() + "\n";        
    }
}
