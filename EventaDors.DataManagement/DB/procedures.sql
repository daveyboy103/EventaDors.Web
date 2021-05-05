use EventaDors
go

create proc QUOTE_AddUpdateDelegation
@userId int,
@quoteRequestElementid int,
@admin bit
As

    if exists(select * from QuoteRequestElementDelegate 
                where QuoteRequestElementId = @quoteRequestElementid
                and UserId = @userId)
        begin
            update QuoteRequestElementDelegate set Admin= @admin
            where QuoteRequestElementId = @quoteRequestElementid
              and UserId = @userId
        end
    else
        begin 
            insert into QuoteRequestElementDelegate(UserId, QuoteRequestElementId, Admin)
            values(@userId, @quoteRequestElementid, @admin)
        end
go


create proc QUOTE_AssignDeleateToWholeRequest
    @userId bigint,
    @quoteIdIdentity int,
    @admin bit

As
INSERT INTO QuoteRequestElementDelegate(UserID, QuoteRequestElementId, Admin)
SELECT @UserId, QuoteRequestElementId, @admin FROM QuoteRequestElement QRE
                                                       INNER JOIN QuoteRequest QR on QRE.QuoteId = QR.QuoteId
WHERE QR.QuoteIdIdentity = @quoteIdIdentity
  AND QRE.QuoteRequestElementId NOT In(
    SELECT qrd.QuoteRequestElementId FROM QuoteRequestElementDelegate qrd
                                          INNER JOIN QuoteRequestElement Q on q.QuoteRequestElementId = qrd.QuoteRequestElementId
                                          INNER JOIN QuoteRequest QR on QRE.QuoteId = QR.QuoteId
    WHERE UserId = @UserId And qr.QuoteIdIdentity = @quoteIdIdentity)
go

CREATE PROCEDURE dbo.QUOTE_CreateFromTemplate
@TemplateId int,
@Attendees int,
@OwnerId int,
@DueDate datetime = null
AS 

	BEGIN TRAN
	
	DECLARE @id UNIQUEIDENTIFIER
	DECLARE @QuoteidIdentity int
	
	SET @id = NEWID()
		
	INSERT INTO QuoteRequest(QuoteId, Name, Notes, QuoteTypeId, QuoteSubTypeId, Owner, Attendees, DueDate)
	SELECT 
		@id,
		'From Template: ' + qt.Name, 
		Notes, 
		QuoteTypeId, 
		QuoteSubTypeId,
		@OwnerId,
		@Attendees,
	    @DueDate
	FROM QuoteTemplate qt
	WHERE qt.Id = @TemplateId
	
	SET @QuoteIdIdentity = SCOPE_IDENTITY()
	
	INSERT INTO QuoteRequestElement(QuoteId, QuoteElementId, Quantity, BudgetTolerance, LeadWeeks)
	SELECT @id, 
		qe.Id,
		CASE qe.InheritTopLevelQuantity
			WHEN 1 THEN @Attendees
			ELSE qe.Quantity	
		END,
		qe.BudgetTolerance,
	    qe.LeadWeeks
	FROM QuoteElement qe
	INNER JOIN QuoteTemplateElement qte On qte.ElementId = qe.Id 
	INNER JOIn QuoteElementType qet On qet.Id = qe.ElementTypeId 
	WHERE TemplateId = @TemplateId 
    
    If @DueDate IS NOT Null
        BEGIN
            UPDATE QuoteRequestElement 
            SET DueDate = @DueDate - (LeadWeeks * 7)
            WHERE QuoteId = @id AND LeadWeeks IS NOT NULL
        END
    
    INSERT INTO QuoteRequestElementMetaData(QuoteRequestElementId, MetaDataNameId, Value) 
    SELECT 
        qre.QuoteRequestElementId, md.NameId, md.Value
    FROM QuoteRequestElement qre 
    INNER JOIN QuoteElement qe ON qe.Id = qre.QuoteElementId
    INNER JOIN MetaData MD on md.KeyId = qe.Id and md.TableName = 'QuoteElement'
    WHERE qre.QuoteId = @id
	
	COMMIT TRAN
	
    return @QuoteIdIdentity
