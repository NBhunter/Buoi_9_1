using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai_8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] NV = new string[6];
        SqlDataAdapter daChucVu;
        SqlDataAdapter daNhanVien;
        DataSet ds = new DataSet();
        private string ktcongviec(string Row)
        {
            if (Row == "CV")
            {
                return "Chuyên Viên";
            }
            else if (Row == "LX")
            {
                return "Lái xe cơ quan";
            }
            else if (Row == "KT")
            {
                return "Kế toán";
            }
            else if (Row == "PP")
            {
                return "Phó trưởng phòng";
            }
            else
                return "Trưởng phòng";
        }
        private string ktmacongviec(string Row)
        {
            if (Row == "Chuyên Viên")
            {
                return "CV";
            }
            else if (Row == "Lái xe cơ quan")
            {
                return "LX";
            }
            else if (Row == "Kế toán")
            {
                return "KT";
            }
            else if (Row == "Phó trưởng phòng")
            {
                return "PP";
            }
            else
                return "TP";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            int n = dgDSNhanVien.RowCount;
            int m = 0;
            for (int i = 0; i < n - 1; i++)
            {
                DataGridViewRow dr = dgDSNhanVien.Rows[i];
                if (txtmsnv.Text == dr.Cells["manv"].Value.ToString())
                {
                    MessageBox.Show("MSNV đã có, vui lòng thay đổi");
                    m++;
                    break;
                }



            }
            if (m == 0)
            {
                DataRow row = ds.Tables["tblDSNhanVien"].NewRow();
                row["manv"] = txtmsnv.Text;
                row["holot"] = txtHo.Text;
                row["tennv"] = txtTen.Text;
                if (rb2.Checked == true)
                {
                    row["phai"] = "Nữ";
                }
                else
                {
                    row["phai"] = "Nam";
                }
                row["ngaysinh"] = tme.Text;
                row["macv"] = cboChucVu.SelectedValue;
                row["tencv"] = ktcongviec(row["macv"].ToString());
                ds.Tables["tblDSNhanVien"].Rows.Add(row);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Server=DESKTOP-Q7NJU74\NBHUNTER; Database=QLNV;;Integrated Security=True";

            // Dữ liệu combobox Chức vụ
            string sQueryChucVu = @"select * from chucvu";
            daChucVu = new SqlDataAdapter(sQueryChucVu, conn);
            daChucVu.Fill(ds, "tblChucVu");
            cboChucVu.DataSource = ds.Tables["tblChucVu"];
            cboChucVu.DisplayMember = "tencv";
            cboChucVu.ValueMember = "macv";
            // Dữ liệu datagrid Danh sách nhân viên 
            string sQueryNhanVien = @"select n.*, c.tencv from nhanvien n, chucvu c where 
n.macv=c.macv";
            daNhanVien = new SqlDataAdapter(sQueryNhanVien, conn);
            daNhanVien.Fill(ds, "tblDSNhanVien");
            dgDSNhanVien.DataSource = ds.Tables["tblDSNhanVien"];

            // … đặt tiêu đề tiếng Việt, định độ rộng cho các trường còn lại
            // dgDSNhanVien.Columns["macv"].Visible = false;


        }

        private void dgDSNhanVien_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDSNhanVien.SelectedRows[0];
            txtmsnv.Text = dr.Cells["manv"].Value.ToString();
            txtHo.Text = dr.Cells["holot"].Value.ToString();
            txtTen.Text = dr.Cells["tennv"].Value.ToString();
            string gt = dr.Cells["phai"].Value.ToString();
            if (gt == "Nam")
            {
                rb1.Checked = true;
            }
            else
            {
                rb2.Checked = true;
            }
            tme.Value = (DateTime)dr.Cells["ngaysinh"].Value;
            cboChucVu.SelectedValue = dr.Cells["macv"].Value;

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Server=DESKTOP-Q7NJU74\NBHUNTER; Database=QLNV;Connection Timeout=60; User Id=sa;Password=bang123";
            string diachi = @"select * from nhanvien";
            DataAdapter da = new SqlDataAdapter(diachi, con);
            DataTable dt = (DataTable)dgDSNhanVien.DataSource;
            SqlCommandBuilder cb = new SqlCommandBuilder((SqlDataAdapter)da);
            //  da.Update(dt);
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDSNhanVien.SelectedRows[0];
            dr.Cells["manv"].Value = txtmsnv.Text;
            dr.Cells["holot"].Value = txtHo.Text;
            dr.Cells["tennv"].Value = txtTen.Text;
            if (rb1.Checked == true)
            {
                dr.Cells["phai"].Value = "Nam";
            }
            else
            {
                dr.Cells["phai"].Value = "Nu";
            }
            dr.Cells["macv"].Value = cboChucVu.SelectedValue;
            string cv = dr.Cells["macv"].Value.ToString();
            dr.Cells["tencv"].Value = ktcongviec(cv);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dgDSNhanVien.SelectedRows)
            {
                dgDSNhanVien.Rows.RemoveAt(item.Index);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            
        }
    }
}

