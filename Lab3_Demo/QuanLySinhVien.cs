using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab3_Demo
{

    enum TuyChon
    {
        TheoMa,
        TheoHoTen,
        TheoNgaySinh
    }
    public delegate int SoSanh(object sv1, object sv2);
    class QuanLySinhVien
    {
        public List<SinhVien> DanhSach;
        public QuanLySinhVien()
        {
            DanhSach = new List<SinhVien>();
        }
        public void Them(SinhVien sv)
        {
            this.DanhSach.Add(sv);
        }
        public SinhVien this[int index]
        {
            get { return this.DanhSach[index]; }
            set { DanhSach[index] = value; }
        }
        
        //Xóa các obj trong danh sách nếu thỏa điều kiện so sánh
        public void Xoa(object obj, SoSanh ss)
        {
            int i = DanhSach.Count - 1;
            for (; i >= 0; i--)
                if (ss(obj, this[i]) == 0)
                    this.DanhSach.RemoveAt(i);
            ;
        }
        //Tìm một sinh viên trong danh sách thỏa điều kiện so sánh
        public SinhVien Tim(object obj, SoSanh ss)
        {
            SinhVien svresult = null;
            foreach (SinhVien sv in DanhSach)
                if (ss(obj, sv) == 0)
                {
                    svresult = sv;
                    break;
                }
            return svresult;
        }
        //Tìm một sinh viên trong danh sách thỏa điều kiện so sánh, 
        //gán lại thông tin cho sinh viên này thành svsua
        public bool Sua(SinhVien svsua, object obj, SoSanh ss)
        {
            int i, count;
            bool kq = false;
            count = this.DanhSach.Count - 1;
            for (i = 0; i < count; i++)
                if (ss(obj, this[i]) == 0)
                {
                    this[i] = svsua;
                    kq = true;
                    break;
                }
            return kq;
        }

        // Hàm đọc danh sách sinh viên từ tập tin txt
        public void DocTuFile()
        {
            string filename = "DanhSachSV.txt", t;
            string[] s;
            SinhVien sv;
            using (StreamReader sr = new StreamReader(
             new FileStream(filename,
            FileMode.Open)))
            {
                while ((t = sr.ReadLine()) != null)
                {
                    s = t.Split('\t');
                    sv = new SinhVien();
                    sv.MaSo = s[0];
                    sv.HoTen = s[1];
                    sv.NgaySinh = DateTime.Parse(s[2]);
                    sv.DiaChi = s[3];
                    sv.Lop = s[4];
                    sv.Hinh = s[5];
                    sv.GioiTinh = false;
                    if (s[6] == "1")
                        sv.GioiTinh = true;
                    string[] cn = s[7].Split(',');
                    foreach (string c in cn)
                        sv.ChuyenNganh.Add(c);
                    this.Them(sv);
                }
            }
                
        }

        public void SapXepTheoYeuCau(TuyChon kieu)
        {
            switch(kieu)
            {
                case TuyChon.TheoMa:
                    SapXep(SoSanhTangTheoMaSo);
                    break;
                case TuyChon.TheoHoTen:
                    SapXep(SoSanhTangTheoTen);
                    break;
                case TuyChon.TheoNgaySinh:
                    SapXep(SoSanhTangTheoNgaySinh);
                    break;
            }
        }
        public List<SinhVien> TimKiemTheoYeuCau(TuyChon kieu, string input)
        {
            List<SinhVien> result = new List<SinhVien>();
            switch (kieu)
            {
                case TuyChon.TheoMa:
                    result = DanhSach.FindAll(x => x.MaSo == input.Trim());
                    break;
                case TuyChon.TheoHoTen:
                    result = DanhSach.FindAll(x => x.HoTen == input.Trim());
                    break;
                case TuyChon.TheoNgaySinh:
                    result = DanhSach.FindAll(x => x.NgaySinh == DateTime.Parse(input.Trim()));
                    break;
            }
            return result;
        }
        static int SoSanhTangTheoMaSo(object a, object b)
        {
            SinhVien gv1 = (SinhVien)a;
            SinhVien gv2 = (SinhVien)b;
            return gv1.MaSo.CompareTo(gv2.MaSo);
        }
        static int SoSanhTangTheoTen(object a, object b)
        {
            SinhVien sv1= (SinhVien)a;
            SinhVien sv2 = (SinhVien)b;
            return sv1.HoTen.CompareTo(sv2.HoTen);
        }
        static int SoSanhTangTheoNgaySinh(object a, object b)
        {
            SinhVien sv1 = (SinhVien)a;
            SinhVien sv2 = (SinhVien)b;
            return sv1.NgaySinh.CompareTo(sv2.NgaySinh);
        }
       
        public void SapXep(SoSanh ss)
        {
            for (int i = 0; i < this.DanhSach.Count - 1; i++)
                for (int j = i + 1; j < this.DanhSach.Count; j++)
                    if (ss(this.DanhSach[i], this.DanhSach[j]) == 1)
                    {
                        SinhVien temp = this.DanhSach[i];
                        this.DanhSach[i] = this.DanhSach[j];
                        this.DanhSach[j] = temp;
                    }
        }
    }
}
