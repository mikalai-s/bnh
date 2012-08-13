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
        /// Collection of items to split into pages
        /// </summary>
        public IEnumerable<T> Items { get; private set; }

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
        public IEnumerable<T> PageItems { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalLength"></param>
        /// <param name="items"></param>
        public Pager(int pageIndex, int pageSize, int totalLength, IEnumerable<T> items)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalLength = totalLength;
            this.Items = items;
            this.NumberOfPages = (int)Math.Ceiling(totalLength / (double)pageSize);
            this.PageItems = items.Skip(pageIndex * pageSize).Take(pageSize);
        }
    }
}