using System;

namespace ProjetALT.src
{
    public class Message
    {
        private int id;
        private int student_id;
        private double gps_lat;
        private double gps_long;
        private string student_message;

        public Message()
        {
            this.Id = 0;
            this.Student_id = 219;
            this.Student_message = "Test";
            this.Gps_lat = 0.0;
            this.Gps_long = 0.0;
        }

        public Message(int id, int student_id, double gps_lat, double gps_long, string student_message)
        {
            Id = id;
            Student_id = student_id;
            Gps_lat = gps_lat;
            Gps_long = gps_long;
            Student_message = student_message;
        }

        public Message(int student_id, double gps_lat, double gps_long, string student_message)
        {
            Student_id = student_id;
            Gps_lat = gps_lat;
            Gps_long = gps_long;
            Student_message = student_message;
        }

        public int Id { get; set; }
        public int Student_id { get; set; }
        public double Gps_lat { get; set; }
        public double Gps_long { get; set; }
        public string Student_message { get; set; }

        public override string ToString()
        {
            return Id + " : ("+Gps_lat+";"+Gps_long+")\t[" + Student_id + "]" + Student_message;
        }
    }
}
