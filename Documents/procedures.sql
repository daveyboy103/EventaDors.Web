CREATE PROCEDURE dbo.QUOTE_CreateFromTemplate @TemplateId int,
                                              @Attendees int,
                                              @OwnerId int,
                                              @DueDate datetime = null
AS

    BEGIN TRAN

DECLARE @id UNIQUEIDENTIFIER
DECLARE @QuoteidIdentity int

    SET @id = NEWID()

INSERT INTO QuoteRequest(QuoteId, Name, Notes, QuoteTypeId, QuoteSubTypeId, Owner, Attendees, DueDate)
SELECT @id,
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
            WHERE QuoteId = @id
              AND LeadWeeks IS NOT NULL
        END

    /*
    FROM QuoteElement qe
    INNER JOIN QuoteTemplateElement qte On qte.ElementId = qe.Id 
    INNER JOIN QuoteElementType qet On qet.Id = qe.ElementTypeId 
    WHERE TemplateId = @TemplateId*/

    COMMIT TRAN

    return @QuoteIdIdentity
go

CREATE procedure QUOTE_GetDeadline @QuoteIdIdentity int,
                                   @AlarmThreshold int = 30
As
SELECT       QRE.QuoteRequestElementId,
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
WHERE QR.QuoteIdIdentity = @QuoteIdIdentity
  and Qre.Completed = 0
ORDER BY QRE.DueDate
go

CREATE PROCEDURE dbo.QUOTE_LoadQuote @QuoteIdIdentity int
AS

SELECT qr.QuoteId,
       qr.QuoteIdIdentity,
       qr.Name,
       qr.Notes,
       qr.DueDate,
       qr.Created,
       qr.Modified,
       qt.Name  As QuoteTypeName,
       qt.Notes As QuoteTypeNotes,
       qt.Link  As QuoteTypeLink,
       qt.Name  As QuoteSubTypeName,
       qt.Notes As QuoteSubTypeNotes,
       qt.Link  As QuoteSubTypeLink
FROM QuoteRequest qr
         LEFT JOIN QuoteType qt ON qt.Id = qr.QuoteTypeId
         LEFT JOIN QuoteSubType qst ON qst.Id = qr.QuoteSubTypeId
WHERE qr.QuoteIdIdentity = @QuoteIdIdentity

SELECT qe.Name       As QuoteElementType,
       qe.Notes      As QuoteElementNotes,
       qe.id         As QuoteElementId,
       qre.DueDate,
       qe.LeadWeeks,
       qre.Completed,
       qre.Notes     As QuoteRequestElementNotes,
       qre.Budget    As Budget,
       qre.BudgetTolerance,
       qre.Quantity,
       qre.Created   As QuoteRequestElementCreated,
       qre.Modified  As QuoteRequestElementModified,
       qre.Submitted As QuoteRequestElementSubmitted,
       qre.Exclude   As QuoteRequestElementExclude

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
    IF NOT EXISTS(SELECT *
                  from QuoteRequestElementResponse
                  WHERE QuoteRequestElementId = @quoteRequestElementId
                    And UserId = @userId)
        INSERT INTO QuoteRequestElementResponse(UserId, Accepted, QuoteRequestElementId, AmountHigh, AmountLow, Notes,
                                                Link, Estimate)
        VALUES (@userId, @accepted, @quoteRequestElementId, @AmountHigh, @amountLow, @notes, @link, @estimate)
    ELSE
        UPDATE QuoteRequestElementResponse
        SET Accepted   = @accepted,
            AmountHigh = @AmountHigh,
            AmountLow  = @amountLow,
            Notes      = @notes,
            Link       = @link,
            Estimate   = @estimate
        WHERE QuoteRequestElementId = @quoteRequestElementId
          And UserId = @userId

