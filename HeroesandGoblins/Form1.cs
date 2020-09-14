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
                public Obstacle(int x, int y) : base(x,y)
                {
                   
                }
            }

            class EmptyTile : Tile
            {
                public EmptyTile(int x, int y) : base(x,y)
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
                target.hp -= Damage;
            }

            public bool IsDead()
            {
                if (hp < 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public virtual bool CheckRange(Character target)
            {
                if (DistanceTo(target) < 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            private int DistanceTo(Character target)
            {
                return Math.Abs(target.X - this.X) + Math.Abs(target.Y - this.Y);
            }

            public void Move(Movement move)
            {
                if (move == Movement.Up)
                {
                    y++;
                }
                if (move == Movement.Down)
                {
                    y--;
                }
                if (move == Movement.Left)
                {
                    x--;
                }
                if (move == Movement.Right)
                {
                    x++;
                }
            }

            public abstract Movement ReturnMove(Movement move);

            public abstract override string ToString();
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
