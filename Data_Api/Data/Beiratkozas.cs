namespace Data_Api.Data
{
    public class Beiratkozas
    {
        public int Id { get; set; }
        public Guid TanuloId { get; set; }
        public Guid CsoportId { get; set; } = Guid.Empty;
    }
}
