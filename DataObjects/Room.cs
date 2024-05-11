namespace DataObjects
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomTypeID { get; set; }
        public bool RoomAvailability { get; set; }
        public string RoomStatus { get; set; }
    }
    public class RoomVM : Room
    {
        public int RoomPrice { get; set; }
        public RoomType RoomType { get; set; }
    }
}
