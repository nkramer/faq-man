using System;
using System.Linq;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using AdaptiveCards;

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
- The URL and http verb for the method of interest (eg: POST https://graph.microsoft.com/beta/teams/{id}/channels)
- Application permissions or user delegated permissions?
- The request - id from a call thats failed within the last 24 hours (we only keep a few days of logs)
- The payload you passed in, and the payload you got back
- The http response code
- If it was a 401 or 403, why do you think this was a mistake? What permission scopes are you calling it with?

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

            var response = new ComposeExtensionResponse(new ComposeExtensionResult(
                attachmentLayout: "list", 
                type: "result", 
                attachments: attachments.ToList()));
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

        //private static ComposeExtensionAttachment GetAttachment(string title, string body)
        //{
        //    var previewCard = new ThumbnailCard
        //    {
        //        Title = title,
        //        Text = body,
        //        //Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
        //    };

        //    return previewCard
        //        .ToAttachment()
        //        .ToComposeExtensionAttachment();
        //}

        //private static ComposeExtensionAttachment GetAttachment(string title, string body)
        //{
        //    var previewCard = new ThumbnailCard
        //    {
        //        Title = title,
        //        Text = body,
        //        //Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
        //    };

        //    var mainCard = new ThumbnailCard
        //    {
        //        Title = title,
        //        Text = body,
        //        //Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
        //    };

        //    return new ComposeExtensionAttachment(
        //        //contentType: AdaptiveCard.ContentType, //HeroCard.ContentType,
        //        //content: attachment,
        //        contentType: ThumbnailCard.ContentType,
        //        content: mainCard,
        //        preview: previewCard.ToAttachment());
        //}

        private static ComposeExtensionAttachment GetAttachment(string title, string body)
        {
            var previewCard = new ThumbnailCard
            {
                Title = title,
                Text = body,
                //Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
            };

            string json = cardJson.Replace("**replace here**", body);
            AdaptiveCard mainCard = AdaptiveCard.FromJson(json).Card;

            return new ComposeExtensionAttachment(
                //contentType: AdaptiveCard.ContentType, //HeroCard.ContentType,
                //content: attachment,
                contentType: AdaptiveCard.ContentType,
                content: mainCard,
                preview: previewCard.ToAttachment());
        }

        //private static ComposeExtensionAttachment GetAttachment(string title, string body)
        //{
        //    string json = cardJson.Replace("**replace here**", body);
        //    var parseResult = AdaptiveCard.FromJson(json);
        //    var attachment = new Attachment
        //    {
        //        ContentType = AdaptiveCard.ContentType,
        //        Content = parseResult.Card,
        //    };
        //    attachment.Name = "name";

        //    var previewCard = new ThumbnailCard
        //    {
        //        Title = !string.IsNullOrWhiteSpace(title) ? title : Faker.Lorem.Sentence(),
        //        Text = body,
        //        //Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
        //    };

        //    return new ComposeExtensionAttachment(
        //        //contentType: AdaptiveCard.ContentType, //HeroCard.ContentType,
        //        //content: attachment,
        //        contentType: HeroCard.ContentType,
        //        content: previewCard.ToAttachment(),
        //        preview: previewCard.ToAttachment());

        //    //return attachment.ToComposeExtensionAttachment();
        //    //return parseResult.Card.

        //    //var welcomeMessage = Activity.CreateMessageActivity();
        //    //welcomeMessage.Attachments.Add(attachment);



        //    //return card
        //    //    .ToAttachment()
        //    //    .ToComposeExtensionAttachment();
        //}

        //private static ComposeExtensionAttachment GetAttachment(string title, string body)
        //{
        //    string json = cardJson.Replace("**replace here**", body);
        //    var parseResult = AdaptiveCard.FromJson(json);
        //    var attachment = new Attachment
        //    {
        //        ContentType = AdaptiveCard.ContentType,
        //        Content = parseResult.Card,
        //    };
        //    attachment.Name = "name";

        //    return attachment.ToComposeExtensionAttachment();
        //    //return parseResult.Card.

        //    //var welcomeMessage = Activity.CreateMessageActivity();
        //    //welcomeMessage.Attachments.Add(attachment);


        //    //var card = new A
        //    //var card = new ThumbnailCard
        //    //{
        //    //    Title = !string.IsNullOrWhiteSpace(title) ? title : Faker.Lorem.Sentence(),
        //    //    Text = body,
        //    //   //Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
        //    //};

        //    //return card
        //    //    .ToAttachment()
        //    //    .ToComposeExtensionAttachment();
        //}
    }
}
