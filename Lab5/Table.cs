using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public class Table
    {
        public Table() { }
        public int countField;
        public Panel PanelOut { get; set; }
        public Panel PanelIn { get; set; }
        public Label Label { get; set; }
        public TextBox TextBox { get; set; }
        public List<Field> Field = new List<Field>();
    }

    public class Field
    {
        public Field() { }
        public Panel Panel { get; set; }
        public Label Label { get; set; }
        public Button Button { get; set; }
        public TextBox TextBox { get; set; }
        public ComboBox ComboBox { get; set; }
    }

    public class Arrow
    {
        public Arrow() { }
        public string NameClassFirst { get; set; }
        public string NameClassSecond { get; set; }
        public string NameFirst { get; set; }
        public string NameSecond { get; set; }
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
    }

}
