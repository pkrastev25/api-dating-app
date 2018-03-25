﻿using System;

namespace api_dating_app.DTOs
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class PhotoForDetailDto
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsMain { get; set; }
    }
}