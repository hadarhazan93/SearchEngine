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
    public partial class ViewEntities : Form
    {
        public ViewEntities(String[] max5Entities)
        {
            InitializeComponent();
            putEntities(max5Entities);
        }

        private void putEntities(String[] max5Entities)
        {
            if (max5Entities.Length > 0)
                label1.Text = "1. " + max5Entities[0];
            if (max5Entities.Length > 1)
                label1.Text = "2. " + max5Entities[1];
            if (max5Entities.Length > 2)
                label1.Text = "3. " + max5Entities[2];
            if (max5Entities.Length > 3)
                label1.Text = "4. " + max5Entities[3];
            if (max5Entities.Length > 4)
                label1.Text = "5. " + max5Entities[4];
        }

        private void ProgressLabel_Click(object sender, EventArgs e)
        {

        }

        private void ViewEntities_Load(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
