using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TennisBole
{
    public partial class FormTennis : Form
    {
        public FormTennis()
        {
            InitializeComponent();
        }

        private void buttonAgeLimits_Click(object sender, EventArgs e)
        {
            listViewPlayers.BeginUpdate();
            listViewPlayers.Items.Clear();
            listViewPlayers.EndUpdate();

            FormAgeLimits f = new FormAgeLimits();
            f.ShowDialog();
        }
    }
}
