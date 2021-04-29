create table Chat
(
    QuoteRequestElementId int                                not null,
    MessageId             uniqueidentifier default newid(),
    Message               varchar(500)                       not null,
    Link                  varchar(1000),
    Created               datetime         default getdate() not null,
    SequenceNumber        int identity,
    OwnerId               int                                not null,
    ReceiverId            int                                not null
)
go

exec sp_addextendedproperty 'MS_Description', 'message interchange about responses', 'SCHEMA', 'dbo', 'TABLE', 'Chat'
go

create unique index Chat_MessageId_uindex
    on Chat (MessageId)
go

create unique index Chat_SequenceNumber_uindex
    on Chat (SequenceNumber)
go

create table MetaDataName
(
    Id   int identity
        constraint MetaDataName_pk
            primary key nonclustered,
    Name varchar(50)
)
go

create table MetaData
(
    TableName varchar(255) not null,
    KeyId     int          not null,
    Type      varchar(50) default 'String',
    Value     varchar(500),
    NameId    int          not null
        constraint MetaData_MetaDataName_Id_fk
            references MetaDataName,
    Created   datetime    default getdate(),
    Modified  datetime    default getdate(),
    constraint MetaData_pk
        primary key nonclustered (TableName, KeyId, NameId)
)
go

exec sp_addextendedproperty 'MS_Description', 'General meta data storage', 'SCHEMA', 'dbo', 'TABLE', 'MetaData'
go

create table QuoteElementType
(
    Id    int identity
        constraint QuoteElementType_pk
            primary key nonclustered,
    Name  varchar(255) not null,
    Notes varchar(max),
    Link  varchar(1000)
)
go

exec sp_addextendedproperty 'MS_Description', 'Type of the quote element', 'SCHEMA', 'dbo', 'TABLE', 'QuoteElementType'
go

create table QuoteElement
(
    Id                      int identity
        constraint QuoteElement_pk
            primary key nonclustered,
    Name                    varchar(500),
    Notes                   varchar(max),
    BudgetTolerance         float default 10 not null,
    Quantity                int   default 1,
    ElementTypeId           int              not null
        constraint QuoteElement_QuoteElementType_Id_fk
            references QuoteElementType,
    InheritTopLevelQuantity bit   default 0  not null,
    LeadWeeks               int
)
go

exec sp_addextendedproperty 'MS_Description', 'Elements that go to make up a quote request', 'SCHEMA', 'dbo', 'TABLE',
     'QuoteElement'
go

create unique index QuoteElement_Id_uindex
    on QuoteElement (Id)
go

create unique index QuoteElementType_Id_uindex
    on QuoteElementType (Id)
go

create table QuoteSubType
(
    id       int identity (0, 1)
        constraint QuoteSubType_pk
            unique,
    Name     varchar(255),
    Created  datetime default getdate(),
    Modified datetime default getdate(),
    Notes    varchar(max),
    Link     varchar(500)
)
go

exec sp_addextendedproperty 'MS_Description', 'Sub types', 'SCHEMA', 'dbo', 'TABLE', 'QuoteSubType'
go

create table QuoteType
(
    id       int identity (0, 1)
        constraint QuoteType_pk
            unique,
    Name     nvarchar(255),
    Created  datetime default getdate(),
    Modified datetime default getdate(),
    Notes    varchar(max),
    Link     varchar(500)
)
go

exec sp_addextendedproperty 'MS_Description', 'Types of quotes on the system', 'SCHEMA', 'dbo', 'TABLE', 'QuoteType'
go

create table QuoteRequest
(
    QuoteId         uniqueidentifier default newid()   not null
        constraint QuoteRequest_pk
            primary key nonclustered,
    Name            varchar(500)                       not null,
    Notes           varchar(max),
    QuoteTypeId     int                                not null
        constraint QuoteRequest_QuoteType_id_fk
            references QuoteType (id),
    Created         datetime         default getdate() not null,
    Modified        datetime         default getdate() not null,
    QuoteSubTypeId  int,
    Owner           int                                not null,
    Attendees       int              default 1         not null,
    QuoteIdIdentity int identity,
    DueDate         datetime
)
go

create unique index QuoteRequest_QuoteId_uindex
    on QuoteRequest (QuoteId)
go

create table QuoteRequestElement
(
    QuoteId               uniqueidentifier           not null
        constraint QuoteRequestElement_QuoteRequest_QuoteId_fk
            references QuoteRequest,
    QuoteElementId        int                        not null
        constraint QuoteRequestElement_QuoteElement_Id_fk
            references QuoteElement,
    QuoteRequestElementId int identity
        constraint QuoteRequestElement_pk_2
            primary key nonclustered,
    Budget                money,
    BudgetTolerance       float,
    Quantity              int,
    Created               datetime default getdate() not null,
    Modified              datetime default getdate() not null,
    Submitted             datetime,
    Exclude               bit      default 0         not null,
    Notes                 varchar(1000),
    LeadWeeks             int,
    DueDate               datetime,
    Completed             bit      default 0         not null,
    constraint QuoteRequestElement_pk
        unique (QuoteId, QuoteElementId)
)
go

create table QuoteRequestElementMetaData
(
    QuoteRequestElementId int not null
        constraint QuoteRequestElementMetaData_QuoteRequestElement_QuoteRequestElementId_fk
            references QuoteRequestElement,
    MetaDataNameId        int not null
        constraint QuoteRequestElementMetaData_MetaDataName_Id_fk
            references MetaDataName,
    Value                 varchar(1000),
    constraint QuoteRequestElementMetaData_pk
        primary key nonclustered (QuoteRequestElementId, MetaDataNameId)
)
go

