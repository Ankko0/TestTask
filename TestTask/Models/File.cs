using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public Guid UserId { set; get; }
        public User User { set; get; }
    }
}
