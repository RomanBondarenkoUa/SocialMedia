namespace Domain.Users.Posts.Comments
{
    public class CreateCommentModel
    {
        public long PostId { get; set; }

        public string CommentatorEmail { get; set; }

        public string Text { get; set; }

        public bool? IsPositiveComment { get; set; }
    }
}
