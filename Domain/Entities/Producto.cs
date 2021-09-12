using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Existencia { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public UnidadMedida UnidadMedida { get; set; }

        public class ProductoIdComparer : IComparer<Producto>
        {
            public int Compare(Producto i, Producto j)
            {
                if (i.Id > j.Id)
                {
                    return 1;
                }

                else if (i.Id < j.Id)
                {
                    return -1;
                }

                else
                {
                    return 0;
                }
            }
        }

        public int CompareTo(Producto other)
        {
            return new ProductoIdComparer().Compare(this, other);
        }
    }
}