create table QuoteTemplate
(
    Id             int identity
        constraint QuoteTemplate_pk
            primary key nonclustered,
    Name           varchar(500)               not null,
    Notes          varchar(max),
    QuoteTypeId    int                        not null
        constraint QuoteTemplate_QuoteType_id_fk
            references QuoteType (id),
    Created        datetime default getdate() not null,
    Modified       datetime default getdate() not null,
    QuoteSubTypeId int
)
go

create unique index QuoteTemplate_Id_uindex
    on QuoteTemplate (Id)
go

create table QuoteTemplateElement
(
    TemplateId int not null
        constraint QuoteTemplateElement_QuoteTemplate_Id_fk
            references QuoteTemplate,
    ElementId  int not null
        constraint QuoteTemplateElement_QuoteElement_Id_fk
            references QuoteElement,
    constraint QuoteTemplateElement_pk
        primary key nonclustered (TemplateId, ElementId)
)
go

create table QuoteTypeSubType
(
    QuoteTypeId    int
        constraint QuoteTypeSubType_QuoteType_id_fk
            references QuoteType (id),
    QuoteSubTypeId int
        constraint QuoteTypeSubType_QuoteSubType_id_fk
            references QuoteSubType (id),
    constraint QuoteTypeSubType_pk
        unique (QuoteTypeId, QuoteSubTypeId)
)
go

exec sp_addextendedproperty 'MS_Description', 'Linking table', 'SCHEMA', 'dbo', 'TABLE', 'QuoteTypeSubType'
go

create table [User]
(
    id       bigint identity (0, 1)
        constraint User_PK
            primary key nonclustered,
    UserName varchar(100)                       not null,
    uuid     uniqueidentifier default newid()   not null,
    Created  datetime         default getdate() not null,
    Modified datetime         default getdate() not null,
    Email    varchar(500),
    Verified bit              default 0         not null
)
go

create table QuoteRequestElementResponse
(
    QuoteRequestElementId int                        not null
        constraint QuoteRequestElementResponse_QuoteRequestElement_QuoteRequestElementId_fk
            references QuoteRequestElement,
    UserId                bigint                     not null
        constraint QuoteRequestElementResponse_User_id_fk
            references [User],
    Accepted              bit      default 0         not null,
    AmountLow             money    default 0,
    Notes                 varchar(1000),
    Link                  varchar(1000),
    Created               datetime default getdate() not null,
    Modified              datetime default getdate() not null,
    Submitted             datetime,
    AmountHigh            money,
    Estimate              bit      default 0         not null,
    constraint QuoteRequestElementResponse_pk
        primary key nonclustered (QuoteRequestElementId, UserId)
)
go

create unique clustered index User_uuid_IDX
    on [User] (uuid)
go

create unique index User_id_IDX
    on [User] (id)
go

create table UserPasswordHistory
(
    UserId   bigint                             not null
        constraint UserPasswordHistory_FK
            references [User],
    Password varchar(100)                       not null,
    uuid     uniqueidentifier default newid()   not null,
    Created  datetime         default getdate() not null,
    Expired  datetime,
    constraint UserPasswordHistory_PK
        primary key (UserId, uuid)
)
go

create unique index UserPasswordHistory_uuid_IDX
    on UserPasswordHistory (uuid, UserId)
go

create table UserQuoteElement
(
    QuoteElementId int                        not null
        constraint UserQuoteElement_QuoteElement_Id_fk
            references QuoteElement,
    UserId         bigint                     not null
        constraint UserQuoteElement_User_id_fk
            references [User],
    Created        datetime default getdate() not null,
    Modified       datetime default getdate() not null,
    Active         bit      default 1         not null,
    constraint UserQuoteElement_pk
        primary key nonclustered (QuoteElementId, UserId)
)
go

create table UserTokenTransaction
(
    UserId        bigint                        not null
        constraint UserTokenTransaction_User_id_fk
            references [User],
    TransactionId int identity
        constraint UserTokenTransaction_pk
            primary key nonclustered,
    Amount        int,
    Created       datetime    default getdate() not null,
    Action        varchar(10) default 'Credit'  not null
)
go

create unique index UserTokenTransaction_TransactionId_uindex
    on UserTokenTransaction (TransactionId)
go

create table UserType
(
    id       int identity
        constraint UserType_pk
            primary key nonclustered,
    Name     varchar(255),
    Created  datetime default getdate(),
    Modified datetime default getdate()
)
go

exec sp_addextendedproperty 'MS_Description', 'Types of user', 'SCHEMA', 'dbo', 'TABLE', 'UserType'
go

create table UserUserBlocks
(
    UserID        bigint
        constraint UserUserBlocks_User_id_fk
            references [User],
    BlockedUserId bigint
        constraint UserUserBlocks_User_id_fk_2
            references [User],
    Created       datetime default getdate() not null
)
go

exec sp_addextendedproperty 'MS_Description', 'Blocking user', 'SCHEMA', 'dbo', 'TABLE', 'UserUserBlocks', 'COLUMN',
     'UserID'
go

exec sp_addextendedproperty 'MS_Description', 'User being blocked', 'SCHEMA', 'dbo', 'TABLE', 'UserUserBlocks',
     'COLUMN', 'BlockedUserId'
go

create table UserUserType
(
    UserId     bigint not null
        constraint UserUserType_User_id_fk
            references [User],
    UserTypeId int    not null
        constraint UserUserType_UserType_id_fk
            references UserType,
    constraint UserUserType_pk
        unique (UserId, UserTypeId)
)
go


