using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sokoban_2
{
   
    public enum Square_type
    {
        empty, border, box, person, place_for_box
    }

    public enum Direction
    {
        right, left, up, down
    }


    public partial class Form1 : Form
    { 
        int current_level = 0;
        Desk field = new Desk(0);
        public Form1()
        {
          InitializeComponent();
        //  field = new Desk(0);
          label1.Text = "Current level : " + current_level;
          this.Focus();
          Invalidate();
            this.KeyPreview = true;
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            field.Draw(e.Graphics);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
           Direction direct = new Direction();
            if (e.KeyCode == Keys.Right)
                direct = Direction.right;
            else if (e.KeyCode == Keys.Left)
                direct = Direction.left;
            else if (e.KeyCode == Keys.Up)
                direct = Direction.up;
            else if (e.KeyCode == Keys.Down)
                direct = Direction.down;
            if (field.Move_person(direct))
            {
                Invalidate(field.Get_person_location());
                if (field.list_places.All(x => x.Value == true))
                {
                    Invalidate();
                }
            }
            Console.WriteLine("----------");
            foreach (var x in field.list_places)
            {
                Console.WriteLine( x.Key.X + " " + x.Key.Y + " " + x.Value + "/n");
            }
            
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            var result = form2.DialogResult;
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                int level = form2.getName();
                field = new Desk(level);
                label1.Text = "Current level : " + level;
                this.Focus();
                Invalidate();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
 //           e.Handled = true;
        }


    }



    public class Desk
    {
       
        const int DESK_SIZE = 20;
        const int SQUARE_SIZE = 20;
        Point person = new Point();
        public Dictionary<Point, bool> list_places = new Dictionary<Point, bool>();
        
     
        public Desk(int lev)
        {
            Set_desk(lev);            
        }

        public Square[,] desk = new Square[DESK_SIZE, DESK_SIZE];
        
        public string Load_Level(int lev)
        {
            string[] lines = System.IO.File.ReadAllLines("AllLevels.txt");
            
            int amount_lev = int.Parse(lines[0]);
            for (int i = 1; i < amount_lev * 2 ; i+=2) 
            {
                if ( int.Parse(lines[i]) == lev)
                {
                    return lines[i + 1];
                }
            }
            return lines[3];
        }

        
        public void Set_desk(int lev)
        {
            
            string level = Load_Level(lev);    
            string[] lines = System.IO.File.ReadAllLines(level);

            for (int j = 0; j < DESK_SIZE; j++)
            {
                string[] line = lines[j].Split();
                for (int i = 0; i < DESK_SIZE; i++)
                {
                    int temp = int.Parse(line[i]);

                    if (temp == 0)
                    {
                        desk[i, j] = new Square();
                        desk[i, j].type = Square_type.empty;
                    }
                    else if (temp == 1)
                    {
                        desk[i, j] = new Square();
                        desk[i, j].type = Square_type.border;
                    }
                    else if (temp == 2)
                    {
                        desk[i, j] = new Square();
                        desk[i, j].type = Square_type.box;
                    }
                    else if (temp == 3)
                    {
                        desk[i, j] = new Square();
                        person.X = i;
                        person.Y = j;
                        desk[i, j].type = Square_type.person;
                    }
                    else if (temp == 4)
                    {
                        desk[i, j] = new Square();
                        desk[i, j].type = Square_type.place_for_box;
                        Point p = new Point(i, j);
                            list_places.Add(p, false);
                    }
                    desk[i, j].x = i;
                    desk[i, j].y = j;
                }
            }
            
        }




        public void Draw(Graphics g )
        {
            
            g.DrawRectangle(Pens.Black, 0, 0, DESK_SIZE * SQUARE_SIZE, DESK_SIZE * SQUARE_SIZE);
            for (int i = 0; i < DESK_SIZE; i++)
            {
                for (int j = 0; j < DESK_SIZE; j++)
                {
                    if (desk[i, j].type == Square_type.empty)
                    {
                        g.FillRectangle(SystemBrushes.Control, i * DESK_SIZE, j * DESK_SIZE, SQUARE_SIZE, SQUARE_SIZE);
                        g.DrawRectangle(Pens.Blue, i * DESK_SIZE, j * DESK_SIZE, SQUARE_SIZE, SQUARE_SIZE);
                    }
                    else if (desk[i, j].type == Square_type.border)
                    {
                        g.FillRectangle(Brushes.DarkGray, i * DESK_SIZE, j * DESK_SIZE, SQUARE_SIZE, SQUARE_SIZE);
                    }
                    else if (desk[i, j].type == Square_type.place_for_box)
                    {
                        g.DrawRectangle(Pens.Blue, i * DESK_SIZE, j * DESK_SIZE, SQUARE_SIZE, SQUARE_SIZE);
                        g.FillEllipse(Brushes.CadetBlue, i * DESK_SIZE, j * DESK_SIZE, 20, 20);
                    }
                    else if (desk[i, j].type == Square_type.box)
                    {
                        g.FillRectangle(Brushes.Gray, i * DESK_SIZE, j * DESK_SIZE, SQUARE_SIZE, SQUARE_SIZE);
                        g.DrawRectangle(Pens.DarkBlue, i * DESK_SIZE, j * DESK_SIZE, SQUARE_SIZE, SQUARE_SIZE);
                        g.DrawLine(Pens.DarkBlue, i * DESK_SIZE, j * DESK_SIZE, (i + 1) * DESK_SIZE, (j + 1) * DESK_SIZE);
                        g.DrawLine(Pens.DarkBlue, (i + 1) * DESK_SIZE, j * DESK_SIZE, i * DESK_SIZE, (j + 1) * DESK_SIZE);
                    }
                    else if (desk[i, j].type == Square_type.person)
                    {
                        g.DrawRectangle(Pens.Blue, i * DESK_SIZE, j * DESK_SIZE, SQUARE_SIZE, SQUARE_SIZE);
                        g.DrawEllipse(Pens.Black, i * DESK_SIZE + 7, j * DESK_SIZE + 2, 7, 7);
                        g.DrawLine(Pens.Black, i * DESK_SIZE + 10, j * DESK_SIZE + 17, i * DESK_SIZE + 10, j * DESK_SIZE + 9);  //head
                        g.DrawLine(Pens.Black, i * DESK_SIZE + 10, j * DESK_SIZE + 12, i * DESK_SIZE + 7, j * DESK_SIZE + 14);  //left_arm
                        g.DrawLine(Pens.Black, i * DESK_SIZE + 10, j * DESK_SIZE + 12, i * DESK_SIZE + 13, j * DESK_SIZE + 14);  //right_arm
                        g.DrawLine(Pens.Black, i * DESK_SIZE + 10, j * DESK_SIZE + 17, i * DESK_SIZE + 7, j * DESK_SIZE + 19);   //left_legs
                        g.DrawLine(Pens.Black, i * DESK_SIZE + 10, j * DESK_SIZE + 17, i * DESK_SIZE + 13, j * DESK_SIZE + 19);  //right_leg
                    }
                }
            }
            if (list_places.All(x => x.Value == true))
            {
                Font myFont = new Font("Arial", 32);
                g.DrawString(" YOU WON :)", myFont, Brushes.Black, new Point(100, 100) );
            }
        }




        public void Per_Movement(Direction direct)             //Moves Person on the next square in the sertain direction
        {
             if (direct == Direction.down)
                {
                    desk[person.X, person.Y + 1].type = Square_type.person;
                    desk[person.X, person.Y].type = Square_type.empty;
                    person.Y++;
                }
                else if (direct == Direction.up)
                {

                    desk[person.X, person.Y - 1].type = Square_type.person;
                    desk[person.X, person.Y].type = Square_type.empty;
                    person.Y--;
                }
                else if (direct == Direction.left)
                {

                    desk[person.X - 1, person.Y].type = Square_type.person;
                    desk[person.X, person.Y].type = Square_type.empty;
                    person.X--;
                }
                else if (direct == Direction.right)
                {

                    desk[person.X + 1, person.Y].type = Square_type.person;
                    desk[person.X, person.Y].type = Square_type.empty;
                    person.X++;
                }
        }



        public void Restore_place_for_box()
        {
            bool need_restore = false;
            Point temper_point = new Point();
            foreach (var x in list_places)
            {
                if (desk[x.Key.X, x.Key.Y].type == Square_type.empty)
                {
                    desk[x.Key.X, x.Key.Y].type = Square_type.place_for_box;
                    Point p = new Point(x.Key.X, x.Key.Y);
                     temper_point = p;
                     need_restore = true;
                }
             }
            if (need_restore)
                list_places[temper_point] = false;
                
        }
        public bool Move_Box(Direction direct, Point present_p)
        {
            Point temp_point = new Point();   // Point to check to move to
            if (direct == Direction.down)
            {
                temp_point.X = present_p.X;
                temp_point.Y = present_p.Y + 1;
            }
            else if (direct == Direction.up)
            {
                temp_point.X = present_p.X;
                temp_point.Y = present_p.Y - 1;
            }
            else if (direct == Direction.right)
            {
                temp_point.X = present_p.X + 1;
                temp_point.Y = present_p.Y;
            }
            else if (direct == Direction.left)
            {
                temp_point.X = present_p.X - 1;
                temp_point.Y = present_p.Y;
            }
            Square_type type_return = Check_Movement(temp_point);
            if ((type_return == Square_type.border) || (type_return == Square_type.box))
                return false;
            else if (type_return == Square_type.empty)
            {
                desk[temp_point.X, temp_point.Y].type = Square_type.box;
                desk[present_p.X, present_p.Y].type = Square_type.empty;
                return true;
            }
            else if (type_return == Square_type.place_for_box)
            {
                list_places[temp_point] = true;
                desk[present_p.X, present_p.Y].type = Square_type.empty;
                desk[temp_point.X, temp_point.Y].type = Square_type.box;
                if (list_places.ContainsKey(present_p))
                list_places[present_p] = false;
                return true;
            }
            return false;

        }



        public Square_type Check_Movement( Point temp)                // Checks the type of square, where we want to move to
        {
            return desk[temp.X, temp.Y].type;
        }


        public Rectangle Get_person_location()
        {
            int t_x = person.X - 1, t_y = person.Y - 1, t_w = 3, t_h = 3;
            if (person.X == 1)
            {
                t_x = person.X;
                t_w = 2;
            }
            else if (person.X == DESK_SIZE - 2)
            {
                t_w = 2;
            }

            if (person.Y == 1)
            {
                t_y = person.Y;
                t_h = 2;
            }
            else if (person.Y == DESK_SIZE - 2)
            {
                t_h = 2;
            }

            Point cent = new Point(t_x * SQUARE_SIZE, t_y * SQUARE_SIZE);
            Rectangle rect = new Rectangle(cent.X, cent.Y, SQUARE_SIZE * t_w, SQUARE_SIZE * t_h);
            return rect;
        }

        public bool Move_person(Direction direct)                          //Moves Person with smth or without
        {
            bool is_moved = false;
            Point temp_point = new Point();  // where we want to move to
            if (direct == Direction.down)
            {
                temp_point.X = person.X;
                temp_point.Y = person.Y + 1;
            }
            else if (direct == Direction.up)
            {
                temp_point.X = person.X;
                temp_point.Y = person.Y - 1;
            }
            else if (direct == Direction.right)
            {
                temp_point.X = person.X + 1;
                temp_point.Y = person.Y;
            }
            else if (direct == Direction.left)
            {
                temp_point.X = person.X - 1;
                temp_point.Y = person.Y;
            }
        
            Square_type type_return = Check_Movement( temp_point);
            if (type_return == Square_type.empty)
            {
                Per_Movement(direct);
                is_moved = true;
            }
            else if (type_return == Square_type.border) { }
            else if (type_return == Square_type.box)
            {
                if (Move_Box(direct, temp_point))
                {
                    Per_Movement(direct);
                    is_moved = true;
                }

            }
            else if (type_return == Square_type.place_for_box)
            {
                desk[person.X, person.Y].type = Square_type.empty;
                desk[temp_point.X, temp_point.Y].type = Square_type.person;
                person.X = temp_point.X;
                person.Y = temp_point.Y;
                is_moved = true;
            }
            Restore_place_for_box();
            return is_moved;
        }
    }



    public class Square
    {
        public int x, y;
        public Square_type type;
    }

}
