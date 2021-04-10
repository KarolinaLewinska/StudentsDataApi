using System;

namespace StudentsDataApi.Models
{
    public class StudentData
    {
        public int Id { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public int indexNumber { get; set; }

        public string pesel { get; set; }

        public string email { get; set; }

        public string studiesType { get; set; }

        public string degree { get; set; }

        public string fieldOfStudy { get; set; }

        public string specialization { get; set; }

        public DateTime dateNow = DateTime.Now;
        public DateTime dateModified
        {
            get { return dateNow; }
            set { dateNow = value; }
        }
    }
}
