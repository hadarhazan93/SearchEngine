using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine
{
    public partial class CountrySelectDialog : Form
    {
        List<string> cityChecked = new List<string>();
        public CountrySelectDialog()
        {
            InitializeComponent();
            this.populateList();
        }
        public void populateList()
        {
            city c = new city();
            String[] city = c.returnCities();
            checkedListBox1.Items.AddRange(city);
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            StartDialog.cityCheckedByUser = cityChecked.ToArray();
            this.Close();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox s = (CheckedListBox)sender;
            string item = ((string)s.SelectedItem).Substring(0, ((string)s.SelectedItem).Length - 1);
            if (!cityChecked.Contains(item))
                cityChecked.Add(item);
            else
                cityChecked.Remove(item);

        }
    }
}
