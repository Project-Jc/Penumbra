using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Penumbra
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (StopBotAfter.Checked)
            {
                label6.Show();
                StopAfterValue.Show();
            }
            else
            {
                label6.Hide();
                StopAfterValue.Hide();
            }
        }

        private void StopBotAfter_CheckedChanged(object sender, EventArgs e)
        {
            if (StopBotAfter.Checked)
            {
                label6.Show();
                StopAfterValue.Show();
            }
            else
            {
                label6.Hide();
                StopAfterValue.Hide();
            }
        }
    }
}
