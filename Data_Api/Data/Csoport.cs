namespace Data_Api.Data
{
    public class Csoport
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List<Tanulo> Tanulok { get; set; } = [];

    }
}
