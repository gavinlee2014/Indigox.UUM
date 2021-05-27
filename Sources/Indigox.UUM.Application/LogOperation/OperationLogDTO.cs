using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.NHibernateImpl.Model;

namespace Indigox.UUM.Application.LogOperation
{
    public class OperationLogDTO
    {
        public int ID { get; set; }
        public string Operator { get; set; }
        public string Operation { get; set; }
        public DateTime OperationTime { get; set; }
        public string DetailInformation { get; set; }

        public static OperationLogDTO ConvertToDTO(OperationLog item)
        {
            OperationLogDTO dto = new OperationLogDTO();
            dto.ID = item.ID;
            dto.Operator = item.Operator;
            dto.Operation = item.Operation;
            dto.OperationTime = item.OperationTime;
            dto.DetailInformation = item.DetailInformation.Replace("，","\r\n");
            
            return dto;
        }

        public static IList<OperationLogDTO> ConvertToDTOs(IList<OperationLog> items) {
            
            IList<OperationLogDTO> dtos = new List<OperationLogDTO>();
            
            foreach (var item in items)
            {
                dtos.Add(ConvertToDTO(item));   
            }

            return dtos;
        }
    }
}
