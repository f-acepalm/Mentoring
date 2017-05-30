using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace WindowsAppMemoryLeak
{
    public partial class frmMemoryLeakage : Form
    {

        private List<Brush> _brushes = new List<Brush>(); 

        public frmMemoryLeakage()
        {
            InitializeComponent();
        }

        private void btnUnManagedLeak_Click(object sender, EventArgs e)
        {
            timerManaged.Enabled = false;
            timerUnManaged.Enabled = true;
            
        }

        private void timerUnManaged_Tick(object sender, EventArgs e)
        {
            Marshal.AllocHGlobal(7000000);
        }

        private void btnManagedLeak_Click(object sender, EventArgs e)
        {
            timerManaged.Enabled = true;
            timerUnManaged.Enabled = false;
        }

        private void timerManaged_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 10000; i++)
            {
                Brush obj = new SolidBrush(Color.Blue);
                _brushes.Add(obj);
            }
        }

        private void frmMemoryLeakage_Load(object sender, EventArgs e)
        {

        }
    }
}