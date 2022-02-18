using System;


namespace FileMover
{
    public class FileOperationInfo
    {
        public string SourceDir { get; set; }

        public string BackupDir { get; set; }

        public string FileCriteria { get; set; }

        public int NumberFilesToGet { get; set; }

        public DateTime FileDate { get; set; }
    }
}
