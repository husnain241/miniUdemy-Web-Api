﻿namespace MiniUdemy.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string Content { get; set; } // optional text/pdf link

        // Foreign Key
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
