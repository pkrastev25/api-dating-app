namespace api_dating_app.models
{
    /// <summary>
    /// Model of a value. Used to store data in the DB.
    /// TODO: Remove in a future version. Used only to test the connection to the DB.
    /// </summary>
    public class ValueModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}