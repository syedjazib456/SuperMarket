﻿using Microsoft.AspNetCore.Identity.UI.Services;

namespace Admin_panel.Models.Data
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
           return Task.CompletedTask;
        }
    }
}
