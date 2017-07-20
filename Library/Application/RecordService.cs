using System;
using AutoMapper;
using Music.Application.IService;
using Music.Domain.Entities;
using Music.Domain.IRepositories;
using Music.Domain.Factories;
using Music.Helper;

namespace Music.Application
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository RecordRepository;
        private readonly IMapper Mapper;

        public RecordService(IRecordRepository recordRepository, IMapper mapper)
        {
            RecordRepository = recordRepository;
            Mapper = mapper;
        }

        public void DeleteRecord(long id)
        {
            throw new NotImplementedException();
        }
    }
}