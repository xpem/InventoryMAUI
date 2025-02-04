using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class ApiOperationDTO
    {
        [Key]
        public int Id { get; set; }

        public required string Content { get; set; }

        public required ObjectType ObjectType { get; set; }

        public required ExecutionType ExecutionType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ApiOperationStatus Status { get; set; }

        public required string ObjectId { get; set; }
    }

    public enum ApiOperationStatus { Pending, Processing, Success, Failure }

    public enum ObjectType { SubCategory }

    public enum ExecutionType { Insert, Update, Delete }
}
