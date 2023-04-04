using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab3_Demo
{
    public partial class frmSinhVien : Form
    {
        QuanLySinhVien qlsv = new QuanLySinhVien();
        public frmSinhVien()
        {
            InitializeComponent();
        }
        //Lấy thông tin từ controls thông tin SV
        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien();
            bool gt = true;
            List<string> cn = new List<string>();
            sv.MaSo = this.mtxtMaSo.Text;
            sv.HoTen = this.txtHoTen.Text;
            sv.NgaySinh = this.dtpNgaySinh.Value;
            sv.DiaChi = this.txtDiaChi.Text;
            sv.Lop = this.cboLop.Text;
            sv.Hinh = this.txtHinh.Text;
            if (rdNu.Checked)
                gt = false;
            sv.GioiTinh = gt;
            for (int i = 0; i < this.clbChuyenNganh.Items.Count; i++)
                if (clbChuyenNganh.GetItemChecked(i))
                    cn.Add(clbChuyenNganh.Items[i].ToString());
            sv.ChuyenNganh = cn;
            return sv;
        }
        //Lấy thông tin sinh viên từ dòng item của ListView
        private SinhVien GetSinhVienLV(ListViewItem lvitem)
        {
            SinhVien sv = new SinhVien();
            sv.MaSo = lvitem.SubItems[0].Text;
            sv.HoTen = lvitem.SubItems[1].Text;
            sv.NgaySinh = DateTime.Parse(lvitem.SubItems[2].Text);
            sv.DiaChi = lvitem.SubItems[3].Text;
            sv.Lop = lvitem.SubItems[4].Text;
            sv.GioiTinh = false;
            if (lvitem.SubItems[5].Text == "Nam")
                sv.GioiTinh = true;
            List<string> cn = new List<string>();
            string[] s = lvitem.SubItems[6].Text.Split(',');
            foreach (string t in s)
                cn.Add(t);
            sv.ChuyenNganh = cn;
            sv.Hinh = lvitem.SubItems[7].Text;
            return sv;
        }
        //Thiết lập các thông tin lên controls sinh viên
        private void ThietLapThongTin(SinhVien sv)
        {
            this.mtxtMaSo.Text = sv.MaSo;
            this.txtHoTen.Text = sv.HoTen;
            this.dtpNgaySinh.Value = sv.NgaySinh;
            this.txtDiaChi.Text = sv.DiaChi;
            this.cboLop.Text = sv.Lop;
            this.txtHinh.Text = sv.Hinh;
            this.pbHinh.ImageLocation = sv.Hinh;
            if (sv.GioiTinh)
                this.rdNam.Checked = true;
            else
                this.rdNu.Checked = true;
            for (int i = 0; i < this.clbChuyenNganh.Items.Count; i++)
                this.clbChuyenNganh.SetItemChecked(i, false);
            foreach (string s in sv.ChuyenNganh)
            {
                for (int i = 0; i < this.clbChuyenNganh.Items.Count; i++)
                    if (s.CompareTo(this.clbChuyenNganh.Items[i]) == 0)
                        this.clbChuyenNganh.SetItemChecked(i, true);
            }
        }
        //Thêm sinh viên vào ListView
        private void ThemSV(SinhVien sv)
        {
            ListViewItem lview = new ListViewItem(sv.MaSo);
            lview.SubItems.Add(sv.HoTen);
            lview.SubItems.Add(sv.NgaySinh.ToShortDateString());
            lview.SubItems.Add(sv.DiaChi);
            lview.SubItems.Add(sv.Lop);
            string gt = "Nữ";
            if (sv.GioiTinh)
                gt = "Nam";
            lview.SubItems.Add(gt);
            string cn = "";
            foreach (string s in sv.ChuyenNganh)
                cn += s + ",";
            cn = cn.Substring(0, cn.Length - 1);
            lview.SubItems.Add(cn);
            lview.SubItems.Add(sv.Hinh);
            this.lvSinhVien.Items.Add(lview);
        }
        //Hiển thị các sinh viên trong qlsv lên ListView
        public void LoadListView()
        {
            this.lvSinhVien.Items.Clear();
            foreach (SinhVien sv in qlsv.DanhSach)
            {
                ThemSV(sv);
            }
            tssTong.Text = "Tổng số sinh viên: " + lvSinhVien.Items.Count.ToString();
        }
        //sự kiện Load form
        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            qlsv.DocTuFile();
            LoadListView();
        }

       
        //Khi chọn dòng sinh viên bên ListView 
        //thực hiện gán thông tin lên các control
        private void lvSinhVien_SelectedIndexChanged(object sender,EventArgs e)
        {
            int count = this.lvSinhVien.SelectedItems.Count;
            if (count > 0)
            {
                ListViewItem lvitem =
                this.lvSinhVien.SelectedItems[0];
                SinhVien sv = GetSinhVienLV(lvitem);
                ThietLapThongTin(sv);
            }
        }

        //Chức năng thêm sinh viên
        private void btnThem_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            SinhVien kq = qlsv.Tim(sv.MaSo,
            delegate (object obj1, object obj2)
            {
                return (obj2 as SinhVien).MaSo.CompareTo(obj1.ToString());
            });
            if (kq != null)
                MessageBox.Show("Mã sinh viên đã tồn tại!", "Lỗi thêm dữ liệu",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.qlsv.Them(sv);
                //ThemSV(sv);
                this.LoadListView();
                
                //GetListCount();
               
            }

            
        }
        //Thoát chương trình
        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Xóa tất cả sinh viên được chọn trên ListView
        private void btnXoa_Click(object sender, EventArgs e)
        {
            int count, i;
            ListViewItem lvitem;
            count = this.lvSinhVien.Items.Count - 1;
            for (i = count; i >= 0; i--)
            {
                lvitem = this.lvSinhVien.Items[i];
                if (lvitem.Checked)
                    qlsv.Xoa(lvitem.SubItems[0].Text, SoSanhTheoMa);
            }
            this.LoadListView();
            this.btnMacDinh.PerformClick();

        }
        //Để các control ở giá trị mặc định
        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            this.mtxtMaSo.Text = "";
            this.txtHoTen.Text = "";
            this.dtpNgaySinh.Value = DateTime.Now;
            this.txtDiaChi.Text = "";
            this.cboLop.Text = this.cboLop.Items[0].ToString();
            this.txtHinh.Text = "";
            this.pbHinh.ImageLocation = "";
            this.rdNam.Checked = true;
            for (int i = 0; i < this.clbChuyenNganh.Items.Count - 1;
            i++)
                this.clbChuyenNganh.SetItemChecked(i, false);
        }
        //Sửa thông tin sinh viên được chọn
        private void btnSua_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            bool kqsua;
            kqsua = qlsv.Sua(sv, sv.MaSo, SoSanhTheoMa);
            if (kqsua)
            {
                this.LoadListView();
            }
        }
        private int SoSanhTheoMa(object obj1, object obj2)
        {
            SinhVien sv = obj2 as SinhVien;
            return sv.MaSo.CompareTo(obj1);
        }

        //Mở file hình
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog openFileImage = new OpenFileDialog();
            DialogResult result = openFileImage.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileImage.FileName;
            }*/

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "";
                openFileDialog.Filter = "Image File(*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
                // openFileDialog.FilterIndex = 2;
                // openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open File Image";
                //openFileDialog.Tag = "Hãy chọn file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    
                    //Read the contents of the file into a stream
                    //var fileStream = openFileDialog.OpenFile();

                    /*using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }*/
                }
            }

        }
        private void toolStripOpenFile_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "";
                openFileDialog.Filter = "Image File(*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
                // openFileDialog.FilterIndex = 2;
                // openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open File Image";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    //var fileStream = openFileDialog.OpenFile();

                    /*using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }*/
                }
            }
        }
        //Thoát

        private void toolStripExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Thêm
        private void toolStripAdd_Click(object sender, EventArgs e)
        {
            this.btnThem.PerformClick();
        }
        //Xóa
        private void toolStripDelete_Click(object sender, EventArgs e)
        {
            this.btnXoa.PerformClick();
        }
        //Sửa
        private void toolStripEdit_Click(object sender, EventArgs e)
        {
            this.btnSua.PerformClick();
        }
        //Đồi font
        private void toolStripFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            DialogResult fontResult = fontDialog.ShowDialog();
            if (fontResult == DialogResult.OK)
            {
                lvSinhVien.Font = fontDialog.Font;
            }
        }
        //Đổi màu chữ
        private void toolStripFontColor_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            MyDialog.Color = lvSinhVien.ForeColor;

            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
                lvSinhVien.ForeColor = MyDialog.Color;
        }
        //=======================
        //Sắp xếp
        TuyChon kieu;
        string input = "";
        private void toolStripSort_Click(object sender, EventArgs e)
        {
            frmTuyChon frm = new frmTuyChon();
            frm.panelThongTinVisible = false;
            frm.ShowDialog();
            kieu = frm.chon;
            qlsv.SapXepTheoYeuCau(kieu);
            LoadListView();
        }
        //Tìm kiếm
        private void toolStripSearch_Click(object sender, EventArgs e)
        {
            lvSinhVien.Items.Clear();
            frmTuyChon frm = new frmTuyChon();
            frm.ShowDialog();
            input = frm.ChuoiTim;
            kieu = frm.chon;
            List<SinhVien> dsSV = new List<SinhVien>();
            dsSV = qlsv.TimKiemTheoYeuCau(kieu, input);
            if (input == String.Empty)
                MessageBox.Show("Hãy nhập thông tin cần tìm");
            else
            {
                if (dsSV.Count == 0)
                    MessageBox.Show("Không tìm thấy sinh viên có thông tin như yêu cầu");
                else
                {
                    MessageBox.Show($"Có {dsSV.Count()} sinh viên có thông tin theo yêu cầu");
                    foreach (SinhVien sv in dsSV)
                        ThemSV(sv);
                }
            }
        }
        
    }
}
