using System.Collections;
using System.Collections.Generic;
using EventaDors.Entities.Classes;

namespace StaticDataMaintenence.Models
{
    public class EventWrapper : FormActionBase
    {
        public EventWrapper()
        {
            List = new List<Event>();
        }
        
        public EventWrapper(Event @event, IEnumerable<Event> list)
        {
            Event = @event;
            List = list;
        }
        
        public Event Event { get; set; }
        public IEnumerable<Event> List { get; set; } 
    }

    public class FormActionBase
    {
        public FormAction Action { get; set; }
    }

    public enum FormAction
    {
        New,
        Delete,
        Get,
        Save,
        Success,
        Failed,
        Load
    }
}
