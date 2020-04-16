using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Form1 : Form
    {
        string activeTextBox = "";
        private Point p1, p2;
        public Graphics gr;
        Bitmap bitmap;

        int count;
        string nameFirst;
        string nameClassFirst;
        bool flag = false;

        MoveObject moveObject;
        public List<Table> Tables;
        public List<Arrow> Arrows;
        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Tables = new List<Table>();
            Arrows = new List<Arrow>();
            moveObject = new MoveObject(this);
            this.KeyPreview = true;
            this.KeyUp += new KeyEventHandler(this.Form1_KeyUp);

            ToolStripMenuItem insertMenuItem = new ToolStripMenuItem("Добавить поле");
            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить таблицу");

            contextMenuStrip1.Items.AddRange(new[] { insertMenuItem, deleteMenuItem });

            insertMenuItem.Click += buttonPlus_Click;
            deleteMenuItem.Click += RemoveTable_Click;
        }
        private void AddTable()
        {
            Table table = Controller.CreateTable();

            table.Label.MouseDown += new MouseEventHandler(MoveObject.mDown);
            table.Label.MouseUp += new MouseEventHandler(MoveObject.mUp);
            table.Label.MouseMove += new MouseEventHandler(MoveObject.mMove);

            table.Label.MouseDoubleClick += DoubleClickLable;

            table.PanelIn.MouseClick += Panel_MouseClick;
            table.PanelOut.MouseClick += Panel_MouseClick;

            pictureBox1.Controls.Add(table.Label);
            pictureBox1.Controls.Add(table.TextBox);
            pictureBox1.Controls.Add(table.PanelOut);
            pictureBox1.Controls.Add(table.PanelIn);

            table.Label.ContextMenuStrip = contextMenuStrip1;

            table.TextBox.Visible = false;

            AddField(table, "Id");
            Tables.Add(table);
        }
        private void AddField(Table table)
        {
            Field field = Controller.CreateField(table);

            for (int i = 0; i < Arrows.Count; i++)
            {
                if (table.PanelIn.Name == Arrows[i].NameSecond)
                {
                    Arrows[i].Point2 = new Point(Arrows[i].Point2.X, Arrows[i].Point2.Y + 30);
                }
            }

            RePrintLine();

            field.Button.Click += buttonMinus_Click;

            field.Label.MouseDoubleClick += DoubleClickLable;

            pictureBox1.Controls.Add(field.Button);
            pictureBox1.Controls.Add(field.ComboBox);
            pictureBox1.Controls.Add(field.Label);
            pictureBox1.Controls.Add(field.TextBox);
            pictureBox1.Controls.Add(field.Panel);

            field.TextBox.Visible = false;

            table.countField++;
            table.Field.Add(field);
        }
        private void AddField(Table table, string text)
        {
            Field field = Controller.CreateField(table, text);

            for (int i = 0; i < Arrows.Count; i++)
            {
                if (table.PanelIn.Name == Arrows[i].NameSecond)
                {
                    Arrows[i].Point2 = new Point(Arrows[i].Point2.X, Arrows[i].Point2.Y + 30);
                }
            }
            RePrintLine();

            field.Button.Click += buttonMinus_Click;

            if (text == "Id")
                field.Button.Visible = false;

            pictureBox1.Controls.Add(field.Button);
            pictureBox1.Controls.Add(field.ComboBox);
            pictureBox1.Controls.Add(field.Label);
            pictureBox1.Controls.Add(field.TextBox);
            pictureBox1.Controls.Add(field.Panel);

            field.TextBox.Visible = false;

            table.countField++;
            table.Field.Add(field);
        }
        private void RemoveField(Table table, Field field)
        {
            pictureBox1.Controls.Remove(field.Button);
            pictureBox1.Controls.Remove(field.Label);
            pictureBox1.Controls.Remove(field.TextBox);
            pictureBox1.Controls.Remove(field.ComboBox);
            pictureBox1.Controls.Remove(field.Panel);

            int place = -1;
            for (int i = 0; i < table.Field.Count; i++)
            {
                if (table.Field[i].Button.Name == field.Button.Name)
                {
                    place = i;
                }
            }

            for (int i = place; i < table.Field.Count; i++)
            {
                table.Field[i].Panel.Top -= 30;
                table.Field[i].Button.Top -= 30;
                table.Field[i].Label.Top -= 30;
                table.Field[i].TextBox.Top -= 30;
                table.Field[i].ComboBox.Top -= 30;

            }
            table.PanelIn.Top -= 30;
            table.Field.Remove(field);

            for (int i = 0; i < Arrows.Count; i++)
            {
                if (table.PanelIn.Name == Arrows[i].NameSecond)
                {
                    Arrows[i].Point2 = new Point(Arrows[i].Point2.X, Arrows[i].Point2.Y - 30);
                }

            }
            RePrintLine();
        }
        private void RemoveTable(Table table)
        {
            List<string> NameArrow = new List<string>();

            for (int i = 0; i < Tables.Count; i++)
            {
                for (int j = 0; j < Arrows.Count; j++)
                {
                    if (Arrows[j].NameSecond == Tables[i].PanelIn.Name || Arrows[j].NameFirst == Tables[i].PanelOut.Name)
                    {
                        for (int k = 0; k < Tables[i].Field.Count; k++)
                        {
                            if (Tables[i].Field[k].Label.Text == table.Label.Text + "Id")
                            {
                                RemoveField(Tables[i], Tables[i].Field[k]);
                                k--;
                            }
                        }
                    }
                }
            }

            if (Arrows.Count != 0)
            {
                for (int j = 0; j < Arrows.Count; j++)
                {
                    if (Arrows[j].NameSecond == table.PanelIn.Name || Arrows[j].NameFirst == table.PanelOut.Name)
                    {
                        Arrows.RemoveAt(j);
                        j--;
                    }
                }
            }

            pictureBox1.Controls.Remove(table.PanelIn);
            pictureBox1.Controls.Remove(table.PanelOut);
            pictureBox1.Controls.Remove(table.Label);
            pictureBox1.Controls.Remove(table.TextBox);

            for (int i = 0; i < table.Field.Count; i++)
            {
                pictureBox1.Controls.Remove(table.Field[i].Button);
                pictureBox1.Controls.Remove(table.Field[i].Label);
                pictureBox1.Controls.Remove(table.Field[i].TextBox);
                pictureBox1.Controls.Remove(table.Field[i].ComboBox);
                pictureBox1.Controls.Remove(table.Field[i].Panel);
            }

            Tables.Remove(table);
            RePrintLine();
        }
        private void RemoveTable_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("111");
            ToolStripMenuItem myItem = sender as ToolStripMenuItem;
            if (myItem != null)
            {
                ContextMenuStrip theStrip = myItem.Owner as ContextMenuStrip;
                if (theStrip != null)
                {
                    Label lab = theStrip.SourceControl as Label;
                    for (int i = 0; i < Tables.Count; i++)
                    {
                        if (Tables[i].Label.Name == lab.Name)
                        {
                            //MessageBox.Show("!!!!!!!");
                            RemoveTable(Tables[i]);
                        }
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            nameFirst = "";
            count = 0;
            flag = false;
            AddTable();
            BackLable();
            button1.Visible = false;
            button1.Visible = true;
        }
        public void DoubleClickLable(object sender, EventArgs e)
        {
            nameFirst = "";
            count = 0;
            flag = false;
            BackLable();
            if (activeTextBox == "")
            {
                Label lb = (Label)sender;
                foreach (Table l in Tables)
                {
                    if (l.Label.Name == lb.Name)
                    {
                        activeTextBox = l.TextBox.Name;
                        l.Label.Visible = false;
                        l.TextBox.Visible = true;
                        l.TextBox.Text = l.Label.Text;
                    }
                    foreach (Field f in l.Field)
                    {
                        if (f.Label.Name == lb.Name)
                        {
                            activeTextBox = f.TextBox.Name;
                            f.Label.Visible = false;
                            f.TextBox.Visible = true;
                        }
                    }
                }
            }
            else
            {
                BackLable();
            }

        }
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            nameFirst = "";
            count = 0;
            flag = false;
            BackLable();
            ToolStripMenuItem myItem = sender as ToolStripMenuItem;
            if (myItem != null)
            {
                ContextMenuStrip theStrip = myItem.Owner as ContextMenuStrip;
                if (theStrip != null)
                {
                    Label lab = theStrip.SourceControl as Label;
                    foreach (Table l in Tables)
                    {
                        if (l.Label.Name == lab.Name)
                        {
                            AddField(l);
                        }
                    }
                }
            }
            button1.Visible = false;
            button1.Visible = true;
        }
        private void buttonMinus_Click(object sender, EventArgs e)
        {
            nameFirst = "";
            count = 0;
            flag = false;
            BackLable();
            Button b = (Button)sender;
            foreach (Table l in Tables)
            {
                for (int i = 0; i < l.Field.Count; i++)
                {
                    if (l.Field[i].Button.Name == b.Name)
                    {
                        for (int j = 0; j < Arrows.Count; j++)
                        {

                            if (l.Field[i].Label.Text == Arrows[j].NameClassFirst + "Id")
                            {
                                if (Arrows[j].NameSecond == l.PanelIn.Name)
                                    Arrows.RemoveAt(j);
                            }
                        }
                        RemoveField(l, l.Field[i]);
                    }
                }
            }
            button1.Visible = false;
            button1.Visible = true;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            nameFirst = "";
            count = 0;
            flag = false;
            if (e.KeyCode == Keys.Enter)
            {
                BackLable();
            }
        }
        private bool CheckTables(string newClass)
        {
            foreach (Table l in Tables)
            {
                if (l.Label.Text == newClass)
                    return false;                
            }           
            return true;
        }
        private bool CheckTablesIsDigit(string newClass)
        {
            
            for (int i = 0; i < newClass.Length; i++)
            {           
                if (!Char.IsDigit(newClass[i]))
                {
                    return false;
                }
            }
            return true;
        }
        private void BackLable()
        {
            nameFirst = "";
            count = 0;
            flag = false;
            string oldText = "", newText = "";
            foreach (Table l in Tables)
            {
                if (l.TextBox.Name == activeTextBox)
                {
                    oldText = l.Label.Text;
                    newText = l.TextBox.Text;
                    if(CheckTablesIsDigit(newText) || !CheckTables(newText))
                    {
                        l.Label.Text = oldText;
                        l.Label.Visible = true;
                        l.TextBox.Visible = false;
                        activeTextBox = "";
                        return;
                    }
                    else
                    {
                        l.Label.Text = l.TextBox.Text;
                        l.Label.Visible = true;
                        l.TextBox.Visible = false;
                        activeTextBox = "";
                    }

                }
                foreach (Field f in l.Field)
                {
                    if (f.TextBox.Name == activeTextBox)
                    {
                        f.Label.Text = f.TextBox.Text;
                        f.Label.Visible = true;
                        f.TextBox.Visible = false;

                        activeTextBox = "";
                    }
                }
            }

            foreach (Table l in Tables)
            {
                foreach (Field f in l.Field)
                {
                    if (f.Label.Text == oldText + "Id")
                    {
                        f.Label.Text = newText + "Id";
                    }
                }
            }
            foreach (Arrow l in Arrows)
            {
                if (l.NameClassFirst == oldText)
                {
                    l.NameClassFirst = newText;
                }
                if (l.NameClassSecond == oldText)
                {
                    MessageBox.Show(oldText);
                    l.NameClassSecond = newText;
                    MessageBox.Show(newText);
                }
            }
        }

        public void lb_Click(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            foreach (Table l in Tables)
            {
                if (l.Label.Name == lb.Name)
                    MessageBox.Show(l.PanelOut.Name + " " + l.Label.Name);
            }
        }
        private bool CheckArrow(Arrow ar)
        {
            if (ar.NameClassFirst == ar.NameClassSecond)
                return false;
            foreach (Arrow l in Arrows)
            {
                if (l.NameClassFirst == ar.NameClassFirst && l.NameClassSecond == ar.NameClassSecond)
                    return false;
                if (l.NameClassFirst == ar.NameClassSecond && l.NameClassSecond == ar.NameClassFirst)
                    return false;
            }
            return true;
        }
        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (flag == true)
            {
                Panel panel = (Panel)sender;
                Arrow ar = new Arrow();
                if (count == 0)
                {
                    foreach (Table l in Tables)
                    {
                        if (l.PanelOut.Name == panel.Name)
                        {
                            p1.X = l.PanelOut.Left + 15;
                            p1.Y = l.PanelOut.Top - 2;
                            nameFirst = l.PanelOut.Name;
                            nameClassFirst = l.Label.Text;
                            count++;
                        }
                    }
                }

                if (count == 1)
                {
                    foreach (Table l in Tables)
                    {
                        if (l.PanelIn.Name == panel.Name)
                        {

                            p2.X = l.PanelIn.Left + 100;
                            p2.Y = l.PanelIn.Top + 32;
                            ar.NameFirst = nameFirst;
                            ar.NameClassFirst = nameClassFirst;
                            ar.NameClassSecond = l.Label.Text;
                            ar.NameSecond = l.PanelIn.Name;
                            ar.Point1 = new Point(p1.X, p1.Y);
                            ar.Point2 = new Point(p2.X, p2.Y);
                            if (CheckArrow(ar))
                            {
                                PrintLine(p1, p2);
                                nameFirst = "";
                                count = 0;
                                flag = false;
                                Arrows.Add(ar);
                                AddField(l, nameClassFirst + "Id");
                            }
                            else
                            {
                                nameFirst = "";
                                count = 0;
                                flag = false;
                                return;
                            }
                        }
                    }
                }
            }
        }
        public void PrintLine(Point p1, Point p2)
        {
            gr = Graphics.FromImage(bitmap);
            gr.Clear(Color.WhiteSmoke);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Black, 2);
            pen.StartCap = LineCap.RoundAnchor;
            pen.CustomEndCap = new AdjustableArrowCap(4, 4);
            gr.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
            pictureBox1.Image = bitmap;
            gr.Dispose();
        }
        public void RePrintLine()
        {
            pictureBox1.Image = null;
            gr = Graphics.FromImage(bitmap);
            gr.Clear(Color.WhiteSmoke);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Black, 2);
            pen.StartCap = LineCap.RoundAnchor;
            pen.CustomEndCap = new AdjustableArrowCap(4, 4);
            for (int i = 0; i < Arrows.Count; i++)
            {
                gr.DrawLine(pen, Arrows[i].Point1.X, Arrows[i].Point1.Y, Arrows[i].Point2.X, Arrows[i].Point2.Y);
            }
            pictureBox1.Image = bitmap;
            gr.Dispose();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            flag = true;
        }
        private bool CheckExistTable()
        {
            if (Tables.Count == 0)
                return false;
            foreach (Table t in Tables)
            {
                if (t.Field.Count < 2)
                {
                    return false;
                }
            }
            return true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            nameFirst = "";
            count = 0;
            flag = false;
            if(CheckExistTable() == true)
            {
                Generate gen = new Generate(Tables);
                gen.Test();
            }     
            else
            {
                MessageBox.Show("Имеются пустые таблицы, либо удалите, либо заполните их.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override CreateParams CreateParams
        {
            get
            {
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
