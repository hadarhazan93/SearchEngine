using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/**
 * Dictionary view window controller class
 * main feature of the window is loading dictionary files given from upper level window, supports few files and selection of needed one by operating the top combo box
 */

namespace SearchEngine
{
    public partial class DictionaryDialog : Form
    {
        private String[] dict_paths;
        public DictionaryDialog(string[] dict_paths)
        {
            InitializeComponent();
            if(File.Exists(dict_paths[0]))
                this.dict_selector.Items.Add(dict_paths[0]);
            if (File.Exists(dict_paths[1]))
                this.dict_selector.Items.Add(dict_paths[1]);
            if (this.dict_selector.Items.Count == 0)
                this.msg.Text = "Check index location!";
            /*for(int i = 0; i < dict_paths.Length; i++)
            {
                this.dict_selector.Items.Add(dict_paths[i]);
            }*/

        }

        private void close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
/*
 *function reloads contents of the window if another file chosen
 */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.msg.Text = "Loading...";
            String content = File.ReadAllText((String)this.dict_selector.SelectedItem);
            this.richTextBox1.Text = content;
            /*String[] termlist = content.Split(new string[] { "\n" }, StringSplitOptions.None);
            for(int i = 0; i < termlist.Length; i++)
            {
                if (termlist[i].Length < 3)
                    continue;
                string[] term = termlist[i].Split(';');
                ListViewItem it = new ListViewItem(term[0]);
                it.SubItems.Add(term[1]);
                this.list.Items.Add(it);
            }
            */
            this.msg.Text = "";

        }
    }
}
