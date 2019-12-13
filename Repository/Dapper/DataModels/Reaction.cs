
namespace Repository.Dapper.DataModels
{
    public class Reaction
    {
        public string UserEmail { get; set; }

        public long PostId { get; set; }

        public bool? IsPositive { get; set; }

        public string Text { get; set; }
    }
}
