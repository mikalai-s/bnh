using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Web.Models
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
            this.NumberOfPages = (int)Math.Ceiling(totalLength / (double)pageSize);
            this._pageItems = new Lazy<T[]>(() => items.Skip(pageIndex * pageSize).Take(pageSize).ToArray());
        }
    }
}