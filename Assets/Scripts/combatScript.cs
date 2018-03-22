using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class combatScript : MonoBehaviour {

    public Text mainText;
    public int roundCounter;
    System.Random rnd = new System.Random();
    public int orcsSlain;

    //private int hp;
    //private int attack;
    //private int defence;
    //private int dam;
    public class Weapon
    {
        public string Name;
        public int Dam;
        public int AttackMod;
        public int DefenceMod;

        public Weapon(int dam, int attmod, int defmod, string name)
        {
            Dam = dam;
            AttackMod = attmod;
            DefenceMod = defmod;
            Name = name;
        }
    }

    public class OffHand
    {
        public string Name;
        public int DefenceBonus;
        public int DefenceMod;
        public string Type;
        

        public OffHand(int defbonus, int defencemod, string name, string type)
        {
            DefenceBonus = defbonus;
            DefenceBonus = defencemod;
            Name = name;
            Type = type;
        }
    }

    public class Item
    {
        //public interface IActiveEffect { };       
    }

    public class Warrior
    {
        public string Name;
        public int Hp;
        public int AttackTrue;
        public int DefenceTrue;
        public int AttackEffective;
        public int DefenceEffective;
        public int Dam;
        public Weapon EquippedWeapon;
        public OffHand EquippedOffHand;
        public List<Item> Inventory;

        public Warrior(int hp, int attack, int defence, string name, Weapon weapon, OffHand offhand)
        {
            Hp = hp;
            AttackTrue = attack;
            DefenceTrue = defence;            
            Name = name;
            EquippedWeapon = weapon;
            EquippedOffHand = offhand;

            UpdateStats();
        } 
        public void UpdateStats()
        {
            AttackEffective = AttackTrue + EquippedWeapon.AttackMod;
            DefenceEffective = DefenceTrue + EquippedWeapon.DefenceMod;
            Dam = EquippedWeapon.Dam;
        }

        public void TakeDamage(int dam)
        {
            Hp -= dam;
        }

        public int CalcAttack(System.Random rnd)
        {
            return (rnd.Next(1, AttackEffective +1));
        }       

       public int CalcDefence(System.Random rnd)
        {
            return (rnd.Next(1, DefenceEffective + 1) + EquippedOffHand.DefenceBonus);
        }

        public int CalcDamage(System.Random rnd)
        {
            return (rnd.Next(0, Dam +1) + Dam);
        }

        public override string ToString()
        {
            return Name + " HP: " + Hp;
        }
    }

	// Use this for initialization
	void Start () {
        mainText.text = "";        
        Weapon Axe = new Weapon(4, 0, 0, "axe");
        OffHand none = new OffHand(0, 0, "nothing", "NONE");
        OffHand shield = new OffHand(1, 3, "shield", "SHIELD");        
        Warrior player = new Warrior(10, 10, 10, "Fjeldulf", Axe, none);
        Warrior Gundar = new Warrior(10, 10, 5, "Gundar", Axe, shield);
//Warrior orc1 = new Warrior(5, 10, 5, "Orc", Axe, none);

        List<Warrior> orcs = new List<Warrior>();
        for (int i = 0; player.Hp > 0; i++)
        {
            orcs.Add(new Warrior(5, 10, 5, "Orc", Axe, none));
            CombatStart(player, orcs[i]);
            orcsSlain = i;
        }
        PrintNewline(player.Name + " died after slaying " + orcsSlain + " orcs");
        // for (int i = 0; 0 < player.Hp; i++)
        // {
        //     Warrior orci = new 
        // }
        //CombatStart(player, orc1);        
        //CombatStart(player, Gundar);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CombatStart(Warrior w1, Warrior w2)
    {
        PrintNewline("The combat begins!");
        roundCounter = 0;
        CombatOverCheck(w1, w2, true);
    }

    // The body of combat rounds. Ends in CombatOverCheck() or a result
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
            PrintHitAttack(w1, w2, currentDam);
        }
        else
        {
            PrintParry(w1, w2);
        }
        
        if (attackValueW2 > defenceValueW1)
        {
            int currentDam = w2.CalcDamage(rnd);
            w1.TakeDamage(currentDam);
            PrintHitAttack(w2, w1, currentDam);
        }
        else
        {
            PrintParry(w2, w1);
        }
        CombatOverCheck(w1, w2, false);
        
    }

    public void CombatOverCheck(Warrior w1, Warrior w2, bool firstRound)
    {
        PrintNewline(w1.Name + " has " + w1.Hp + " HP left");
        PrintNewline(w2.Name + " has " + w2.Hp + "HP left");
        if (w1.Hp > 0 && w2.Hp > 0)
        {
            if (!firstRound)
            {
                PrintNewline("Combat carries on");
            }
            CombatRound(w1, w2);
        }
        else
        {            
            if (w1.Hp < 1 && w2.Hp < 1)
            {
                PrintNewline(w1.Name + " and " + w2.Name + " are both dead");
                PrintNewline("Nobody is Victorious");
            }
            else
            {
                if (w1.Hp < 1)
                {
                    PrintNewline(w1.Name + " has been slain");
                    PrintNewline(w2.Name + " is victorious");
                }
                else
                {
                    PrintNewline(w2.Name + " has been slain");
                    PrintNewline(w1.Name + " is victorious");
                }
            }
        }
    }

    public void PrintNewline(string newText)
    {        
        mainText.text += newText.ToString() + "\n";        
    }

    public void PrintHitAttack(Warrior attacker, Warrior defender, int currentDam)
    {
        PrintNewline(attacker.Name + " strikes " + defender.Name + " with his " + attacker.EquippedWeapon.Name + " for " + currentDam + " points of damage");
    }

    public void PrintParry(Warrior attacker, Warrior defender)
    {
        if (ShieldWeild(defender.EquippedOffHand))
        {
            PrintNewline(attacker.Name + "'s attack is blocked by " + defender.Name + "'s " + defender.EquippedOffHand.Name);
        }
        else
        {
            PrintNewline(attacker.Name + "'s " + attacker.EquippedWeapon.Name + " is parried by " + defender.Name);
        }        
    }
    public bool ShieldWeild(OffHand offHand)
    {
        if (offHand.Type == "SHIELD")
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }
}
