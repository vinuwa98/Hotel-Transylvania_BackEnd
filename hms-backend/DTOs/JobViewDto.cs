using HmsBackend.DTOs;
using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.DTOs
{
    public class JobViewDto
    {
     
        public string Id { get; set; }
        public string? JobNumber { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public string CreatedUserId { get; set; }
        public string? AssignedManagerUserId { get; set; }

        
        public required string ComplaintId { get; set; }
        public List<JobUserViewDto> Users { get; set; } = new();
        public string? AssignedManagerName { get; set; }

    }
}