go


CREATE procedure QUOTE_GetChatHistoryForElement
    @quoteRequestElementId int,
    @receiverId int
    as
    SELECT
        c.Message,
        c.Link ,
        uf.UserName As [From],
        ut.UserName As [To],
        c.Created
    FROM Chat c
             INNER JOIN [User] uf On uf.id  = c.OwnerId
             INNER JOIN [User] ut On ut.id  = c.ReceiverId
    WHERE c.QuoteRequestElementId  = @quoteRequestElementId
    AND c.ReceiverId = @receiverId or c.OwnerId = @receiverId
    ORDER BY c.created desc
go


CREATE procedure QUOTE_GetDeadline
    @QuoteIdIdentity int,
    @AlarmThreshold int = 30
As
    SELECT
             QRE.QuoteRequestElementId,
             QE.Name,
             QRE.DueDate,
             DATEDIFF(WW, GETDATE(), QRE.DueDate) As 'In Weeks',
    Status = CASE
                 WHEN DATEDIFF(WW, GETDATE(), QRE.DueDate) < @AlarmThreshold THEN 'RED'
                 WHEN DATEDIFF(WW, GETDATE(), QRE.DueDate) < (@AlarmThreshold * 2) THEN 'AMBER'
                 ELSE 'GREEN'
                 END
FROM QuoteRequestElement QRE
         INNER JOIN QuoteRequest QR on QRE.QuoteId = QR.QuoteId
         INNER JOIN QuoteElement QE on QE.Id = QRE.QuoteElementId
WHERE QR.QuoteIdIdentity = @QuoteIdIdentity and Qre.Completed = 0
ORDER BY QRE.DueDate
go

CREATE PROCEDURE dbo.QUOTE_LoadQuote
@QuoteIdIdentity int

AS 
	
	SELECT 
		qr.QuoteId,
		qr.QuoteIdIdentity,
		qr.Name,
		qr.Notes,
	    qr.DueDate,
		qr.Created,
		qr.Modified,
		qt.Name As QuoteTypeName,
		qt.Notes As QuoteTypeNotes,
		qt.Link As QuoteTypeLink,
		qt.Name As QuoteSubTypeName,
		qt.Notes As QuoteSubTypeNotes,
		qt.Link As QuoteSubTypeLink	
	FROM QuoteRequest qr 
	LEFT JOIN QuoteType qt ON qt.Id = qr.QuoteTypeId
	LEFT JOIN QuoteSubType qst ON qst.Id = qr.QuoteSubTypeId
	WHERE qr.QuoteIdIdentity = @QuoteIdIdentity
	
	SELECT
		qe.Name As QuoteElementType,
		qe.Notes As QuoteElementNotes,
		qe.id As QuoteElementId,
		qre.QuoteRequestElementId,
	    qre.DueDate,
	    qe.LeadWeeks,
	    qre.Completed,
		qre.Notes As QuoteRequestElementNotes,
		qre.Budget As Budget,
		qre.BudgetTolerance,
		qre.Quantity,
		qre.Created As QuoteRequestElementCreated,
		qre.Modified As QuoteRequestElementModified,
		qre.Submitted As QuoteRequestElementSubmitted,
		qre.Exclude As QuoteRequestElementExclude
	
	FROM QuoteRequestElement qre 
	INNER JOIN QuoteRequest qr On qr.QuoteId = qre.QuoteId
	INNER Join QuoteElement qe ON qe.Id = qre.QuoteElementId
	WHERE qr.QuoteIdIdentity = @QuoteIdIdentity
go


CREATE procedure QUOTE_PickupQuoteRequestItem(
    @quoteRequestElementId int,
    @userId int,
    @accepted int = 1,
    @amountLow money = 0,
    @AmountHigh money = 0,
    @notes varchar(1000) = null,
    @link varchar(1000) = null,
    @estimate bit = 0
    ) 
