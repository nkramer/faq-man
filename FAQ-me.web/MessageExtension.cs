using System;
using System.Linq;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using AdaptiveCards;
using System.Web;

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
                GetAttachment("Which API?",
                @"In order to give you a precise answer, could you clarify:
- The URL and http verb for the method of interest (eg: POST https://graph.microsoft.com/beta/teams/{id}/channels)
- Application permissions or user delegated permissions?

Thx.
"),
                GetAttachment("API failed",
                @"In order to give you a precise answer, could you clarify:
- The URL and http verb for the method of interest (eg: POST https://graph.microsoft.com/beta/teams/{id}/channels)
- Application permissions or user delegated permissions?
- The request - id from a call thats failed within the last 24 hours (we only keep a few days of logs)
- The payload you passed in, and the payload you got back
- The http response code
- If it was a 401 or 403, why do you think this was a mistake? What permission scopes are you calling it with?

Thx.
"),
                GetAttachment("Roadmap",
                @"Our roadmap is available at [here](https://microsoft.sharepoint.com/:x:/r/teams/skypespacesteamnew/_layouts/15/Doc.aspx?sourcedoc=%7BF8198243-EEDB-4E51-B6FD-BF3D5894EED0%7D&file=Roadmap.xlsx&action=default&mobileredirect=true), start with the tab on the left. Please let me know if you have any questions, thx."),

                GetAttachment("Not planned", 
                "We do not currently have plans to build that feature. If you would like to request it, please let me know which customers are blocked, what scenario they are trying to build, and what the expected Platform MAU impact will be. Thx."),

                            GetAttachment("beta in production",
                @"Can I use the beta Teams Graph APIs in production? Yes, with caveats. We hold beta APIs to the same reliability standards as V1, and address live site issues promptly. The big difference between beta and V1 is support and breaking changes:
- Microsoft Support will not support beta APIs. 
- We try not to break beta because lots of people are using them in production, but sometimes we need to make a change. Usually 'break' means 'there is a new way to achieve that scenario' – it's very rare for us to remove functionality without offering a new way of achieving the result. We try to give advanced warning of breaking changes, announced on the [Microsoft Graph blog](https://developer.microsoft.com/en-us/graph/blogs/).
                "),
};

            //for (int i = 0; i < 5; i++)
            //{
            //    attachments[i] = GetAttachment(title);
            //}

            var response = new ComposeExtensionResponse(new ComposeExtensionResult(
                attachmentLayout: "list", 
                type: "result", 
                attachments: attachments.ToList()));
            response.ComposeExtension.Text = "-----------faq me!-----------";
            //response.ComposeExtension.Attachments = attachments.ToList();
            //var response = new ComposeExtensionResponse(new ComposeExtensionResult("list", type: "message", text: "foo"));
            //response.ComposeExtension.Text = "faq me!";

            return response;
        }

        private static string cardJson
            = @"{
    'type': 'AdaptiveCard',
    'body': [
                {
                    'type': 'TextBlock',
                    'text': '**replace here**',
                    'wrap': true
                }
           ],
    '$schema': 'http://adaptivecards.io/schemas/adaptive-card.json',
    'version': '1.0'
}";

        private static ComposeExtensionAttachment GetAttachment(string title, string body)
        {
            var previewCard = new ThumbnailCard
            {
                Title = title,
                Text = body,
                //Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
            };

            string json = cardJson.Replace("**replace here**", HttpUtility.JavaScriptStringEncode(body)).Replace("\\r", "");
            AdaptiveCard mainCard = AdaptiveCard.FromJson(json).Card;

            return new ComposeExtensionAttachment(
                contentType: AdaptiveCard.ContentType,
                content: mainCard,
                preview: previewCard.ToAttachment());
        }
    }
}
