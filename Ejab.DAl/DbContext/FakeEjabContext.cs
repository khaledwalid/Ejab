// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.5
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace Ejab.DAl
{

    using System.Linq;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class FakeEjabContext : IEjabContext
    {
        public System.Data.Entity.DbSet<AcceptOffer> AcceptOffers { get; set; }
        public System.Data.Entity.DbSet<Device> Devices { get; set; }
        public System.Data.Entity.DbSet<Interest> Interests { get; set; }
        public System.Data.Entity.DbSet<Message> Messages { get; set; }
        public System.Data.Entity.DbSet<Offer> Offers { get; set; }
        public System.Data.Entity.DbSet<OfferDetail> OfferDetails { get; set; }
        public System.Data.Entity.DbSet<OfferImage> OfferImages { get; set; }
        public System.Data.Entity.DbSet<PredefinedAction> PredefinedActions { get; set; }
        public System.Data.Entity.DbSet<ProposalPrice> ProposalPrices { get; set; }
        public System.Data.Entity.DbSet<Rating> Ratings { get; set; }
        public System.Data.Entity.DbSet<Request> Requests { get; set; }
        public System.Data.Entity.DbSet<RequestDetaile> RequestDetailes { get; set; }
        public System.Data.Entity.DbSet<RequestDetailesPrice> RequestDetailesPrices { get; set; }
        public System.Data.Entity.DbSet<ServiceType> ServiceTypes { get; set; }
        public System.Data.Entity.DbSet<SuggestionsComplaint> SuggestionsComplaints { get; set; }
        public System.Data.Entity.DbSet<SysLog> SysLogs { get; set; }
        public System.Data.Entity.DbSet<Truck> Trucks { get; set; }
        public System.Data.Entity.DbSet<TruckType> TruckTypes { get; set; }
        public System.Data.Entity.DbSet<User> Users { get; set; }
        public System.Data.Entity.DbSet<UserDevice> UserDevices { get; set; }

        public FakeEjabContext()
        {
            AcceptOffers = new FakeDbSet<AcceptOffer>("Id");
            Devices = new FakeDbSet<Device>("Id");
            Interests = new FakeDbSet<Interest>("Id");
            Messages = new FakeDbSet<Message>("Id");
            Offers = new FakeDbSet<Offer>("Id");
            OfferDetails = new FakeDbSet<OfferDetail>("Id");
            OfferImages = new FakeDbSet<OfferImage>("Id");
            PredefinedActions = new FakeDbSet<PredefinedAction>("Id");
            ProposalPrices = new FakeDbSet<ProposalPrice>("Id");
            Ratings = new FakeDbSet<Rating>("Id");
            Requests = new FakeDbSet<Request>("Id");
            RequestDetailes = new FakeDbSet<RequestDetaile>("Id");
            RequestDetailesPrices = new FakeDbSet<RequestDetailesPrice>("Id");
            ServiceTypes = new FakeDbSet<ServiceType>("Id");
            SuggestionsComplaints = new FakeDbSet<SuggestionsComplaint>("Id");
            SysLogs = new FakeDbSet<SysLog>("Id");
            Trucks = new FakeDbSet<Truck>("Id");
            TruckTypes = new FakeDbSet<TruckType>("Id");
            Users = new FakeDbSet<User>("Id");
            UserDevices = new FakeDbSet<UserDevice>("Id");

            InitializePartial();
        }

        public int SaveChangesCount { get; private set; }
        public int SaveChanges()
        {
            ++SaveChangesCount;
            return 1;
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            ++SaveChangesCount;
            return System.Threading.Tasks.Task<int>.Factory.StartNew(() => 1);
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            ++SaveChangesCount;
            return System.Threading.Tasks.Task<int>.Factory.StartNew(() => 1, cancellationToken);
        }

        partial void InitializePartial();

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public System.Data.Entity.Infrastructure.DbChangeTracker _changeTracker;
        public System.Data.Entity.Infrastructure.DbChangeTracker ChangeTracker { get { return _changeTracker; } }
        public System.Data.Entity.Infrastructure.DbContextConfiguration _configuration;
        public System.Data.Entity.Infrastructure.DbContextConfiguration Configuration { get { return _configuration; } }
        public System.Data.Entity.Database _database;
        public System.Data.Entity.Database Database { get { return _database; } }
        public System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity)
        {
            throw new System.NotImplementedException();
        }
        public System.Collections.Generic.IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> GetValidationErrors()
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.DbSet Set(System.Type entityType)
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            throw new System.NotImplementedException();
        }
        public override string ToString()
        {
            throw new System.NotImplementedException();
        }

    }
}
// </auto-generated>
