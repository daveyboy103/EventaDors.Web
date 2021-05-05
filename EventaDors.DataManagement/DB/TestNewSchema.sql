select
    e.Name as 'Event Name',
    qrv.Name as 'Venue Name',
    qre.EventDate As 'Event Date'
from QuoteRequest qr
         inner join QuoteRequestEvent QRE on qr.QuoteId = QRE.QuoteId
         inner join Event e on e.Id = qre.EventId
         inner join QuoteRequestVenue QRV on QRV.QuoteId = qr.QuoteId and QRE.VenueId = QRV.Venueid
where qr.QuoteId = 'FE2BE4EC-B5BB-474F-80EA-95547CA1EF00'
Order by qre.EventOrder


select
    e.Name as 'Event Name',
    qrgl.FirstName + ' ' + qrgl.LastName as FullName,
    sn.Name As 'Special Need',
    qrgln.Notes As 'Special Need Notes'
from QuoteRequest qr
         inner join QuoteRequestEvent QRE on qr.QuoteId = QRE.QuoteId
         inner join Event e on e.Id = qre.EventId
         inner join QuoteRequestGuestList QRGL on qr.QuoteId = QRGL.QuoteId
         inner join QuoteRequestGuestListAttending QRGLA on e.Id = QRGLA.EventId and qr.QuoteId = QRGLA.QuoteId and QRGL.GuestId = QRGLA.GuestId
         left join QuoteRequestGuestListNeed QRGLN on QRGLN.GuestId = QRGL.GuestId
         left join SpecialNeed sn on sn.NeedId = qrgln.NeedId
where qr.QuoteId = 'FE2BE4EC-B5BB-474F-80EA-95547CA1EF00'
order by e.Name, qre.EventOrder

select e.Name, Count(QRGLA.EventId) As Attending
from QuoteRequest qr
         inner join QuoteRequestEvent QRE on qr.QuoteId = QRE.QuoteId
         inner join Event e on e.Id = qre.EventId
         inner join QuoteRequestGuestList QRGL on qr.QuoteId = QRGL.QuoteId
         inner join QuoteRequestGuestListAttending QRGLA on e.Id = QRGLA.EventId and qr.QuoteId = QRGLA.QuoteId and QRGL.GuestId = QRGLA.GuestId
group by qr.QuoteId, e.Name, QRGLA.EventId
having qr.QuoteId = 'FE2BE4EC-B5BB-474F-80EA-95547CA1EF00'




    
    