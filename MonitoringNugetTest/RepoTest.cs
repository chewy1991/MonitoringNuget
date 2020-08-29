using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using MonitoringNuget.EntityFramework;
using MonitoringNuget.IoCContainer;
using MonitoringNuget.LinqDTO;
using MonitoringNuget.Models;
using NUnit.Framework;

namespace MonitoringNugetTest
{
    [TestFixture]
    public class RepoTest
    {
        [Test]
        public void LogginRepository_Count_IsTrue()
        {
            // Arrange
            var IoCContainer = new IoCContainer<IRepositoryBase<VLogentriesDTO>>();
            var returnwert = 8;
            var repomock = IoCContainer.ResolveTestingRepo<VLogentriesDTO>(returnwert);
            // Act
            var bOk = repomock.Count() == returnwert;
            // Assert
            Assert.IsTrue(bOk);
        }

        [Test]
        public void LocationRepository_Count_IsTrue()
        {
            // Arrange
            var IoCContainer = new IoCContainer<IRepositoryBase<Location>>();
            var returnwert = 88;
            var repomock = IoCContainer.ResolveTestingRepo<Location>(returnwert);
            // Act
            var bOk = repomock.Count() == returnwert;
            // Assert
            Assert.IsTrue(bOk);
        }

        [Test]
        public void KundenRepo_Count_IsTrue()
        {
            // Arrange
            var IoCContainer = new IoCContainer<IRepositoryBase<Kunde>>();
            var returnwert = 99;
            var repomock = IoCContainer.ResolveTestingRepo<Kunde>(returnwert);
            // Act
            var bOk = repomock.Count() == returnwert;
            // Assert
            Assert.IsTrue(bOk);
        }

        [Test]
        public void JahresvergleichRepo_Count_IsTrue()
        {
            // Arrange
            var IoCContainer = new IoCContainer<IRepositoryBase<Jahresvergleich>>();
            var returnwert = 188;
            var repomock = IoCContainer.ResolveTestingRepo<Jahresvergleich>(returnwert);
            // Act
            var bOk = repomock.Count() == returnwert;
            // Assert
            Assert.IsTrue(bOk);
        }
    }
}
