using Domain.Entities;
using Domain.Enums;
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
    public partial class FrmObtenerID : Form
    {
        public ProductoModel pModel { get; set; }

        public FrmObtenerID()
        {
            InitializeComponent();
        }

        private void FrmObtenerID_Load(object sender, EventArgs e)
        {
            cmbUnidadMedida.Items.AddRange(Enum.GetValues(typeof(UnidadMedida))
                                  .Cast<object>().ToArray()
                             );
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Producto p;

            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtDescripcion.Text) || cmbUnidadMedida.SelectedIndex == -1)
            {
                MessageBox.Show("Error","Hay campos vacios que son necesarios rellenar.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            else
            {
                p = new Producto()
                {
                    Id = (int)nudId.Value,
                    Nombre = txtName.Text,
                    Descripcion = txtDescripcion.Text,
                    Existencia = (int)nudExistencia.Value,
                    Precio = nudPrecio.Value,
                    FechaVencimiento = dtpVencimiento.Value,
                    UnidadMedida = (UnidadMedida)cmbUnidadMedida.SelectedIndex,
                };

                try
                {
                    pModel.Update(p);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Busqueda no encontrada", "El ID del producto que desea actualizar no se encontró", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Dispose();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
