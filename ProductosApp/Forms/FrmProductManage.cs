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
    public partial class FrmProductManage : Form
    {
        private ProductoModel productoModel;

        public FrmProductManage()
        {
            productoModel = new ProductoModel();
            InitializeComponent();
        }

        private void FrmProductManage_Load(object sender, EventArgs e)
        {
            cmbMeasureUnit.Items.AddRange(Enum.GetValues(typeof(UnidadMedida))
                                              .Cast<object>().ToArray()
                                         );
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbFinder.SelectedIndex)
            {
                case 0:
                    pnlId.Visible = true;
                    pnlMeasureUnit.Visible = false;
                    pnlPriceRange.Visible = false;
                    pnlCaducity.Visible = false;
                    break;
                case 1:
                    pnlId.Visible = false;
                    pnlMeasureUnit.Visible = true;
                    pnlPriceRange.Visible = false;
                    pnlCaducity.Visible = false;
                    break;
                case 2:
                    pnlId.Visible = false;
                    pnlMeasureUnit.Visible = false;
                    pnlPriceRange.Visible = true;
                    pnlCaducity.Visible = false;
                    break;
                case 3:
                    pnlId.Visible = false;
                    pnlMeasureUnit.Visible = false;
                    pnlPriceRange.Visible = false;
                    pnlCaducity.Visible = true;
                    break;
            }
        }

        public void ValoresPorDefecto()
        {
            cmbMeasureUnit.SelectedIndex = -1;
            txtId.Text = string.Empty;
            nudToPrice.Value = nudToPrice.Minimum;
            nudFromPrice.Value = nudFromPrice.Minimum;
            dtpCaducity.Value = DateTime.Now;
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            FrmProducto frmProducto = new FrmProducto();
            frmProducto.ProductoModel = productoModel;
            frmProducto.ShowDialog();

            rtbProductView.Text = productoModel.GetProductosAsJson();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            switch (cmbFinder.SelectedIndex)
            {
                //ID
                case 0:
                    if (string.IsNullOrEmpty(txtId.Text))
                    {
                        MessageBox.Show("Error", "La caja de texto no puede quedar vacia.",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!int.TryParse(txtId.Text, out int codigo))
                    {
                        MessageBox.Show($"El codigo: {txtId.Text} no es valido.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Producto p = productoModel.GetProductoById(codigo);
                    if (p != null)
                    {
                        rtbProductView.Text = $"El producto con ID: {codigo} es: \n";
                        rtbProductView.Text += JsonConvert.SerializeObject(p);
                        txtId.Text = string.Empty;
                    }

                    else
                    {
                        MessageBox.Show("Error", "No se ha encontrado el producto con ese codigo.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                break;

                //Rango de precios
                case 1:
                    if (nudToPrice.Value < nudFromPrice.Value)
                    {
                        MessageBox.Show("Error", "El precio inicial no puede ser menor al final.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        nudToPrice.Value = nudToPrice.Minimum;
                        nudFromPrice.Value = nudFromPrice.Minimum;
                        return;
                    }

                    Producto[] productos = productoModel.GetProductosByRangoPrecio(nudFromPrice.Value, nudToPrice.Value);
                    if (productos != null)
                    {
                        rtbProductView.Text = $"Los productos con precio entre {nudFromPrice.Value} y {nudToPrice.Value} son: \n";
                        rtbProductView.Text += JsonConvert.SerializeObject(productos);
                    }

                    else
                    {
                        MessageBox.Show("Error", "No se han encontrado productos con el rango de precio ingresado.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                break;

                //Unidad de Medida
                case 2:
                    if (cmbMeasureUnit.SelectedIndex >= 0)
                    {
                        productos = productoModel.GetProductosByUnidadMedida((UnidadMedida)cmbMeasureUnit.SelectedIndex);
                        if (productos != null)
                        {
                            rtbProductView.Text = $"Los productos con unidad de medida: {(UnidadMedida)cmbMeasureUnit.SelectedIndex} son: \n";
                            rtbProductView.Text += productoModel.ConvertAsJson(productos);
                        }

                        else
                        {
                            MessageBox.Show("Error", "No se han encontrado productos con esa unidad de medida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    else
                    {
                        MessageBox.Show("Error", "No se ha seleccionado ninguna unidad de medida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                break;

                //Fecha de vencimiento
                case 3:
                    DateTime dt = dtpCaducity.Value;
                    productos = productoModel.GetProductosByVencimiento(dt);
                    if (productos != null)
                    {
                        rtbProductView.Text = $"Los productos con fecha de caducidad próxima a: {dt} son: \n";
                        rtbProductView.Text += productoModel.ConvertAsJson(productos);
                    }

                    else
                    {
                        MessageBox.Show("Error", "No se han encontrado productos con dichas caracteristicas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                break;

                //Mostrar productos ordenados por ID
                case 4:
                    if (productoModel.GetAll() != null)
                    {
                        rtbProductView.Text = $"Los productos en existencia son: \n";
                        Array.Sort(productoModel.GetAll(), new Producto.ProductoIdComparer());
                        rtbProductView.Text += productoModel.GetProductosAsJson();
                    }

                    else
                    {
                        MessageBox.Show("Error", "No hay productos registrados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                break;

                default:
                    MessageBox.Show("Error", "No se ha seleccionado ninguna opcion de busqueda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                break;
            }


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (productoModel.GetAll() != null)
            {
                FrmObtenerID frmActualizar = new FrmObtenerID();
                frmActualizar.pModel = productoModel;
                frmActualizar.ShowDialog();
                rtbProductView.Text = productoModel.GetProductosAsJson();
            }

            else
            {
                MessageBox.Show("Error", "No hay productos para actualizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (productoModel.GetAll() != null)
            {
                FrmEliminar frmEliminar = new FrmEliminar();
                frmEliminar.pModel = productoModel;
                frmEliminar.ShowDialog();
                rtbProductView.Text = productoModel.ConvertAsJson();
            }
            else
            {
                MessageBox.Show("No hay productos para eliminar", "Mensaje de error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
