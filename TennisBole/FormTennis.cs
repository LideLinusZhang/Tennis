using System;
using System.IO;
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

            if(DataFilesExist())
            {
                TennisDataProcessor.ImportCSV(UTRDataFileName, ATPDataFileName);
                buttonShowData.Enabled = true;
            }
            else
                buttonUpdate.Text = "Fetch Data";
        }
        private readonly string UTRDataFileName = "utr_data.csv";
        private readonly string ATPDataFileName = "atp_data.csv";
        private bool DataFilesExist()
        {
            if (File.Exists(UTRDataFileName) &&
                File.Exists(ATPDataFileName))
                return true;
            else
                return false;
        }

        private void buttonAgeLimits_Click(object sender, EventArgs e)
        {
            listViewPlayers.BeginUpdate();
            listViewPlayers.Items.Clear();
            listViewPlayers.EndUpdate();

            FormAgeLimits f = new FormAgeLimits();
            f.ShowDialog();
        }

        private void buttonWeight_Click(object sender, EventArgs e)
        {
            listViewPlayers.BeginUpdate();
            listViewPlayers.Items.Clear();
            listViewPlayers.EndUpdate();

            FormSourceWeight f = new FormSourceWeight();
            f.ShowDialog();
        }

        private void buttonShowData_Click(object sender, EventArgs e)
        {
            listViewPlayers.BeginUpdate();
            foreach (ListViewItem playerItem in TennisDataProcessor.GetPlayerListViewItems())
                listViewPlayers.Items.Add(playerItem);
            listViewPlayers.EndUpdate();

            listViewCountry.BeginUpdate();
            foreach (ListViewItem countryItem in TennisDataProcessor.GetCountryListViewItems())
                listViewCountry.Items.Add(countryItem);
            listViewCountry.EndUpdate();
        }

        private void FormTennis_Load(object sender, EventArgs e)
        {

        }
    }
}
