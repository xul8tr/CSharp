using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Amoba
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Program.columns = (int)this.numericUpColumn.Value;
            Program.rows = (int)this.numericUpRow.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.numericUpColumn.Value = (decimal)Program.columns;
            this.numericUpRow.Value = (decimal)Program.rows;
        }

    }
}
