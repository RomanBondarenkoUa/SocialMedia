namespace Domain.Users.SocialActivities
{
    public class Comment
    {
        public string CreatorName { get; set; }

        public string Text { get; set; }
        
        public bool? IsPositiveComment { get; set; }
    }
}
