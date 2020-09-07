using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TMTDentalFPS.Utilities
{
    internal class MyUniversalStatic
    {
        public static void ChangeGridProperties(DataGridView dataGridView)
        {
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 5, 0, 5);

            /*
            dataGridView.Columns[0].HeaderCell.Style.Font = new Font("Tahoma", 18, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 25, FontStyle.Bold);

            dataGridView.Columns[0].HeaderCell.Style.ForeColor = Color.Red;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;

            dataGridView.Columns[0].HeaderCell.Style.BackColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;
            */

            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            //dataGridView.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.AliceBlue;
            dataGridView.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
            //dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.RowHeadersWidth = 25;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.RowHeadersVisible = false;
            //foreach (DataGridViewColumn c in dataGridView.Columns)
            //{
            //    c.Resizable = DataGridViewTriState.False;
            //    c.ReadOnly = true;
            //}
            //dataGridView.AllowUserToOrderColumns = true; // Switch Cols

            dataGridView.EnableHeadersVisualStyles = false;
        }

    }
}
