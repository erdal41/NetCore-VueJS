﻿namespace VueJS.Entities.Dtos
{
    public class FileUploadDto
    {
        public string FileFullName { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}