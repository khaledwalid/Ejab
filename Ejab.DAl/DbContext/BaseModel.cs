namespace Ejab.DAl
{
    public class BaseModel
    {
        public int Id { get; set; }
        public short  FlgStatus { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime? UpdatedOn { get; set; }

    }
}