DECLARE @id int

EXEC @id = QUOTE_CreateFromTemplate 2, 179, 32, '12-Jul-2022'

select
    qr.Name,
    e.Name,
    qre.Attendees,
    qe.Name
from QuoteRequest qr
         inner join QuoteRequestEvent QRE on qr.QuoteId = QRE.QuoteId
         left join QuoteRequestElement qrel on qrel.QuoteId = qre.QuoteId and qrel.EventId = qre.EventId
         left join QuoteElement qe on qe.Id = qrel.SourceElementId
         left join Event E on QRE.EventId = E.Id
where QuoteIdIdentity = @Id