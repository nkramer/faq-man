using System;
using System.Linq;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;

namespace Microsoft.Teams.Samples.HelloWorld.Web
{
    public class MessageExtension
    {
        public static ComposeExtensionResponse HandleMessageExtensionQuery(ConnectorClient connector, Activity activity)
        {
            var query = activity.GetComposeExtensionQueryData();
            if (query == null || query.CommandId != "getRandomText")
            {
                // We only process the 'getRandomText' queries with this message extension
                return null;
            }

            var title = "";
            var titleParam = query.Parameters?.FirstOrDefault(p => p.Name == "cardTitle");
            if (titleParam != null)
            {
                title = titleParam.Value.ToString();
            }

            var attachments = new ComposeExtensionAttachment[]
            {
                GetAttachment("which API",
                @"In order to give you a precise answer, could you clarify:
- The URL and http verb for the method of interest (eg: POST https://graph.microsoft.com/beta/teams/{id}/channels)
- Application permissions or user delegated permissions?

Thx.
"),
                                GetAttachment("API failed",
                @"In order to give you a precise answer, could you clarify:
The URL and http verb for the method of interest(eg: POST https://graph.microsoft.com/beta/teams/{id}/channels)
Application permissions or user delegated permissions ?
The request - id from a call that's failed within the last 24 hours (we only keep a few days of logs)
The payload you passed in, and the payload you got back
The http response code
If it was a 401 or 403, why do you think this was a mistake? What permission scopes are you calling it with?

Thx.
"),
                                GetAttachment("Roadmap",
                @"Our roadmap is available at _________________, please let me know if you have any questions."),
                                GetAttachment("Not planned", "We do not currently have plans to build that feature."),
        };

            //for (int i = 0; i < 5; i++)
            //{
            //    attachments[i] = GetAttachment(title);
            //}

            var response = new ComposeExtensionResponse(new ComposeExtensionResult("list", "result"));
            //var response = new ComposeExtensionResponse(new ComposeExtensionResult("list", type: "message", text: "foo"));
            response.ComposeExtension.Attachments = attachments.ToList();
            //response.ComposeExtension.Text = "faq me!";

            return response;
        }

        private static ComposeExtensionAttachment GetAttachment(string title, string body)
        {
            var card = new ThumbnailCard
            {
                Title = !string.IsNullOrWhiteSpace(title) ? title : Faker.Lorem.Sentence(),
                Text = body,
               //Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
            };
            
            return card
                .ToAttachment()
                .ToComposeExtensionAttachment();
        }
    }
}
