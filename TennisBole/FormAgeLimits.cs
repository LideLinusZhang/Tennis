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
    public partial class FormAgeLimits : Form
    {
        public FormAgeLimits()
        {
            InitializeComponent();
        }

        private void buttonAddLimit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxNationality.Text) &&
                numericUpDownAge.Value != 0)
            {
                listViewLimits.BeginUpdate();

                string nationality = comboBoxNationality.Text;
                string age = Convert.ToInt32(numericUpDownAge.Value).ToString();

                foreach(ListViewItem limitItem in listViewLimits.Items.Find(nationality,true))
                {
                    listViewLimits.Items.Remove(limitItem);
                }

                ListViewItem newLimitItem = new ListViewItem(nationality);
                newLimitItem.SubItems.Add(age);

                listViewLimits.Items.Add(newLimitItem);

                listViewLimits.EndUpdate();
            }
        }

        private void buttonRemoveLimit_Click(object sender, EventArgs e)
        {
            if(listViewLimits.SelectedItems.Count!=0)
            {
                listViewLimits.BeginUpdate();

                foreach (ListViewItem limitItem in listViewLimits.SelectedItems)
                    listViewLimits.Items.Remove(limitItem);

                listViewLimits.EndUpdate();
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            listViewLimits.BeginUpdate();

            listViewLimits.Items.Clear();

            listViewLimits.EndUpdate();
        }
    }
}
