namespace api_dating_app.models
{
    public class LikeModel
    {
        public int LikerId { get; set; }

        public int LikeeId { get; set; }

        public UserModel Liker { get; set; }

        public UserModel Likee { get; set; }
    }
}