as
    IF NOT EXISTS(SELECT * from QuoteRequestElementResponse WHERE QuoteRequestElementId = @quoteRequestElementId And UserId = @userId)
        INSERT INTO QuoteRequestElementResponse(UserId, Accepted, QuoteRequestElementId, AmountHigh, AmountLow, Notes, Link, Estimate)
        VALUES (@userId, @accepted, @quoteRequestElementId, @AmountHigh, @amountLow, @notes, @link, @estimate)
    ELSE
        UPDATE QuoteRequestElementResponse 
        SEt Accepted = @accepted,
            AmountHigh = @AmountHigh,
            AmountLow = @amountLow,
            Notes = @notes,
            Link = @link,
            Estimate = @estimate
        WHERE QuoteRequestElementId = @quoteRequestElementId And UserId = @userId

go


create proc QUOTE_RemoveDelegation
@userId int,
@quoteRequestElementid int
As

    delete from QuoteRequestElementDelegate 
        where UserId = @userId 
      and QuoteRequestElementId = @quoteRequestElementid
go

create procedure STATIC_AddUpdateQuoteElement
@id int = 0,
@name varchar(500),
@notes varchar(max),
@budgetTolerance float,
@quantity int,
@elementTypeId int,
@inheritTopLevelQuantity bit = 0,
@leadWeeks int

AS
    
    if exists(select * from QuoteElement  where id = @id)
        begin -- update
            update QuoteElement
                set Name = @name,
                    Notes = @notes,
                    BudgetTolerance = @budgetTolerance,
                    Quantity = @quantity,
                    ElementTypeId = @elementTypeId,
                    InheritTopLevelQuantity = @inheritTopLevelQuantity,
                    LeadWeeks = @leadWeeks
            where id = @id
            return @id
        end
    else
        begin -- insert
            insert into QuoteElement(Name, Notes, BudgetTolerance, Quantity, ElementTypeId, InheritTopLevelQuantity, LeadWeeks)
            values(
                   @name, @notes, @budgetTolerance, @quantity, @elementTypeId, @inheritTopLevelQuantity, @leadWeeks
                  )
                  
            declare @retid int
            SET @retid = scope_identity()
            return @retid
        end
go

CREATE procedure STATIC_AddUpdateQuoteElementToQuoteRequest
@QuoteId uniqueidentifier,
@QuoteElementId int,
@QuoteRequestElementId int = null,
@budget money = null,
@BudgetTolerance float = null,
@quantity int = null,
@exclude bit = 0,
@notes varchar(1000),
@leadWeeks int = null,
@DueDate datetime = null,
@completed bit  = 0

as
    
    if exists(select * from QuoteRequestElement where QuoteRequestElementId = @QuoteRequestElementID)
        begin -- update
            update QuoteRequestElement
            set     
                BudgetTolerance = @budgetTolerance ,
                Budget = @budget,
                Quantity = @quantity,
                Exclude = @exclude,
                Notes = @notes,
                LeadWeeks = @leadWeeks,
                DueDate = @DueDate,
                Completed = @completed
            where QuoteRequestElementId = @QuoteRequestElementId
            return @QuoteRequestElementId
        end
    else
        begin
            insert into QuoteRequestElement(quoteid, quoteelementid, budget, budgettolerance, quantity, notes, leadweeks, duedate) 
            values (@QuoteId, @QuoteElementId, @budget, @BudgetTolerance, @quantity, @notes, @leadWeeks, @DueDate)
            
            Declare @id int 
            Set @id = scope_identity()
            declare @weeks int
            
            if @duedate is null
                begin
                    select @DueDate = qr.DueDate - - (qe.LeadWeeks * 7), 
                           @weeks = qe.LeadWeeks from QuoteRequest qr
                    inner join QuoteRequestElement QRE on qr.QuoteId = QRE.QuoteId
                    inner join QuoteElement QE on QRE.QuoteElementId = QE.Id
                    where qre.QuoteRequestElementId = @id
                end

            if @quantity is null
                begin
                    select @Quantity = qre.Quantity from QuoteRequest qr
                     inner join QuoteRequestElement QRE on qr.QuoteId = QRE.QuoteId
                     inner join QuoteElement QE on QRE.QuoteElementId = QE.Id
                    where qre.QuoteRequestElementId = @id and qe.InheritTopLevelQuantity = 1
                end
            
            update QuoteRequestElement set Quantity = @quantity, DueDate = @DueDate, LeadWeeks = @weeks
                where QuoteRequestElementId = @id  
            
            return @id
        end
