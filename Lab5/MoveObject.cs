using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public class MoveObject
    {
        static bool isPress = false;
        static Point startPst;
        static Form1 form1;
        public MoveObject(Form1 frm)
        {
            form1 = frm;
        }

        /// <summary>
        /// Функция выполняется при нажатии на перемещаемый контрол
        /// </summary>
        /// <param name="sender">контролл</param>
        /// <param name="e">событие мышки</param>
        public static void mDown(object sender, MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Right) return;//проверка что нажата левая кнопка
            isPress = true;
            startPst = e.Location;
        }

        /// <summary>
        /// Функция выполняется при отжатии перемещаемого контрола
        /// </summary>
        /// <param name="sender">контролл</param>
        /// <param name="e">событие мышки</param>
        public static void mUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return;//проверка что нажата левая кнопка
            isPress = false;
            
        }

        /// <summary>
        /// Функция выполняется при перемещении контрола
        /// </summary>
        /// <param name="sender">контролл</param>
        /// <param name="e">событие мышки</param>
        public static void mMove(object sender, MouseEventArgs e)
        {
            if (isPress)
            {
                foreach (Table t in form1.Tables)
                {
                    if (sender is Label)
                    {
                        if (t.Label == (Label)sender)
                        {
                            Task.Delay(1).Wait();

                            t.PanelOut.Top += e.Y - startPst.Y;
                            t.PanelOut.Left += e.X - startPst.X;
                         
                            t.PanelIn.Top += e.Y - startPst.Y;
                            t.PanelIn.Left += e.X - startPst.X;
 
                            t.Label.Top += e.Y - startPst.Y;
                            t.Label.Left += e.X - startPst.X;
    
                            t.TextBox.Top += e.Y - startPst.Y;
                            t.TextBox.Left += e.X - startPst.X;

                            for (int i = 0; i < form1.Arrows.Count; i++)
                            {
                                if (form1.Arrows[i].NameFirst == t.PanelOut.Name)
                                {
                                    int newX = form1.Arrows[i].Point1.X;
                                    int newY = form1.Arrows[i].Point1.Y;

                                    newY += e.Y - startPst.Y;
                                    newX += e.X - startPst.X;

                                    form1.Arrows[i].Point1 = new Point(newX, newY);
                                }
                                if (form1.Arrows[i].NameSecond == t.PanelIn.Name)
                                {
                                    int newX = form1.Arrows[i].Point2.X;
                                    int newY = form1.Arrows[i].Point2.Y;

                                    newY += e.Y - startPst.Y;
                                    newX += e.X - startPst.X;

                                    form1.Arrows[i].Point2 = new Point(newX, newY);
                                }
                            }

                            for (int i = 0; i < t.Field.Count; i++)
                            {                    
                                t.Field[i].Panel.Top += e.Y - startPst.Y;
                                t.Field[i].Panel.Left += e.X - startPst.X;
                                
                                t.Field[i].Button.Top += e.Y - startPst.Y;
                                t.Field[i].Button.Left += e.X - startPst.X;
                                
                                t.Field[i].Label.Top += e.Y - startPst.Y;
                                t.Field[i].Label.Left += e.X - startPst.X;
                                
                                t.Field[i].TextBox.Top += e.Y - startPst.Y;
                                t.Field[i].TextBox.Left += e.X - startPst.X;
                                
                                t.Field[i].ComboBox.Top += e.Y - startPst.Y;
                                t.Field[i].ComboBox.Left += e.X - startPst.X;
                               
                            }                          
                        }
                    }
                }

                form1.RePrintLine();
            }
            
        }

    }
}
