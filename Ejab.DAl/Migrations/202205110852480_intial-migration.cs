namespace Ejab.DAl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intialmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AboutApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AboutApp = c.String(nullable: false),
                        AboutAppEng = c.String(nullable: false),
                        AppLink = c.String(nullable: false),
                        FaceBookLink = c.String(nullable: false),
                        TwitterLink = c.String(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AboutUs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Region = c.String(nullable: false),
                        PostalCode = c.String(),
                        Address = c.String(nullable: false),
                        Longitude = c.Double(nullable: false),
                        latitude = c.Double(nullable: false),
                        phone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        fax = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AcceptOffers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferId = c.Int(nullable: false),
                        AcceptedUserId = c.Int(nullable: false),
                        AcceptedDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        OfferState = c.Int(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offer", t => t.OfferId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.AcceptedUserId, cascadeDelete: true)
                .Index(t => t.OfferId)
                .Index(t => t.AcceptedUserId);
            
            CreateTable(
                "dbo.Offer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferNumber = c.Long(nullable: false),
                        OfferDate = c.DateTime(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 250),
                        Description = c.String(nullable: false, maxLength: 500),
                        Address = c.String(maxLength: 250),
                        AdressLatitude = c.Decimal(precision: 18, scale: 6),
                        AddressLongitude = c.Decimal(precision: 18, scale: 6),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        quantity = c.String(nullable: false, maxLength: 4000),
                        Period = c.String(),
                        IsDiscount = c.Boolean(),
                        DiscountPecent = c.Double(),
                        DiscountAmount = c.Decimal(storeType: "money"),
                        IsActive = c.Boolean(),
                        UserId = c.Int(),
                        ExpireDate = c.DateTime(storeType: "date"),
                        MaxCustomerNumbers = c.Int(),
                        MaxExpireDate = c.DateTime(storeType: "date"),
                        TruckTypeId = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        RegionId = c.Int(),
                        Notes = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionId)
                .ForeignKey("dbo.TruckType", t => t.TruckTypeId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TruckTypeId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        SendingTime = c.String(),
                        Title = c.String(nullable: false, maxLength: 500),
                        Description = c.String(nullable: false, maxLength: 500),
                        Status = c.Boolean(),
                        SenderId = c.Int(nullable: false),
                        ReciverId = c.Int(nullable: false),
                        RequestId = c.Int(),
                        OfferId = c.Int(),
                        MessageType = c.Int(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offer", t => t.OfferId)
                .ForeignKey("dbo.User", t => t.ReciverId)
                .ForeignKey("dbo.Request", t => t.RequestId)
                .ForeignKey("dbo.User", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReciverId)
                .Index(t => t.RequestId)
                .Index(t => t.OfferId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 14),
                        ProfileImgPath = c.String(),
                        ProfileName = c.String(),
                        RegisteredBy = c.Int(nullable: false),
                        FaceBookId = c.String(maxLength: 50),
                        Address = c.String(maxLength: 100),
                        CustomerType = c.Int(nullable: false),
                        ResponsiblePerson = c.String(maxLength: 50),
                        IsAdmin = c.Boolean(),
                        OverAllrating = c.Decimal(precision: 18, scale: 4),
                        AddressLatitude = c.Decimal(precision: 18, scale: 6),
                        AddressLongitude = c.Decimal(precision: 18, scale: 6),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        UserToken_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserTokens", t => t.UserToken_Id)
                .Index(t => t.UserToken_Id);
            
            CreateTable(
                "dbo.proposalPrice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReqestId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        price = c.Decimal(nullable: false, storeType: "money"),
                        ServiceProviderId = c.Int(nullable: false),
                        IsAccepted = c.Boolean(),
                        ExpireDate = c.DateTime(),
                        PropsalStatus = c.Int(nullable: false),
                        Comments = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Request", t => t.ReqestId)
                .ForeignKey("dbo.User", t => t.ServiceProviderId)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.ReqestId)
                .Index(t => t.ServiceProviderId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequesterId = c.Int(nullable: false),
                        RequestNumber = c.Int(nullable: false),
                        Requestdate = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        LocationFromlongitude = c.Decimal(precision: 9, scale: 6),
                        locationFromLatitude = c.Decimal(precision: 9, scale: 6),
                        LocationFrom = c.String(maxLength: 250),
                        LocationToLongitude = c.Decimal(precision: 9, scale: 6),
                        LocationToLatitude = c.Decimal(precision: 9, scale: 6),
                        LocationTo = c.String(nullable: false, maxLength: 250),
                        RequestState = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        IsAccepted = c.Boolean(),
                        StartingDate = c.DateTime(nullable: false),
                        Period = c.String(),
                        Quantity = c.String(),
                        ItemsInfo = c.String(),
                        RegionId = c.Int(),
                        Notes = c.String(),
                        RequestType = c.Int(nullable: false),
                        PermissionDate = c.DateTime(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionId)
                .ForeignKey("dbo.User", t => t.RequesterId)
                .Index(t => t.RequesterId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceProviderId = c.Int(nullable: false),
                        RatingValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceRequestId = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                        Date = c.DateTime(),
                        RequstId = c.Int(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Request", t => t.RequstId)
                .ForeignKey("dbo.User", t => t.ServiceProviderId)
                .ForeignKey("dbo.User", t => t.ServiceRequestId)
                .Index(t => t.ServiceProviderId)
                .Index(t => t.ServiceRequestId)
                .Index(t => t.RequstId);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        parantId = c.Int(),
                        NameArb = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        Region_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TruckId = c.Int(),
                        Date = c.DateTime(nullable: false),
                        Notes = c.String(),
                        RegionId = c.Int(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionId)
                .ForeignKey("dbo.Trucks", t => t.TruckId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TruckId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Trucks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        NameArb = c.String(),
                        Weight = c.Int(),
                        TypeId = c.Int(nullable: false),
                        IsOcuppied = c.Boolean(),
                        AvialableNo = c.Int(),
                        Capacity = c.Decimal(precision: 18, scale: 6),
                        Description = c.String(maxLength: 250),
                        Width = c.Int(),
                        height = c.Int(),
                        ParanetId = c.Int(),
                        TruckImagePath = c.String(),
                        TruckImgeName = c.String(maxLength: 100),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trucks", t => t.ParanetId)
                .ForeignKey("dbo.TruckType", t => t.TypeId)
                .Index(t => t.TypeId)
                .Index(t => t.ParanetId);
            
            CreateTable(
                "dbo.OfferDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferId = c.Int(nullable: false),
                        TruckId = c.Int(nullable: false),
                        NumberOfTrucks = c.Int(nullable: false),
                        User_Id = c.Int(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offer", t => t.OfferId)
                .ForeignKey("dbo.Trucks", t => t.TruckId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.OfferId)
                .Index(t => t.TruckId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.RequestDetaile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        TruckId = c.Int(nullable: false),
                        NumberOfTrucks = c.Int(nullable: false),
                        Notes = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Request", t => t.RequestId)
                .ForeignKey("dbo.Trucks", t => t.TruckId)
                .Index(t => t.RequestId)
                .Index(t => t.TruckId);
            
            CreateTable(
                "dbo.RequestDetailesPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestDetaileId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceProviderId = c.Int(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestDetaile", t => t.RequestDetaileId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.ServiceProviderId, cascadeDelete: true)
                .Index(t => t.RequestDetaileId)
                .Index(t => t.ServiceProviderId);
            
            CreateTable(
                "dbo.TruckType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameArb = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DeviceType = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Seen = c.Boolean(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        ReciverUser_Id = c.Int(),
                        SenderUser_Id = c.Int(),
                        User_Id = c.Int(),
                        User_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ReciverUser_Id)
                .ForeignKey("dbo.User", t => t.SenderUser_Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .ForeignKey("dbo.User", t => t.User_Id1)
                .Index(t => t.ReciverUser_Id)
                .Index(t => t.SenderUser_Id)
                .Index(t => t.User_Id)
                .Index(t => t.User_Id1);
            
            CreateTable(
                "dbo.UserDevices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DeviceId = c.Int(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SerialNumber = c.String(maxLength: 200),
                        DeviceToken = c.String(nullable: false, maxLength: 500),
                        DeviceType = c.String(nullable: false, maxLength: 100),
                        UserDevice_Id = c.Int(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDevices", t => t.UserDevice_Id)
                .Index(t => t.UserDevice_Id);
            
            CreateTable(
                "dbo.Rules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DescriptionEng = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommonQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionArb = c.String(nullable: false),
                        AnswerArb = c.String(nullable: false),
                        QuestionEng = c.String(),
                        AnswerEng = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailSubscribes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OfferImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferId = c.Int(nullable: false),
                        ImageTitle = c.String(),
                        ImageDescription = c.String(),
                        ImageUrl = c.String(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offer", t => t.OfferId, cascadeDelete: true)
                .Index(t => t.OfferId);
            
            CreateTable(
                "dbo.PredefinedActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.Int(),
                        CreatedOn = c.DateTime(),
                        FlgStatus = c.Short(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SysLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionId = c.Int(),
                        Description = c.String(),
                        CreatedBy = c.Int(),
                        CreatedOn = c.DateTime(),
                        FlgStatus = c.Short(),
                        PredefinedAction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PredefinedActions", t => t.PredefinedAction_Id)
                .Index(t => t.PredefinedAction_Id);
            
            CreateTable(
                "dbo.ServiceType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdminPeriod = c.DateTime(nullable: false),
                        ExpirDayies = c.Int(nullable: false),
                        MaxExpirDayies = c.Int(nullable: false),
                        MaxAcceptNo = c.Int(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrucksOrdersNo = c.Int(nullable: false),
                        CustomerNo = c.Int(nullable: false),
                        OfferNo = c.Int(nullable: false),
                        AppDownloadsNo = c.Int(nullable: false),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuggestionsComplaint",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Cause = c.String(nullable: false, maxLength: 500),
                        ComplaintStatus = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 200),
                        Phone = c.String(nullable: false, maxLength: 100),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Fcmtoken = c.String(),
                        FlgStatus = c.Short(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RuleUsers",
                c => new
                    {
                        Rule_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Rule_Id, t.User_Id })
                .ForeignKey("dbo.Rules", t => t.Rule_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Rule_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "UserToken_Id", "dbo.UserTokens");
            DropForeignKey("dbo.SysLogs", "PredefinedAction_Id", "dbo.PredefinedActions");
            DropForeignKey("dbo.OfferImages", "OfferId", "dbo.Offer");
            DropForeignKey("dbo.AcceptOffers", "AcceptedUserId", "dbo.User");
            DropForeignKey("dbo.AcceptOffers", "OfferId", "dbo.Offer");
            DropForeignKey("dbo.Offer", "UserId", "dbo.User");
            DropForeignKey("dbo.Offer", "TruckTypeId", "dbo.TruckType");
            DropForeignKey("dbo.Message", "SenderId", "dbo.User");
            DropForeignKey("dbo.Message", "RequestId", "dbo.Request");
            DropForeignKey("dbo.Message", "ReciverId", "dbo.User");
            DropForeignKey("dbo.RuleUsers", "User_Id", "dbo.User");
            DropForeignKey("dbo.RuleUsers", "Rule_Id", "dbo.Rules");
            DropForeignKey("dbo.UserDevices", "UserId", "dbo.User");
            DropForeignKey("dbo.Devices", "UserDevice_Id", "dbo.UserDevices");
            DropForeignKey("dbo.Notifications", "User_Id1", "dbo.User");
            DropForeignKey("dbo.Notifications", "User_Id", "dbo.User");
            DropForeignKey("dbo.Notifications", "SenderUser_Id", "dbo.User");
            DropForeignKey("dbo.Notifications", "ReciverUser_Id", "dbo.User");
            DropForeignKey("dbo.proposalPrice", "User_Id", "dbo.User");
            DropForeignKey("dbo.proposalPrice", "ServiceProviderId", "dbo.User");
            DropForeignKey("dbo.proposalPrice", "ReqestId", "dbo.Request");
            DropForeignKey("dbo.Request", "RequesterId", "dbo.User");
            DropForeignKey("dbo.Request", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Regions", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.Offer", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Interests", "UserId", "dbo.User");
            DropForeignKey("dbo.Interests", "TruckId", "dbo.Trucks");
            DropForeignKey("dbo.Trucks", "TypeId", "dbo.TruckType");
            DropForeignKey("dbo.RequestDetaile", "TruckId", "dbo.Trucks");
            DropForeignKey("dbo.RequestDetailesPrices", "ServiceProviderId", "dbo.User");
            DropForeignKey("dbo.RequestDetailesPrices", "RequestDetaileId", "dbo.RequestDetaile");
            DropForeignKey("dbo.RequestDetaile", "RequestId", "dbo.Request");
            DropForeignKey("dbo.Trucks", "ParanetId", "dbo.Trucks");
            DropForeignKey("dbo.OfferDetails", "User_Id", "dbo.User");
            DropForeignKey("dbo.OfferDetails", "TruckId", "dbo.Trucks");
            DropForeignKey("dbo.OfferDetails", "OfferId", "dbo.Offer");
            DropForeignKey("dbo.Interests", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Rating", "ServiceRequestId", "dbo.User");
            DropForeignKey("dbo.Rating", "ServiceProviderId", "dbo.User");
            DropForeignKey("dbo.Rating", "RequstId", "dbo.Request");
            DropForeignKey("dbo.Message", "OfferId", "dbo.Offer");
            DropIndex("dbo.RuleUsers", new[] { "User_Id" });
            DropIndex("dbo.RuleUsers", new[] { "Rule_Id" });
            DropIndex("dbo.SysLogs", new[] { "PredefinedAction_Id" });
            DropIndex("dbo.OfferImages", new[] { "OfferId" });
            DropIndex("dbo.Devices", new[] { "UserDevice_Id" });
            DropIndex("dbo.UserDevices", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "User_Id1" });
            DropIndex("dbo.Notifications", new[] { "User_Id" });
            DropIndex("dbo.Notifications", new[] { "SenderUser_Id" });
            DropIndex("dbo.Notifications", new[] { "ReciverUser_Id" });
            DropIndex("dbo.RequestDetailesPrices", new[] { "ServiceProviderId" });
            DropIndex("dbo.RequestDetailesPrices", new[] { "RequestDetaileId" });
            DropIndex("dbo.RequestDetaile", new[] { "TruckId" });
            DropIndex("dbo.RequestDetaile", new[] { "RequestId" });
            DropIndex("dbo.OfferDetails", new[] { "User_Id" });
            DropIndex("dbo.OfferDetails", new[] { "TruckId" });
            DropIndex("dbo.OfferDetails", new[] { "OfferId" });
            DropIndex("dbo.Trucks", new[] { "ParanetId" });
            DropIndex("dbo.Trucks", new[] { "TypeId" });
            DropIndex("dbo.Interests", new[] { "RegionId" });
            DropIndex("dbo.Interests", new[] { "TruckId" });
            DropIndex("dbo.Interests", new[] { "UserId" });
            DropIndex("dbo.Regions", new[] { "Region_Id" });
            DropIndex("dbo.Rating", new[] { "RequstId" });
            DropIndex("dbo.Rating", new[] { "ServiceRequestId" });
            DropIndex("dbo.Rating", new[] { "ServiceProviderId" });
            DropIndex("dbo.Request", new[] { "RegionId" });
            DropIndex("dbo.Request", new[] { "RequesterId" });
            DropIndex("dbo.proposalPrice", new[] { "User_Id" });
            DropIndex("dbo.proposalPrice", new[] { "ServiceProviderId" });
            DropIndex("dbo.proposalPrice", new[] { "ReqestId" });
            DropIndex("dbo.User", new[] { "UserToken_Id" });
            DropIndex("dbo.Message", new[] { "OfferId" });
            DropIndex("dbo.Message", new[] { "RequestId" });
            DropIndex("dbo.Message", new[] { "ReciverId" });
            DropIndex("dbo.Message", new[] { "SenderId" });
            DropIndex("dbo.Offer", new[] { "RegionId" });
            DropIndex("dbo.Offer", new[] { "TruckTypeId" });
            DropIndex("dbo.Offer", new[] { "UserId" });
            DropIndex("dbo.AcceptOffers", new[] { "AcceptedUserId" });
            DropIndex("dbo.AcceptOffers", new[] { "OfferId" });
            DropTable("dbo.RuleUsers");
            DropTable("dbo.UserTokens");
            DropTable("dbo.SuggestionsComplaint");
            DropTable("dbo.Statistics");
            DropTable("dbo.Settings");
            DropTable("dbo.ServiceType");
            DropTable("dbo.SysLogs");
            DropTable("dbo.PredefinedActions");
            DropTable("dbo.OfferImages");
            DropTable("dbo.MailSubscribes");
            DropTable("dbo.CommonQuestions");
            DropTable("dbo.Rules");
            DropTable("dbo.Devices");
            DropTable("dbo.UserDevices");
            DropTable("dbo.Notifications");
            DropTable("dbo.TruckType");
            DropTable("dbo.RequestDetailesPrices");
            DropTable("dbo.RequestDetaile");
            DropTable("dbo.OfferDetails");
            DropTable("dbo.Trucks");
            DropTable("dbo.Interests");
            DropTable("dbo.Regions");
            DropTable("dbo.Rating");
            DropTable("dbo.Request");
            DropTable("dbo.proposalPrice");
            DropTable("dbo.User");
            DropTable("dbo.Message");
            DropTable("dbo.Offer");
            DropTable("dbo.AcceptOffers");
            DropTable("dbo.AboutUs");
            DropTable("dbo.AboutApplications");
        }
    }
}
