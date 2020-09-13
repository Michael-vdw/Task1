using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesandGoblins
{
    public partial class Form1 : Form
    {
        abstract class Tile 
        {
            private protected int x, y;
            public int X { get => x; set => x = value; }
            public int Y { get => y; set => y = value; }

            public enum TileType
            {
                Hero,
                Enemy,
                Gold,
                Weapon
            }

            public Tile(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            class Obstacle : Tile
            {
                public Obstacle() : base(0,0)
                {
                   
                }
            }

            class EmptyTile : Tile
            {
                public EmptyTile() : base(0, 0)
                {

                }
            }
        }
        
        abstract class Character : Tile
        {
            private protected int hp, maxHP, damage;
            private protected int[] Tile;

            public int HP { get => hp; set => hp = value; }
            public int MaxHP { get => maxHP; set => maxHP = value; }
            public int Damage { get => damage; set => damage = value; }
            public enum Movement
            {
                NoMove,
                Up,
                Down,
                Left,
                Right
            }

            public Character(int x, int y, char symbol) : base(x,y)
            {
               
            }

            public virtual void Attack(Character target)
            {

            }

            public bool IsDead()
            {
                return true;
            }

            public virtual bool CheckRange(Character target)
            {
                
                return true;
            }

            private int DistanceTo(target)
            {
                return 0; 
            }

            public void Move(Movement move)
            {

            }

            public abstract Movement ReturnMove(Movement move = 0)
            {

            }

            public abstract override string ToString()
            {

            }
        }

        abstract class Enemy : Character
        {
            Random random = new Random();
            public Enemy(int x, int y, int damage, int hp, int maxHP, char symbol) : base(x,y,symbol)
            {
                
            }
            public override string ToString()
            {
                return "Name at [" + X + "," + Y + "] Damage:" + Damage;
            }
        }

        private class Goblin : Enemy
        {
            public Goblin(int x, int y) : base(x, y, 1, 10, 10, 'G')
            {

            }

            public override Movement ReturnMove(Movement move = random)
            {
                return
            }
        }
       
        public Form1()
        {
            InitializeComponent();
        }
    }
}
