﻿namespace AngleSharp.Dom.Xml
{
    using AngleSharp.Events;
    using AngleSharp.Extensions;
    using AngleSharp.Html;
    using AngleSharp.Network;
    using AngleSharp.Parser.Xml;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a document node that contains only XML nodes.
    /// </summary>
    sealed class XmlDocument : Document, IXmlDocument
    {
        #region ctor

        internal XmlDocument(IBrowsingContext context, TextSource source)
            : base(context ?? BrowsingContext.New(), source)
        {
            ContentType = MimeTypeNames.Xml;
        }

        internal XmlDocument(IBrowsingContext context = null)
            : this(context, new TextSource(String.Empty))
        {
        }

        #endregion

        #region Properties

        public override IElement DocumentElement
        {
            get { return this.FindChild<IElement>(); }
        }

        public override String Title
        {
            get { return String.Empty; }
            set { }
        }

        #endregion

        #region Methods

        public override INode Clone(Boolean deep = true)
        {
            var node = new XmlDocument(Context, new TextSource(Source.Text));
            CloneDocument(node, deep);
            return node;
        }

        internal async static Task<IDocument> LoadAsync(IBrowsingContext context, CreateDocumentOptions options, CancellationToken cancelToken)
        {
            var document = new XmlDocument(context, options.Source);
            var parser = new XmlDomBuilder(document);
            var parserOptions = new XmlParserOptions { };
            var parseEvent = new AngleSharp.Events.HtmlParseStartEvent(document);//TODO TRANSFORM
            document.Setup(options);
            context.NavigateTo(document);
            context.FireSimpleEvent(EventNames.ParseStart);
            await parser.ParseAsync(default(XmlParserOptions), cancelToken).ConfigureAwait(false);
            context.FireSimpleEvent(EventNames.ParseEnd);
            return document;
        }

        #endregion
    }
}
