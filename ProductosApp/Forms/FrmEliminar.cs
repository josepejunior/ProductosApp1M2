using Domain.Entities;
using Infraestructure.Productos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductosApp.Forms
{
    public partial class FrmEliminar : Form
    {
        public ProductoModel pModel { get; set; }

        public FrmEliminar()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Producto p = new Producto()
            {
                Id = (int)nudId.Value
            };

            try
            {
                if (pModel.Delete(p) == true)
                {
                    MessageBox.Show("El producto se ha eliminado correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", "El producto que desea eliminar no existe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Dispose();
        }

        private void btnNot_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