SELECT QRER.QuoteRequestElementId,
       U.Id,
       U.UserName,
       u.Created  As UserCreated,
       u.Modified As UserModified,
       u.uuid     As UserUuid,
       qrer.Accepted,
       qrer.Estimate,
       qrer.AmountLow,
       qrer.AmountHigh,
       qrer.Notes,
       qrer.Link,
       qrer.Created,
       qrer.Modified,
       qrer.Submitted
FROM QuoteRequestElementResponse QRER
         INNER JOIN [User] U on QRER.UserId = U.id
WHERE QRER.QuoteRequestElementId = @quoteRequestElementId
  And QRER.UserId = @userId
go

create procedure USER_AssignToQuoteElement @userId int,
                                           @quoteElementId int,
                                           @active bit = 1
as
    IF EXISTS(SELECT *
              FROM UserQuoteElement
              WHERE UserId = @userId
                AND QuoteElementId = @quoteElementId)
        begin
            update UserQuoteElement
            set Active = @active
            where UserId = @userId
              AND QuoteElementId = @quoteElementId
        end
    else
        begin
            insert into UserQuoteElement(QuoteElementId, UserId, Active)
            values (@userId, @quoteElementId, @active)
        end
go

CREATE procedure USER_AssignToQuoteElementType @userId int,
                                               @quoteElementTypeId int,
                                               @active bit = 1
as
insert into UserQuoteElement(Userid, QuoteElementId, Active)
select @userId, id, @active
from QuoteElement
where ElementTypeId = @quoteElementTypeId
  and id not in (select UQE.QuoteElementId
                 from UserQuoteElement UQE
                          inner join QuoteElement QE on UQE.QuoteElementId = QE.Id
                 where UserId = @userId
                   and QE.ElementTypeId = @quoteElementTypeId
)
go

CREATE PROCEDURE USER_GetTokenBalance @UserId int
AS

DECLARE @credit int
DECLARE @debit int

SELECT @credit = SUM(utt.Amount)
FROM UserTokenTransaction utt
WHERE utt.Action = 'Credit'
  And utt.UserId = @UserId

SELECT @debit = SUM(utt.Amount)
FROM UserTokenTransaction utt
WHERE utt.Action = 'Debit'
  And utt.UserId = @UserId

SELECT @credit - @debit As Balance
go

CREATE PROCEDURE dbo.USER_GetUser @userId bigint
AS
SELECT u.id,
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
SELECT *
from dbo.[User] u
ORDER BY u.UserName
go

CREATE PROCEDURE dbo.USER_LoadUser @userId bigint
AS
SELECT u.id,
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

SELECT mdn.Name,
       md.Value,
       md.Created,
       md.Modified
FROM MetaData md
         INNER JOIN MetaDataName mdn on md.NameId = MDN.Id
WHERE md.TableName = 'User'
  And md.KeyId = @userId
go

CREATE PROCEDURE dbo.USER_LoginUser @UserName varchar(255),
                                    @Password varchar(255)
AS
    IF EXISTS(SELECT *
              FROM [User] u
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

CREATE PROCEDURE dbo.USER_RegisterUser @UserName varchar(100),
                                       @Email varchar(500),
                                       @Password varchar(50)
AS
DECLARE @id int
    IF NOT EXISTS(SELECT *
                  FROM [User]
                  WHERE UserName = @UserName)
        BEGIN
            INSERT INTO [User](UserName, Email)
            VALUES (@UserName, @Email)
            SET @id = SCOPE_IDENTITY()
            INSERT INTO UserPasswordHistory(UserId, Password)
            VALUES (@id, @Password)
        END
    ELSE
        BEGIN
            UPDATE [User]
            SET Email = @Email
            WHERE UserName = @UserName

            SELECT @id = id
            from [User]
            WHERE UserName = @UserName
        END


    RETURN @id
go

CREATE PROCEDURE dbo.USER_VerfyAccount @uuid UNIQUEIDENTIFIER
As
UPDATE [User]
SET Verified = 1
WHERE uuid = @uuid
go


