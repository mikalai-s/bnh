using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.ViewModels
{
    public enum MessageType { Info, Success, Warning, Error }

    public class MessageViewModel
    {
        public MessageType MessageType { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}