using Microsoft.EntityFrameworkCore; 
using System; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
using dotnet_2.Infrastructure.Data.Models;

    namespace dotnet_2.Infrastructure.Dto{

    public class OrganizationDto
    {   
        public int id { get; set; }
        public string? organization_name { get; set; }
        public OrganizationDto() { }
        public OrganizationDto(Organization organizationItem) =>
        (id, organization_name) = (organizationItem.id, organizationItem.organization_name);
    }
    }