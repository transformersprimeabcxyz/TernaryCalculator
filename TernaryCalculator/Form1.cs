using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TernaryCalculator.Framework;

namespace TernaryCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;

            textBox1.MaxLength = textBox2.MaxLength = BalancedTryte.TryteSize;
            textBox1.Tag = textBox4;
            textBox2.Tag = textBox5;
            textBox3.Tag = textBox6;

            textBox4.Tag = textBox1;
            textBox5.Tag = textBox2;
            textBox6.Tag = textBox3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var x = BalancedTryte.Parse(textBox1.Text);
            var y = BalancedTryte.Parse(textBox2.Text);
            BalancedTryte z = 0;

            switch (comboBox1.Text[0])
            {
                case '+':
                    z = x + y;
                    break;
                case '-':
                    z = x - y;
                    break;
                case 'x':
                    z = x * y;
                    break;
                case '/':
                    z = x / y;
                    break;
            }

            textBox3.Text = z.ToString();
        }

        private void TernaryTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var target = textBox.Tag as TextBox;
            if (target == null)
                return;

            target.Tag = null;
            BalancedTryte tryte;
            if (BalancedTryte.TryParse(textBox.Text, out tryte))
            {
                target.Text = tryte.ToInt32().ToString();
                target.ForeColor = Color.Black;
                textBox.ForeColor = Color.Black;
                button1.Enabled = true;
            }
            else
            {
                target.Text = "?";
                target.ForeColor = Color.Red;
                textBox.ForeColor = Color.Red;
                button1.Enabled = false;
            }
            target.Tag = textBox;
        }

        private void DecimalTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var target = textBox.Tag as TextBox;
            if (target == null)
                return;

            target.Tag = null;

            try
            {
                target.Text = BalancedTryte.FromInt32(int.Parse(textBox.Text)).ToString();
                target.ForeColor = Color.Black;
                textBox.ForeColor = Color.Black;
                button1.Enabled = true;
            }
            catch
            {
                target.Text = new string('?', BalancedTryte.TryteSize);
                target.ForeColor = Color.Red;
                textBox.ForeColor = Color.Red;
                button1.Enabled = false;
            }
            target.Tag = textBox;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Logic written by Jerre Starink and Tijn de Leeuw.");
        }
    }
}
