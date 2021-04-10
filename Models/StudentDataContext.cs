using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsDataApi.Models
{
    public class StudentDataContext : DbContext
    {
        public DbSet<StudentData> Students { get; set; }

        public StudentDataContext(DbContextOptions<StudentDataContext> options) : base(options)
        {
        }
    }
}
