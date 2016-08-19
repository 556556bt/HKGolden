﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml;
using HKGoldenAPI;
using Windows.UI.Xaml.Media;
using HtmlAgilityPack;

namespace HKGolden
{
    public static class PostRenderer
    {
        public static RichTextBlock RenderHTML(string html)
        {
            RichTextBlock rtb = new RichTextBlock() { LineHeight = 0, Margin = new Thickness(10), FontSize = 18, Foreground = new SolidColorBrush() { Color = Windows.UI.Colors.White } };
            List<string> paraHtml = new List<string>();
            while (html.Contains("<br>"))
                paraHtml.Add(Utilities.ClearPostContent(Utilities.ReadUntil(ref html, "<br>")));
            if (html != "") paraHtml.Add(Utilities.ClearPostContent(html));
            foreach (string para in paraHtml)
            {
                Paragraph paragraph = new Paragraph() { Margin = new Thickness(0) , LineHeight = 0};
                HtmlDocument paragraphDocument = new HtmlDocument();
                paragraphDocument.LoadHtml(para);
                foreach (HtmlNode n in paragraphDocument.DocumentNode.ChildNodes)
                    switch (n.Name)
                    {
                        case "#text":
                            paragraph.Inlines.Add(new Run() { Text = n.InnerText });
                            break;
                        case "img":
                            paragraph.Inlines.Add(new Run() { Text = "[EMOJI " + n.Attributes["alt"].Value + "]" });
                            break;
                        case "a":
                            if (n.Descendants("img").Count() == 0)
                            {
                                paragraph.Inlines.Add(new Run() { Text = "[LINK " + n.Attributes["href"].Value + "]" });
                            }
                            else
                            {
                                paragraph.Inlines.Add(new Run() { Text = "[IMG " + n.Descendants("img").ElementAt(0).Attributes["src"].Value + "]" });
                            }
                            break;
                        default:
                            paragraph.Inlines.Add(new Run() { Text = "[UNKNOWN]" });
                            break;
                    }
                rtb.Blocks.Add(paragraph);
            }
            return rtb;
        }
    }
}