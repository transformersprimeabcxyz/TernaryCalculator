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

namespace TernaryConverter
{
    public unsafe partial class Form1 : Form
    {
        private bool _isUpdating = false;

        public Form1()
        {
            InitializeComponent();
            balancedTextBox.MaxLength = unbalancedTextBox.MaxLength = UnbalancedTryte.TryteSize;
        }

        private void decimalTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdating) 
                return;

            _isUpdating = true;
            decimalTextBox.ForeColor = Color.Black;

            int input;
            if (!int.TryParse(decimalTextBox.Text, out input))
            {
                if (string.IsNullOrEmpty(decimalTextBox.Text))
                    input = 0;
                else
                {
                    decimalTextBox.ForeColor = Color.Red;
                    unbalancedTextBox.Text = new string('?', UnbalancedTryte.TryteSize);
                    balancedTextBox.Text = new string('?', BalancedTryte.TryteSize);
                    _isUpdating = false;
                    return;
                }
            }

            UnbalancedTryte unbalanced = input;
            BalancedTryte balanced = input;
            unbalancedTextBox.Text = unbalanced.ToString();
            balancedTextBox.Text = balanced.ToString();
            _isUpdating = false;

        }

        private void unbalancedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdating)
                return;

            _isUpdating = true;
            unbalancedTextBox.ForeColor = Color.Black;

            UnbalancedTryte unbalanced;
            if (!UnbalancedTryte.TryParse(unbalancedTextBox.Text, out unbalanced))
            {
                if (string.IsNullOrEmpty(unbalancedTextBox.Text))
                    unbalanced = 0;
                else
                {
                    decimalTextBox.Text = "?";
                    unbalancedTextBox.ForeColor = Color.Red;
                    balancedTextBox.Text = new string('?', BalancedTryte.TryteSize);

                    _isUpdating = false;
                    return;
                }
            }
            int intValue =  unbalanced.ToInt32();
            decimalTextBox.Text = intValue.ToString();
            BalancedTryte balanced = intValue;
            balancedTextBox.Text = balanced.ToString();
            _isUpdating = false;
        }

        private void balancedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdating)
                return;

            _isUpdating = true;
            balancedTextBox.ForeColor = Color.Black;

            BalancedTryte balanced;
            if (!BalancedTryte.TryParse(balancedTextBox.Text, out balanced))
            {
                if (string.IsNullOrEmpty(balancedTextBox.Text))
                    balanced = 0;
                else
                {
                    decimalTextBox.Text = "?";
                    unbalancedTextBox.Text = new string('?', UnbalancedTryte.TryteSize);
                    balancedTextBox.ForeColor = Color.Red;
                    _isUpdating = false;
                    return;
                }
            }
            int intValue = balanced.ToInt32();
            decimalTextBox.Text = intValue.ToString();
            UnbalancedTryte unbalanced = intValue;
            unbalancedTextBox.Text = unbalanced.ToString();
            _isUpdating = false;
        }
    }
}
