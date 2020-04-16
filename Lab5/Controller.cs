using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    class Controller
    {
        static int countTable = 0;
        public Controller() { }

        public static Table CreateTable()
        {
            Random rand = new Random();

            Table table = new Table();
            table.PanelIn = new Panel();
            table.Label = new Label();
            table.TextBox = new TextBox();
            table.PanelOut = new Panel();

            table.PanelOut.BackColor = Color.LightGray;
            table.Label.BackColor = SystemColors.ActiveBorder;
            table.TextBox.BackColor = SystemColors.ActiveBorder;
            table.PanelIn.BackColor = SystemColors.ActiveBorder;
            countTable += 1;

            table.PanelOut.Name = "PanelOut" + countTable;
            table.Label.Name = "ClassTable" + countTable;
            table.TextBox.Name = "TextBox" + countTable;
            table.PanelIn.Name = "PanelIn" + countTable;

            table.Label.Text = "Class" + countTable;
            table.Label.Cursor = Cursors.Hand;
            table.TextBox.Text = table.Label.Text;

            table.PanelOut.Size = new Size(200, 30);
            table.Label.Size = new Size(170, 30);
            table.TextBox.Size = new Size(170, 30);
            table.PanelIn.Size = new Size(200, 30);

            table.Label.Font = new Font(table.Label.Font.FontFamily, 10);
            table.Label.TextAlign = ContentAlignment.MiddleLeft;
            table.TextBox.Font = new Font(table.TextBox.Font.FontFamily, 10);

            table.PanelOut.Left = rand.Next(155, 700);
            table.PanelOut.Top = rand.Next(10, 450);

            table.Label.Left = table.PanelOut.Left + 30;
            table.Label.Top = table.PanelOut.Top;

            table.TextBox.Left = table.PanelOut.Left + 30;
            table.TextBox.Top = table.PanelOut.Top;

            table.PanelIn.Left = table.PanelOut.Left;
            table.PanelIn.Top = table.PanelOut.Top + 30;

            return table;      
        }

        public static Field CreateField(Table table)
        {
            Field field = new Field();
            field.Panel = new Panel();
            field.Button = new Button();
            field.Label = new Label();
            field.TextBox = new TextBox();
            field.ComboBox = new ComboBox();

            field.Panel.BackColor = Color.LightGray;
            field.Button.BackColor = Color.LightGray;
            field.Label.BackColor = Color.LightGray;
            field.TextBox.BackColor = Color.LightGray;
            field.ComboBox.BackColor = Color.LightGray;

            field.Panel.Name = "PanelField" + (table.countField + 1) + table.Label.Text;
            field.Button.Name = "Remove" + (table.countField + 1) + table.Label.Text;
            field.Button.Text = "-";
            field.Label.Name = "Field" + (table.countField + 1) + table.Label.Text;
            field.Label.Text = "Field" + (table.countField + 1);
            field.TextBox.Name = "TextBoxField" + (table.countField + 1) + table.Label.Text;
            field.TextBox.Text = field.Label.Text;
            field.ComboBox.Name = "ComboBoxField" + (table.countField + 1);
            field.ComboBox.Text = "string";
            InitCmbBox(field);

            field.Label.Font = new Font(field.Label.Font.FontFamily, 9);
            field.Button.TextAlign = ContentAlignment.MiddleLeft;
            field.TextBox.Font = new Font(field.TextBox.Font.FontFamily, 8);

            field.Button.Font = new Font(field.Button.Font.Name, 10, FontStyle.Bold);
            field.Button.UseCompatibleTextRendering = true;
            field.Button.FlatStyle = FlatStyle.Standard;
            field.Button.FlatAppearance.BorderColor = Color.Gray;
            field.Button.TextAlign = ContentAlignment.MiddleCenter;

            field.ComboBox.Font = new Font(field.ComboBox.Font.Name, 9);
            field.ComboBox.FlatStyle = FlatStyle.Flat;

            field.Panel.Size = new Size(200, 30);
            field.Label.Size = new Size(110, 30);
            field.Button.Size = new Size(30, 30);
            field.TextBox.Size = new Size(110, 30);
            field.ComboBox.Size = new Size(60, 30);

            field.Panel.Left = table.PanelOut.Left;
            if (table.Field.Count == 0)
            {
                field.Panel.Top = table.PanelOut.Top + 30;
            }
            else
            {
                field.Panel.Top = table.Field[table.Field.Count - 1].Panel.Top + 30;
            }
            field.Label.Left = field.Panel.Left + 60;
            field.Label.Top = field.Panel.Top;

            field.Button.Left = field.Panel.Left + 170;
            field.Button.Top = field.Panel.Top;

            field.TextBox.Left = field.Panel.Left + 60;
            field.TextBox.Top = field.Panel.Top;

            field.ComboBox.Left = field.Panel.Left;
            field.ComboBox.Top = field.Panel.Top;

            table.PanelIn.Top += 30;


            return field;
        }

        public static Field CreateField(Table table, string text)
        {
            Field field = new Field();
            field.Panel = new Panel();
            field.Button = new Button();
            field.Label = new Label();
            field.TextBox = new TextBox();
            field.ComboBox = new ComboBox();

            field.Panel.BackColor = Color.LightGray;
            field.Button.BackColor = Color.LightGray;
            field.Label.BackColor = Color.LightGray;
            field.TextBox.BackColor = Color.LightGray;
            field.ComboBox.BackColor = Color.LightGray;

            field.Panel.Name = "PanelField" + (table.countField + 1);
            field.Button.Name = "Remove" + (table.countField + 1);
            field.Button.Text = "-";
            field.Label.Name = "Field" + (table.countField + 1);
            field.Label.Text = text;
            field.TextBox.Name = "TextBoxField" + (table.countField + 1);
            field.TextBox.Text = field.Label.Name;
            field.ComboBox.Name = "ComboBoxField" + (table.countField + 1);
            field.ComboBox.Text = "int";
            field.ComboBox.Items.Add("int");

            field.Label.Font = new Font(field.Label.Font.FontFamily, 9);
            field.Button.TextAlign = ContentAlignment.MiddleLeft;
            field.TextBox.Font = new Font(field.TextBox.Font.FontFamily, 8);

            field.Button.Font = new Font(field.Button.Font.Name, 10, FontStyle.Bold);
            field.Button.UseCompatibleTextRendering = true;
            field.Button.FlatStyle = FlatStyle.Standard;
            field.Button.FlatAppearance.BorderColor = Color.Gray;
            field.Button.TextAlign = ContentAlignment.MiddleCenter;

            field.ComboBox.Font = new Font(field.ComboBox.Font.Name, 9);
            field.ComboBox.FlatStyle = FlatStyle.Flat;

            field.Panel.Size = new Size(200, 30);
            field.Label.Size = new Size(110, 30);
            field.Button.Size = new Size(30, 30);
            field.TextBox.Size = new Size(110, 30);
            field.ComboBox.Size = new Size(60, 30);

            field.Panel.Left = table.PanelOut.Left;
            if (table.Field.Count == 0)
            {
                field.Panel.Top = table.PanelOut.Top + 30;
            }
            else
            {
                field.Panel.Top = table.Field[table.Field.Count - 1].Panel.Top + 30;
            }
            field.Label.Left = field.Panel.Left + 60;
            field.Label.Top = field.Panel.Top;

            field.Button.Left = field.Panel.Left + 170;
            field.Button.Top = field.Panel.Top;

            field.TextBox.Left = field.Panel.Left + 60;
            field.TextBox.Top = field.Panel.Top;

            field.ComboBox.Left = field.Panel.Left;
            field.ComboBox.Top = field.Panel.Top;

            table.PanelIn.Top += 30;

            return field;
        }
        private static void InitCmbBox(Field fld)
        {
            fld.ComboBox.Items.Add("int");
            fld.ComboBox.Items.Add("string");
        }
    }
}
