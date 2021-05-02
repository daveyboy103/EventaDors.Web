using System;

namespace EventaDors.Entities.Classes
{
    public class Deadline
    {
        private int _responses;
        private string _name;
        public int QuoteRequestElementId { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                
                if (_responses > 0)
                {
                    _name = $" {_name} ({_responses})";
                }
            }
        }

        public DateTime? DueDate { get; set; }
        public DateTime? Submitted { get; set; }
        public int? Weeks { get; set; }
        public string Status { get; set; }
        public int Responses
        {
            get
            {
                return _responses;
            }

            set
            {
                _responses = value;

                if (_responses > 0)
                {
                    _name = $" {_name} ({_responses})";
                }
            }
        }
        public int Chats { get; set; }
    }
}