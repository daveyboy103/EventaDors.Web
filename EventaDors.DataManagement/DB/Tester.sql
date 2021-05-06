select
    qt.Name,
    e.Name as EventName  ,
    qe.Name As ElementName,
    e.Notes,
    qe.BudgetMid * qe.Quantity As MidCost
from QuoteTemplate qt
         left join QuoteTemplateEvent QTE on qt.Id = QTE.QuoteTemplateId
         left join Event E on QTE.EventId = E.Id
         left join QuoteTemplateEventElement qtee on qtee.QuoteTemplateEventId = qte.QuoteTemplateEventId
         left join QuoteElement QE on qtee.ElementId = QE.Id
where qt.Id =5