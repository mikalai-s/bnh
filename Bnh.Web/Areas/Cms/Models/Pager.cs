using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models
{
    /// <summary>
    /// Encapsulates information about sequence of pages
    /// </summary>
    /// <typeparam name="T">Type of a page instance</typeparam>
    public class Pager<T>
    {
        /// <summary>
        /// Total number of items in collection
        /// </summary>
        public int TotalLength { get; private set; }

        /// <summary>
        /// Number of items in a page
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Get number of pages to fit entire collection
        /// </summary>
        public int NumberOfPages { get; private set; }

        /// <summary>
        /// Gets current page index
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Gets items on current page
        /// </summary>
        public T[] PageItems { get { return this._pageItems.Value; } }

        /// <summary>
        /// Helps to evaluate PageItems only when it needs
        /// </summary>
        private Lazy<T[]> _pageItems = null;

        public IEnumerable<Link> Links
        {
            get
            {
                if (this.PageIndex == 0)
                {
                    yield return new Link { Text = "<< First", Disabled = true };
                    yield return new Link { Text = "< Previous", Disabled = true };
                }
                else
                {
                    yield return new Link { Text = "<< First", Action = "Reviews", PageIndex = 0 };
                    yield return new Link { Text = "< Previous", Action = "Reviews", PageIndex = this.PageIndex - 1 };
                }
                for (var i = 0; i < this.NumberOfPages; i++)
                {
                    if (this.PageIndex == i)
                    {
                        yield return new Link { Text = (i + 1).ToString(), Active = true, Numeric = true };
                    }
                    else
                    {
                        yield return new Link { Text = (i + 1).ToString(), Action = "Reviews", PageIndex = i, Numeric = true };
                    }
                }
                if (this.PageIndex == this.NumberOfPages - 1)
                {
                    yield return new Link { Text = "Next >", Disabled = true };
                    yield return new Link { Text = "Last >>", Disabled = true };
                }
                else
                {
                    yield return new Link { Text = "Next >", Action = "Reviews", PageIndex = this.PageIndex + 1 };
                    yield return new Link { Text = "Last >>", Action = "Reviews", PageIndex = this.NumberOfPages - 1 };
                }
            }
        }


        /// <summary>
        /// Creates instance of Pager object
        /// </summary>
        /// <param name="pageIndex">Index of current page</param>
        /// <param name="pageSize">Size of a page</param>
        /// <param name="totalLength">total number of items in collection</param>
        /// <param name="items">Items to split into pages</param>
        public Pager(int pageIndex, int pageSize, int totalLength, IEnumerable<T> items)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalLength = totalLength;
            this.NumberOfPages = (int)Math.Ceiling((totalLength == 0 ? 1 : totalLength) / (double)pageSize);
            this._pageItems = new Lazy<T[]>(() => items.Skip(pageIndex * pageSize).Take(pageSize).ToArray());
        }


        public struct Link
        {
            public string Text { get; set; }
            public string Action { get; set; }
            public int PageIndex { get; set; }
            public bool Disabled { get; set; }
            public bool Active { get; set; }
            public bool Numeric { get; set; }
        }
    }
}