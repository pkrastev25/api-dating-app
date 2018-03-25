namespace api_dating_app.Helpers
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class UserParamsHelper
    {
        private const int MaxPageSize = 50;

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public int UserId { get; set; }

        public string Gender { get; set; }

        public int MinAge { get; set; } = 10;

        public int MaxAge { get; set; } = 99;

        public string OrderBy { get; set; }

        public bool Likees { get; set; } = false;

        public bool Likers { get; set; } = false;
    }
}