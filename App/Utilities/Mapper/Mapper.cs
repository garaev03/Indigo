using AutoMapper;
using Indigo.Entities.Concrets;
using Indigo.Entities.DTOs.PostDtos;
using System.Runtime.InteropServices;

namespace Indigo.Utilities.Mapper
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Post, PostGetDto>();
        }
    }
}
