public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Domicilio { get; set; }
    public string Rfc { get; set; }
    //public decimal Saldo { get; set; }

    // Constructor vacío requerido por algunas librerías/frameworks
    public Cliente()
    {
        Id = 0;
        Nombre = string.Empty;
        Domicilio = string.Empty;
        Rfc = string.Empty;
    }

    // Tu constructor con parámetros habitual
    public Cliente(int id, string nombre, string domicilio, string rfc)
    {
        Id = id;
        Nombre = nombre;
        Domicilio = domicilio;
        Rfc = rfc;
       // Saldo = saldo;
    }
}