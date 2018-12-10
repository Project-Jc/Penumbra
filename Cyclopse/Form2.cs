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
    public partial class Form2 : Form
    {
        public bool Restock = false;
        public int ShopRestockPosition = 0;
        public string RestockType = string.Empty;

        public Form2()
        {
            InitializeComponent();
            Restock = false;
        }

        private void RestockLeather_CheckedChanged(object sender, EventArgs e)
        {
            ShopRestockPosition = 0;
            RestockType = "Leather";
            if (RestockLeather.Checked)
                Restock = true;
            else
                Restock = false;
        }

        private void RestockLinen_CheckedChanged(object sender, EventArgs e)
        {
            ShopRestockPosition = 3;
            RestockType = "Linen";
            if (RestockLinen.Checked)
                Restock = true;
            else
                Restock = false;
        }

        private void RestockCopper_CheckedChanged(object sender, EventArgs e)
        {
            ShopRestockPosition = 0;
            RestockType = "Copper";
            if (RestockCopper.Checked)
                Restock = true;
            else
                Restock = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
