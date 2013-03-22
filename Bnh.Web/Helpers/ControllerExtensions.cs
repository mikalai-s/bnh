using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.ViewModels;

namespace Bnh
{
    public static class ControllerExtensions
    {
        public static ViewResult SuccessMessage(this Controller controller, string title, string text)
        {
            return Message(controller, MessageType.Success, title, text);
        }

        public static ViewResult ErrorMessage(this Controller controller, string title, string text)
        {
            return Message(controller, MessageType.Error, title, text);
        }

        public static ViewResult InfoMessage(this Controller controller, string title, string text)
        {
            return Message(controller, MessageType.Info, title, text);
        }

        public static ViewResult WarningMessage(this Controller controller, string title, string text)
        {
            return Message(controller, MessageType.Warning, title, text);
        }

        public static ViewResult Message(this Controller controller, MessageType messageType, string title, string text)
        {
            return Message(controller, new MessageViewModel
            {
                MessageType = messageType,
                Title = title,
                Text = text
            });
        }

        public static ViewResult Message(this Controller controller, MessageViewModel message)
        {
            if (message != null)
            {
                controller.ViewData.Model = message;
            }

            return new ViewResult
            {
                ViewName = "Message",
                MasterName = null,
                ViewData = controller.ViewData,
                TempData = controller.TempData,
                ViewEngineCollection = controller.ViewEngineCollection
            };
        }
    }
}