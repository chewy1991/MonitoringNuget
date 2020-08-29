using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using GenericRepository;
using MonitoringNuget.DataAccess.EFAccess;
using MonitoringNuget.DataAccess.LinqToSQL;
using MonitoringNuget.DataAccess.RepositoryClasses;
using MonitoringNuget.DataAccess.StoredProcedures;
using MonitoringNuget.DataAccess.StoredProcedures.Interface;
using MonitoringNuget.EntityFramework;
using MonitoringNuget.LinqDTO;
using MonitoringNuget.Models;
using MonitoringNuget.ViewModel;
using Moq;

namespace MonitoringNuget.IoCContainer
{
    public class IoCContainer<T>
    {
        private ContainerBuilder builder;
        private IContainer container;

        public T ResolveViewModel()
        {
            BuilViewModel();
            return container.Resolve<T>();
        }

        public T ResolveContextStrategy<MRepo, MStoredProc>()
        {
            BuildContextStrategy<MRepo, MStoredProc>();
            return container.Resolve<T>();
        }

        public T ResolveRepo()
        {
            builder = new ContainerBuilder();
            RegisterRepoType<T>();
            container = builder.Build();
            return container.Resolve<T>();
        }
        
        private void BuilViewModel()
        {
            builder = new ContainerBuilder();
            builder.RegisterType(typeof(T));
            container = builder.Build();
        }

        private void BuildContextStrategy<MRepo, MStoredProc>()
        {
            builder = new ContainerBuilder();
            builder.RegisterType(typeof(T));
            RegisterRepoType<MRepo>();
            builder.RegisterType(typeof(MStoredProc)).As<IStoredProcedures>();
            container = builder.Build();
        }

        private void RegisterRepoType<M>()
        {
            if (typeof(M) == typeof(LoggingRepository) || typeof(M) == typeof(LogentriesRepositoryLinq))
                builder.RegisterType(typeof(M)).As<IRepositoryBase<VLogentriesDTO>>();
            if (typeof(M) == typeof(LocationRepository))
                builder.RegisterType(typeof(M)).As<IRepositoryBase<Location>>();
            if (typeof(M) == typeof(LocationsRepositoryLinq))
                builder.RegisterType(typeof(M)).As<IRepositoryBase<LocationDTO>>();
            if (typeof(M) == typeof(KundenRepository))
                builder.RegisterType(typeof(M)).As<IRepositoryBase<Kunde>>();
            if (typeof(M) == typeof(JahresvergleichRepo))
                builder.RegisterType(typeof(M)).As<IRepositoryBase<Jahresvergleich>>();
        }

        public T ResolveTestingRepo<M>(int countReturn)
        {
            builder = new ContainerBuilder();
            var mock = new Mock<IRepositoryBase<M>>();
            mock.Setup(foo => foo.Count()).Returns(countReturn);
            builder.RegisterInstance(mock.Object).As<T>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("ViewModel"));
            container = builder.Build();
            return container.Resolve<T>();
        }
    }
}
