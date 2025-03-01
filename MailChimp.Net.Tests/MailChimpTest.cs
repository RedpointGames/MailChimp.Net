﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailChimpTest.cs" company="Brandon Seydel">
//   N/A
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MailChimp.Net.Tests
{
    /// <summary>
    /// The mail chimp test.
    /// </summary>
    public abstract class MailChimpTest
    {
        /// <summary>
        /// The _mail chimp manager.
        /// </summary>
        protected IMailChimpManager _mailChimpManager;

        internal List GetMailChimpList(string listName = "TestList") => new List
        {
            Name = listName,
            PermissionReminder = "none",
            Contact = new Contact
            {
                Address1 = "TEST",
                City = "Bettendorf",
                Country = "USA",
                State = "IA",
                Zip = "61250",
                Company = "TEST"
            },
            CampaignDefaults = new CampaignDefaults
            {
                FromEmail = "test@test.com",
                FromName = "test",
                Language = "EN",
                Subject = "Yo"
            }
        };

        internal async Task ClearLists(params string[] listToDeleteNames)
        {
            var lists = await _mailChimpManager.Lists.GetAllAsync();
            var listsToDelete = listToDeleteNames.Any()
                ? lists.Where(i => listToDeleteNames.Contains(i.Name, StringComparer.InvariantCultureIgnoreCase))
                : lists;

            await Task.WhenAll(listsToDelete.Select(x => _mailChimpManager.Lists.DeleteAsync(x.Id)));
        }

        internal async Task ClearCampaigns()
        {
            var campaigns = await this._mailChimpManager.Campaigns.GetAllAsync();
            await Task.WhenAll(campaigns.Select(x => _mailChimpManager.Campaigns.DeleteAsync(x.Id)));
        }

        internal async Task ClearMailChimpAsync()
        {
            await ClearLists();
            await ClearCampaigns();
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var configuration = new MailChimpConfiguration();
            configuration.ApiKey = "599629998b18960fa6e8a283d280ac28-us13";

            this._mailChimpManager = new MailChimpManager();
            this._mailChimpManager.Configure(configuration);

            RunBeforeTestFixture().Wait();
        }

        internal virtual async Task RunBeforeTestFixture()
        {
        }

        /// <summary>
        /// The hash.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        internal string Hash(string emailAddress)
        {
            using (var md5 = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(emailAddress));
                var builder = new StringBuilder();
                foreach (var t in data)
                {
                    builder.Append(t.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}