using AutoMapper;
using Codebridge.Controllers;
using Codebridge.Domain.IServices.Communication;
using Codebridge.Domain.Models;
using Codebridge.Domain.Models.TechModels;
using Codebridge.Domain.Services;
using Codebridge.Mapping;
using Codebridge.Resources;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class DogsControllerTests
    {
        private static IMapper _mapper;

        public DogsControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ModelToResourceProfile());
                    mc.AddProfile(new ResourceToModelProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void GetDogsTest()
        {
            Sort_Pagination sort = new Sort_Pagination();
           
            var mockRepo = new Mock<IDogsService>();
            mockRepo.Setup(repo => repo.GetDogsAsync(sort)).Returns(TestRecords());
            var controller = new DogsController(mockRepo.Object,_mapper) ;

          
            var result = controller.GetDogsAsync(sort);

            
            var model = Assert.IsAssignableFrom<Task<IEnumerable<GetDogsResource>>>(result);
            Assert.Equal(3, model.Result.Count());
        }

        private static async Task<IEnumerable<Dog>> TestRecords()
        {
            var dogs = new List<Dog>();
            dogs.Add(new Dog()
            {
                Id = 1,
                Name = "Test1",
                Color = "black",
                Tail_Length = 1,
                Weight=1
            });
            dogs.Add(new Dog()
            {
                Id = 1,
                Name = "Test2",
                Color = "red",
                Tail_Length = 2,
                Weight = 2
            });
            dogs.Add(new Dog()
            {
                Id = 1,
                Name = "Test3",
                Color = "gray",
                Tail_Length = 3,
                Weight = 3
            });
            return await Task.FromResult(dogs);
        }


        [Fact]
        public void Test_POST_AddReservation()
        {
          
            SaveDogResource dog_t = new SaveDogResource()
            {            
                Name = "TestAdd",
                Color = "black",
                Tail_Length = 1,
                Weight = 1
            };
            var dog = _mapper.Map<SaveDogResource, Dog>(dog_t);
            var mockRepo = new Mock<IDogsService>();
            mockRepo.Setup(repo => repo.SaveDogAsync(It.IsAny<Dog>())).Returns(Task.FromResult(new SaveDogResponse(dog)));
            var controller = new DogsController(mockRepo.Object,_mapper);

            
            var result = controller.SaveDogAsync(dog_t).Result;

                    
            var dog_result = Assert.IsAssignableFrom<IActionResult>(result );
            OkObjectResult model = Assert.IsType<OkObjectResult>(dog_result);

            Assert.NotNull(model);
            Assert.Equal(200, model.StatusCode);

            Assert.True(model.Value.ToString() == dog_t.ToString());




        }
    }
}
