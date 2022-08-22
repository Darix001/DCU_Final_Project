using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCU_Project
{
    
    public partial class Menu : Form
    {
        db_schema db;
        public Menu()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            db.sqlite_conn.Close();
            Application.Exit();
        }


        int m, mx, my;


        private void titleBar_MouseDown(object sender, MouseEventArgs e)
        {
            m = 1;
            mx = e.X;
            my = e.Y;
        }

        private void Refresh_Table()
        {
            tablegrid.Rows.Clear();
            foreach (int id in db.reincidencias.Keys)
            {
                tablegrid.Rows.Add(new object[] {db.reincidencias[id].ID, db.reincidencias[id].Nombre,
                db.reincidencias[id].Apellido, db.reincidencias[id].Caso, db.reincidencias[id].Descripcion,
                db.reincidencias[id].Cantidad});
            }
            db.reincidencias.Clear();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            db=new db_schema();
            db.Run();
            db.CreateTable();
            db.ReadData();
            Refresh_Table();
        }

        private bool valid_id()
        {
            db.reincidencias.Clear();
            db.ReadData(txtid.Text);
            if (db.reincidencias.Count>0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void tablegrid_SelectionChanged(object sender, EventArgs e)
        {
            if (tablegrid.CurrentCell.RowIndex >= 0)
            {
                var current_row = tablegrid.Rows[tablegrid.CurrentCell.RowIndex];
                if (current_row != null)
                {
                    try
                    {
                        txtid.Text = current_row.Cells[0].Value.ToString();
                        txtnombre.Text = current_row.Cells[1].Value.ToString();
                        txtapellido.Text = current_row.Cells[2].Value.ToString();
                        txtcaso.Text = current_row.Cells[3].Value.ToString();
                        txtdescripcion.Text = current_row.Cells[4].Value.ToString();
                        txtcantidad.Text = current_row.Cells[5].Value.ToString();
                    }
                    catch (Exception)
                    {

                        
                    }
                    
                }
            }
        }

        public bool not_empty_fields()
        {
            bool not_empty_fields = true;
            if (txtid.Text == null & txtid.Text.Trim() == "")
            {
                MessageBox.Show("Error, the ID field is empty. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                not_empty_fields = false;
            }
            else if (txtnombre.Text == null & txtnombre.Text.Trim() == "")
            {
                MessageBox.Show("Error, the field 'nombre' is empty. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                not_empty_fields = false;
            }
            else if (txtapellido.Text == null & txtapellido.Text.Trim() == "")
            {
                MessageBox.Show("Error, the field 'apellido' is empty. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                not_empty_fields = false;
            }
            else if (txtcaso.Text == null & txtcaso.Text.Trim() == "")
            {
                MessageBox.Show("Error, the field 'caso' is empty. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                not_empty_fields = false;
            }
            else if (txtdescripcion.Text == null & txtdescripcion.Text.Trim() == "")
            {
                MessageBox.Show("Error, the ID field 'descripcion' is empty. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                not_empty_fields = false;
            }
            else if (txtcantidad.Text == null & txtcantidad.Text.Trim() == "")
            {
                MessageBox.Show("Error, the ID field 'cantidad' is empty. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                not_empty_fields = false;
            }
            return not_empty_fields;
        }

        public bool isdigit(string sd)
        {
            int x = 0;
            int.TryParse(sd, out x);
            if (x!=0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            if (not_empty_fields())
            {
                if (!isdigit(txtid.Text))
                {
                    MessageBox.Show("Error, the ID field is not a number. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!isdigit(txtcantidad.Text))
                {
                    MessageBox.Show("Error, the field 'cantidad' is not a number. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!valid_id())
                {
                    MessageBox.Show("this ID already exists. ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Reincidencias new_reincidencia = new Reincidencias(int.Parse(txtid.Text), txtnombre.Text,
                    txtapellido.Text, txtcaso.Text, txtdescripcion.Text, int.Parse(txtcantidad.Text));
                    db.InsertData(new_reincidencia);
                    tablegrid.Rows.Add(txtid.Text, txtnombre.Text,
                    txtapellido.Text, txtcaso.Text, txtdescripcion.Text, txtcantidad.Text);
                    MessageBox.Show("Insert Sucessfully!", "", MessageBoxButtons.OK);
                }
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (tablegrid.TabIndex >= 0)
            {
                var current_row = tablegrid.Rows[tablegrid.TabIndex];
                if (current_row != null)
                {
                    string id=current_row.Cells[0].Value.ToString();
                    string nombre= current_row.Cells[1].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show(String.Format("Are you sure you want to delete this row?\nID={0}\nNombre={1}", id, nombre), "Delete Row", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.DeleteData(txtid.Text);
                        tablegrid.Rows.Remove(current_row);
                        MessageBox.Show("Delete Sucessfully!", "", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (not_empty_fields())
            {
                Reincidencias new_reincidencia = new Reincidencias(int.Parse(txtid.Text), txtnombre.Text,
                txtapellido.Text, txtcaso.Text, txtdescripcion.Text, int.Parse(txtcantidad.Text));
                db.UpdateData(txtid.Text, new_reincidencia);
                db.ReadData();
                Refresh_Table();
                MessageBox.Show("Update Sucessfully!", "", MessageBoxButtons.OK);
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            if (txtbuscar.Text!=null & txtbuscar.Text.Trim()!="")
            {
                db.ReadData(txtbuscar.Text);
                Refresh_Table();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            db.ReadData();
            Refresh_Table();
        }

        private void tablegrid_TabIndexChanged(object sender, EventArgs e)
        {
            tablegrid_SelectionChanged(sender, e);
        }

        private void tablegrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tablegrid_SelectionChanged(sender, e);
        }

        private void titleBar_MouseMove(object sender, MouseEventArgs e)
        {
           if (m == 1)
            {
                this.SetDesktopLocation(MousePosition.X - mx, MousePosition.Y - my);
            }
        }

        private void titleBar_MouseUp(object sender, MouseEventArgs e)
        {
            m = 0;
        }


    }
}
