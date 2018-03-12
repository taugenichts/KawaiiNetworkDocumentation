using System.Collections.Generic;
using System.Linq;
using Kawaii.NetworkDocumentation.AppDataService.Managers;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Entities;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Database;
using Kawaii.NetworkDocumentation.AppDataService.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unity;

namespace Kawaii.NetworkDocumentation.AppDataService.Tests.ManagerTests
{
    [TestClass]
    public class ComputerManagerTests
    {
        private readonly Mock<IDatabaseSession> databaseSessionMock = new Mock<IDatabaseSession>();        

        private IEnumerable<Computer> computerDataModel;
        
        private UnityContainer container;

        [TestInitialize]
        public void OneTimeSetup()
        {
            this.computerDataModel = this.RandomizeComputers(5);            

            var container = new UnityContainer();
            container.RegisterInstance<IDatabaseSession>(databaseSessionMock.Object);
            container.RegisterType<IComputerManager, ComputerManager>();

            this.container = container;
        }

        [TestMethod]
        public void GetComputersShouldReturnDtosWithSameInfoAsDataModel()
        {
            this.databaseSessionMock
                .Setup<IEnumerable<Computer>>(x => x.Query<Computer>(It.IsAny<string>(), null))
                .Returns(this.computerDataModel);

            var computerManager = this.container.Resolve<IComputerManager>();
            var computerDtos = computerManager.GetComputers();            

            Assert.AreEqual(computerDtos.Count(), this.computerDataModel.Count());
            foreach(var computerDto in computerDtos)
            {
                var computer = this.computerDataModel.Single(x => x.Id == computerDto.ComputerId);
                Assert.AreEqual(computer.Name, computerDto.Name);
                Assert.AreEqual(computer.ComputerId, computerDto.ComputerId);
                Assert.AreEqual(computer.Inactive, computerDto.Inactive);
                Assert.AreEqual(computer.StaticIp, computerDto.StaticIp);
            }
        }

        private IEnumerable<Computer> RandomizeComputers(int numberOfComputers)
        {
            var computers = new List<Computer>();

            for(int i = 0; i < numberOfComputers; i++)
            {
                var computer = new Computer
                {
                    ComputerId = i + 1,
                    Inactive = Randomize.Bool(),
                    LastModified = Randomize.Date(-10, 0),
                    LastModifiedBy = Randomize.String(20),
                    Name = Randomize.String(30),
                    StaticIp = string.Format("{0}.{1}.{2}.{3}", 
                                                Randomize.Int(1, 255).ToString(), 
                                                Randomize.Int(1, 255).ToString(), 
                                                Randomize.Int(1, 255).ToString(), 
                                                Randomize.Int(1, 255).ToString())
                };

                computers.Add(computer);
            }

            return computers;
        }
    }
    
}
