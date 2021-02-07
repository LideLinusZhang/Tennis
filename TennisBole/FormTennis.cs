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

        private void button1_Click(object sender, EventArgs e)
        {
            listViewPlayers.BeginUpdate();
            listViewPlayers.Items.Clear();
            listViewPlayers.EndUpdate();

            FormSourceWeight f = new FormSourceWeight();
            f.ShowDialog();
        }

        private void buttonShowData_Click(object sender, EventArgs e)
        {
            TennisDataProcessor.ImportCSV("testUTR.csv", "testATP.csv");
            TennisDataProcessor.EliminateOverAgedPlayer();
            TennisDataProcessor.EliminateLowUTRPlayer();
            TennisDataProcessor.CalculateMyRank();

            listViewPlayers.BeginUpdate();
            foreach (ListViewItem limitItem in TennisDataProcessor.GetListViewItems())
                listViewPlayers.Items.Add(limitItem);
            listViewPlayers.EndUpdate();
        }
    }
}
