namespace Sisvta.UseCases.DTOs
{
    public class ProductoDto
    {
        public string Claveproserv { get; set; }
        public string Clave { get; set; }
        public int? Prodserv { get; set; }
        public string Nombre { get; set; }
       
      
      
        public int? Linea { get; set; }
        public string? Nomlinea { get; set; }
        public int? Grupo { get; set; }
        public string? Nomgrupo { get; set; }
        public int? Status { get; set; }
        public double? Pcompra { get; set; }
        public decimal? Pventa { get; set; }
        public string Descripcion { get; set; }
        public int? Umedida { get; set; }
        //public string? Almacen { get; set; }
          public String? Moneda { get; set; }
        public bool? Inventariable { get; set; }

        public int? Maximo { get; set; }
        public int? Minimo { get; set; }
        public int? Reorden { get; set; }

    }
}