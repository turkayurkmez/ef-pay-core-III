﻿namespace Movies.DataApplication.Models
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string CommentBody { get; set; } = string.Empty;
        public int MovieId { get; set; }
    }
}