go

create procedure STATIC_AddUpdateQuoteElementType
@id int,
@name varchar(255),
@notes varchar(max),
@link varchar(1000)
as
    
    if exists(select * from QuoteElementType qet where qet.Id = @id)
        begin -- update
            update QuoteElementType
                set Name = @name,
                    Link = @link,
                    Notes = @notes
            where id = @id
        end
    else
        begin
            insert into QuoteElementType(Name, Notes, Link) 
            values (@name, @notes, @link)
            
            set @id = scope_identity()
        end
    
    return @id
go

CREATE procedure STATIC_AddUpdateQuoteSubType
@id int,
@name varchar(255),
@notes varchar(max),
@link varchar(1000)
as
    
    if exists(select * from QuoteSubType qet where qet.Id = @id)
        begin -- update
            update QuoteSubType
                set Name = @name,
                    Link = @link,
                    Notes = @notes
            where id = @id
        end
    else
        begin
            insert into QuoteSubType(Name, Notes, Link) 
            values (@name, @notes, @link)
            
            set @id = scope_identity()
        end
    
    return @id
go

create procedure STATIC_AddUpdateQuoteType
@id int,
@name varchar(255),
@notes varchar(max),
@link varchar(1000)
as
    
    if exists(select * from QuoteType qet where qet.Id = @id)
        begin -- update
            update QuoteType
                set Name = @name,
                    Link = @link,
                    Notes = @notes
            where id = @id
        end
    else
        begin
            insert into QuoteType(Name, Notes, Link) 
            values (@name, @notes, @link)
            
            set @id = scope_identity()
        end
    
    return @id
go

CREATE procedure STATIC_LoadQuoteElement
@id int = 0

AS

    select 
        qe.id, 
        qe.name, 
        qe.notes, 
        qe.budgettolerance, 
        qe.quantity, 
        qe.inherittoplevelquantity, 
        qe.leadweeks,
        qet.Notes As ElementTypeNotes,
        qet.Name As ElementTypeName,
        qet.Link As ElementTypeLink,
        qet.Id as ElementTypeId
    from QuoteElement qe 
    inner join QuoteElementType qet on qet.id = qe.ElementTypeId
    where qe.id = @id

    select 
        mdn.Name,
        md.Value,
        md.Type,
        md.Created,
        md.Modified
    from MetaData md
    inner join MetaDataName MDN on md.NameId = MDN.Id
    where md.TableName = 'QuoteElement' and md.KeyId = @id
go


create procedure USER_AssignToQuoteElement
    @userId int,
    @quoteElementId int,
    @active bit = 1
as
    IF EXISTS(SELECT * FROM UserQuoteElement WHERE UserId = @userId AND QuoteElementId = @quoteElementId)
        begin
            update UserQuoteElement set Active = @active
            where UserId = @userId AND QuoteElementId = @quoteElementId
        end
    else
        begin
            insert into UserQuoteElement(QuoteElementId, UserId, Active)  
            values(@userId, @quoteElementId, @active)
        end
go

exec sp_addextendedproperty 'MS_Description', 'Assigns a supplier to a single quote element', 'SCHEMA', 'dbo', 'PROCEDURE', 'USER_AssignToQuoteElement'
go


CREATE procedure USER_AssignToQuoteElementType
    @userId int,
    @quoteElementTypeId int,
    @active bit = 1
as
    insert into UserQuoteElement(Userid, QuoteElementId, Active)
    select @userId, id, @active from QuoteElement
    where ElementTypeId = @quoteElementTypeId
    and id not in(select UQE.QuoteElementId from UserQuoteElement UQE
        inner join QuoteElement QE on UQE.QuoteElementId = QE.Id
        where UserId = @userId and QE.ElementTypeId = @quoteElementTypeId)
go

