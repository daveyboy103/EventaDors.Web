using System;

namespace EventaDors.Entities.Classes
{
    public class Deadline
    {
        private int _responses;
        private string _name;
        public int QuoteRequestElementId { get; init; }

        public string Name
        {
            get => _name;
            init
            {
                _name = value;
                
                if (_responses > 0)
                {
                    _name = $" {_name} ({_responses})";
                }
            }
        }

        public DateTime? DueDate { get; init; }
        public DateTime? Submitted { get; init; }
        public int? Weeks { get; init; }
        public string Status { get; init; }
        public int Responses
        {
            get => _responses;

            set
            {
                _responses = value;

                if (_responses > 0)
                {
                    _name = $" {_name} ({_responses})";
                }
            }
        }
        public int Chats { get; init; }
    }
}