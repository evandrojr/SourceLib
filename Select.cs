using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShowLib
{
    public partial class NeoSelect : Form
    {
        public Dictionary<string, string> Options;
        public int SelectedIndex = -1;
        public int SelectedValue;
        public string SelectedDescription;

        public NeoSelect(string keyHeader, string valueHeader, Dictionary<string,string> options)
        {
            Options = options;
            InitializeComponent();
            grid.ColumnCount = 2;
            grid.Columns[0].HeaderCell.Value = keyHeader;
            grid.Columns[1].HeaderCell.Value = valueHeader;
        }

        private void NeoSelect_Load(object sender, EventArgs e)
        {
            int row=0;
            foreach(KeyValuePair<string,string> opt in Options){
                grid.Rows.Add();
                grid.Rows[row].Cells[0].Value = opt.Key;
                grid.Rows[row].Cells[1].Value = opt.Value;
                ++row;
            }
            if (row > 0)
                grid.Rows[0].Selected = true;
        }

        private void NeoSelect_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void NeoSelect_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedIndex = grid.SelectedRows[0].Index;
                SelectedValue = Convert.ToInt32(grid.Rows[SelectedIndex].Cells[0].Value);
                SelectedDescription = Convert.ToString(grid.Rows[SelectedIndex].Cells[1].Value);
            }
            catch { }
        }

        private void grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}