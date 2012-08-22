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
    public partial class FrmGrid : Form
    {
        DataTable dt;
        public List<object> SelectedRowLst; //Campo a ser retornado

        public FrmGrid(DataTable dt)
        {
            InitializeComponent();
            this.dt = dt;
        }

        private void FrmGrid_Load(object sender, EventArgs e)
        {
            grid.DataSource = dt;
        }

        private void FrmGrid_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FrmGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Grava os valores a serem retornados
            if (grid.RowCount != 0)
            {
                SelectedRowLst = new List<object>();
                for (int i = 0; i < grid.ColumnCount; ++i)
                {
                    SelectedRowLst.Add(grid.SelectedRows[0].Cells[i].Value);
                }
            }
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}