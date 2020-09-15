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
        private static readonly char cHero = 'H';
        private static readonly char cGoblin = 'G';
        private static readonly char cEmpty = '.';
        private static readonly char cObstacle = 'X';
        GameEngine gameengine;
        abstract class Tile 
        {
            private protected int x, y;
            public int X { get => x; set => x = value; }
            public int Y { get => y; set => y = value; }

            public TileType thisTile;
            public TileType ThisTile { get => thisTile; set => thisTile = value; }
            public enum TileType
            {
                Hero,
                Enemy,
                Gold,
                Weapon,
                Empty
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
            private protected int symbol;
            private protected Tile[] vision; 

            public int HP { get => hp; set => hp = value; }
            public int MaxHP { get => maxHP; set => maxHP = value; }
            public int Damage { get => damage; set => damage = value; }
            public int Symbol { get => symbol; set => symbol = value; }
            public Tile[] Vision { get => vision; set => vision = value; }
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
                return nameof(Enemy) + " at [" + X + "," + Y + "] Damage:" + Damage;
            }
        }

        private class Goblin : Enemy
        {
            public Goblin(int x, int y) : base(x, y, 1, 10, 10, 'G')
            {

            }

            public override Movement ReturnMove(Movement move)
            {
                Random random = new Random();
                int randomroll = random.Next(1, 5);

                while (vision[randomroll].thisTile != TileType.Empty)
                {
                    randomroll = random.Next(1, 5);
                }
                return (Movement)randomroll;
            }
        }

        private class Hero : Character
        {
            public Hero(int x, int y, int hp) : base(x, y, 'H')
            {
                this.hp = hp;
                this.maxHP = hp;
                this.damage = 2;
            }

            public override Movement ReturnMove(Movement move)
            {
                if (vision[Convert.ToInt32(move)].thisTile != TileType.Empty)
                {
                    return Movement.NoMove;
                }
                else
                {
                    return move;
                }
            }

            public override string ToString()
            {
                return "Player stats: \nHP:" + hp + "/" + maxHP + "\nDamage:" + damage + "[" + x + "," + y + "]"; 
            }
        }

        class Map
        {
            private Tile[,] tileMap;
            private Hero player;
            private Enemy[] enemies;
            private int minWidth, maxWidth, minHeight, maxHeight, height, width;
            Random randomnum;

            public int MinWidth { get => minWidth; set => minWidth = value; }
            public int MaxWidth { get => maxWidth; set => maxWidth = value; }
            public int MinHeight { get => minHeight; set => minHeight = value; }

            public Map(int minwidth, int maxwidth, int minheight, int maxheight, int enemies)
            {
                height = randomnum.Next(minheight, maxheight);
                width = randomnum.Next(minwidth, maxwidth);

                Tile[,] newMap = new Tile[width, height];
                Enemy[] newEnemy = new Enemy[enemies];

                Create(Tile.TileType.Hero);

                for ( int i = 0; i > enemies; i++)
                {
                    Create(Tile.TileType.Enemy);
                }
            }
            public void UpdateVision()
            {
                player.Vision[1] = tileMap[player.X, player.Y + 1];
                player.Vision[2] = tileMap[player.X, player.Y - 1];
                player.Vision[3] = tileMap[player.X - 1, player.Y];
                player.Vision[4] = tileMap[player.X + 1, player.Y];

                for (int i = 1; i > enemies.Length; i++)
                {
                    enemies[i].Vision[1] = tileMap[enemies[i].X, enemies[i].Y + 1];
                    enemies[i].Vision[2] = tileMap[enemies[i].X, enemies[i].Y - 1];
                    enemies[i].Vision[3] = tileMap[enemies[i].X - 1, enemies[i].Y];
                    enemies[i].Vision[4] = tileMap[enemies[i].X + 1, enemies[i].Y];
                }
            }

            private Tile Create(Tile.TileType type)
            {
                int x = randomnum.Next(1, width);
                int y = randomnum.Next(1, height);

                while (tileMap[x,y].ThisTile != Tile.TileType.Empty)
                {
                    x = randomnum.Next(1, width);
                    y = randomnum.Next(1, height);
                }
                if (type == Tile.TileType.Hero)
                {
                    return new Hero(x, y, 40);
                }
                else
                {
                    return new Goblin(x, y);
                }
            }
        }

        class GameEngine
        {
            private Map engineMap;
            private Hero player;
            public Map EngineMap { get => engineMap; set => engineMap = value; }

            public GameEngine() 
            {
                Map newmap = new Map(5,15,5,15,5);
            }

            public bool MovePlayer(Character.Movement move)
            {
                if (move == Character.Movement.Down)
                {
                    if (player.Vision[2].thisTile == Tile.TileType.Empty)
                    {
                        player.Move(Character.Movement.Down);
                        EngineMap.UpdateVision();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (move == Character.Movement.Right)
                {
                    if (player.Vision[4].thisTile == Tile.TileType.Empty)
                    {
                        player.Move(Character.Movement.Right);
                        EngineMap.UpdateVision();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (move == Character.Movement.Left)
                {
                    if (player.Vision[3].thisTile == Tile.TileType.Empty)
                    {
                        player.Move(Character.Movement.Left);
                        EngineMap.UpdateVision();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (move == Character.Movement.Up)
                {
                    if (player.Vision[1].thisTile == Tile.TileType.Empty)
                    {
                        player.Move(Character.Movement.Left);
                        EngineMap.UpdateVision();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            private void btnUp_Click(object sender, EventArgs e)
            {
                MovePlayer(Character.Movement.Up);
            }
            private void btnleft_Click(object sender, EventArgs e)
            {
                MovePlayer(Character.Movement.Left);
            }
            private void btnRight_Click(object sender, EventArgs e)
            {
                MovePlayer(Character.Movement.Right);
            }
            private void btnDown_Click(object sender, EventArgs e)
            {
                MovePlayer(Character.Movement.Down);
            }
        }
       
        public Form1()
        {
            InitializeComponent();
            gameengine = new GameEngine();
        }       
    }
}