exec sp_addextendedproperty 'MS_Description', 'Assigns a supplier to all quote elelmenst in a type', 'SCHEMA', 'dbo', 'PROCEDURE', 'USER_AssignToQuoteElementType'
go

CREATE procedure USER_CreateNewUser
@UserName varchar(100),
@email varchar(500),
@password varchar(100)

As
    if exists(select * from [User] where UserName = @UserName)
        begin
            RAISERROR('User already exists, please choose another name', 16, 1)
            return 0
        end
    else

        begin tran 
        insert into [User](UserName, Email) 
        values(@UserName, @email)
        
        declare @id int
        set @id = SCOPE_IDENTITY()

        insert into UserPasswordHistory(UserId, Password) 
        values(@id, @password)
        
        commit tran

        return @id
go


CREATE PROCEDURE USER_GetTokenBalance
@UserId int

AS

DECLARE @credit int
DECLARE @debit int

SELECT @credit = SUM(utt.Amount) FROM UserTokenTransaction utt
WHERE utt.Action = 'Credit' And utt.UserId = @UserId

SELECT @debit = SUM(utt.Amount) FROM UserTokenTransaction utt
WHERE utt.Action = 'Debit' And utt.UserId = @UserId

SELECT @credit - @debit As Balance
go


CREATE PROCEDURE dbo.USER_GetUser
@userId bigint
AS 
    SELECT 
    	u.id,
        u.UserName,
        u.Created,
        u.Modified,
        u.Created,
        u.uuid,
        uph.Password,
        uph.Expired
    FROM dbo.[User] u
    LEFT JOIN dbo.UserPasswordHistory uph ON u.id = uph.UserId
    WHERE u.id = @userId
go


CREATE PROCEDURE dbo.USER_ListUsers
AS 
SELECT * from dbo.[User] u ORDER BY u.UserName
go

CREATE PROCEDURE dbo.USER_LoadUser
@userId bigint
AS 
    SELECT 
        u.id,
        u.UserName,
        u.uuid,
        u.Email,
        u.Created,
        u.Modified,
        u.Verified,
        uph.Password
    FROM [User] u
    INNER JOIN UserPasswordHistory uph On uph.UserId = u.id 
                                              AND uph.Expired IS NULL
    WHERE u.Id = @userId

    SELECT 
        mdn.Name,
        md.Value,
        md.Type,
        md.Created,
        md.Modified
    FROM MetaData md
    INNER JOIN MetaDataName mdn on md.NameId = MDN.Id
    WHERE md.TableName = 'User' And md.KeyId = @userId
go


CREATE PROCEDURE dbo.USER_LoginUser
@UserName varchar(255),
@Password varchar(255)
AS 

	IF EXISTS(SELECT * FROM [User] u
				INNER JOIN UserPasswordHistory uph on uph.UserId = u.Id
				WHERE u.UserName = @UserName
				AND uph.Password = @Password
				AND uph.Expired IS NULL
	            AND u.Verified = 1
			)  
			BEGIN 
				SELECT 1
			END
	ELSE 	
		BEGIN 
			SELECT 0
		END
go


CREATE PROCEDURE dbo.USER_RegisterUser
	@UserName varchar(100),
	@Email varchar(500),
	@Password varchar(50)
AS 
	DECLARE @id int
	
	IF NOT EXISTS(SELECT * FROM [User] WHERE UserName = @UserName)
		BEGIN 
			INSERT INTO [User](UserName, Email)
			VALUES (@UserName, @Email)
			SET @id = SCOPE_IDENTITY()
			INSERT INTO UserPasswordHistory(UserId, Password)
			VALUES(@id, @Password)
		END
	ELSE 
		BEGIN 
			UPDATE [User]
			SET Email = @Email
			WHERE UserName = @UserName
			
			SELECT @id = id from [User]
			WHERE UserName = @UserName
		END


	RETURN @id
go


CREATE PROCEDURE dbo.USER_VerfyAccount
@uuid UNIQUEIDENTIFIER
As
	UPDATE [User]
	SET Verified = 1
	WHERE uuid = @uuid
go

