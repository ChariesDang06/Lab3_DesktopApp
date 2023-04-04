using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_Demo
{
    
    public partial class frmTuyChon : Form
    {
        QuanLySinhVien ds = new QuanLySinhVien();
        public frmTuyChon()
        {
            InitializeComponent();
        }
        public string ChuoiTim 
        {
            get { return this.txtThongTin.Text; }
            set { this.txtThongTin.Text = value; }
        }
        public bool panelThongTinVisible
        {
            get { return this.panelThongTin.Visible; }
            set { this.panelThongTin.Visible = value; }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void frmTuyChon_Load(object sender, EventArgs e)
        {
            
        }
        internal TuyChon chon;
        private void TuyChonKieu()
        {
            if (rdMaSV.Checked)
                chon = TuyChon.TheoMa;
            else if (rdHoTen.Checked)
                chon = TuyChon.TheoHoTen;
            else if (rdNgaySinh.Checked)
                chon = TuyChon.TheoNgaySinh;
        }
        private void btnSapXep_Click(object sender, EventArgs e)
        {
            TuyChonKieu();
            this.Close();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            TuyChonKieu();
            this.Close();
        }
    }
}
