using System;

namespace Indigox.UUM.Application.DTO
{
    public class SyncTaskDTO
    {
        public int ID { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string ErrorMessage { get; set; }
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ExecuteTime { get; set; }

        public static SyncTaskDTO ConvertToDTO( Indigox.UUM.Sync.Tasks.SyncTask task )
        {
            SyncTaskDTO dto = new SyncTaskDTO();
            dto.ID = task.ID;
            dto.Tag = task.Tag;
            dto.Description = task.Description;
            dto.ErrorMessage = task.ErrorMessage;
            dto.State = (int)task.State;
            dto.CreateTime = task.CreateTime;
            dto.ExecuteTime = task.ExecuteTime;

            return dto;
        }
    }
}